using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция загрузки архива.
    /// </summary>
    public class DownloadArchiveOperation : NetworkLogicOperation<Guid, DownloadArchiveOperationParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public DownloadArchiveOperation(IServiceProvider services, DownloadArchiveOperationParameter parameter)
            : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<Guid> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.Link);
            var storage = Services.GetServiceOrThrow<IStorageService>();
            var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
            var threadProcessService = Services.GetServiceOrThrow<IThreadTreeProcessService>();
            var linkHashService = Services.GetService<ILinkHashService>();
            var threadLink = linkTransformService.ThreadLinkFromThreadPartLink(Parameter.Link);
            if (threadLink == null)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            dynamic other1 = new ExpandoObject();
            other1.Kind = "ARCHIVE";
            OnProgress(new EngineProgress(engine.EngineUriService.GetBrowserLink(threadLink).ToString(), 0.0, other1));
            token.ThrowIfCancellationRequested();
            var threadOperation = engine.GetThread(threadLink);
            var threadResult = await threadOperation.Complete(token);
            if (threadResult == null || threadResult.CollectionResult == null)
            {
                throw new WebException("Неизвестная ошибка получения треда");
            }
            var thread = threadResult.CollectionResult as ThreadTree;
            if (thread == null)
            {
                throw new WebException("Неизвестная ошибка получения треда");
            }
            threadProcessService.SetBackLinks(thread);
            threadProcessService.SortThreadTree(thread);
            var archive = new ArchiveThreadTree()
            {
                Extensions = thread.Extensions,
                LastModified = thread.LastModified,
                Link = threadLink,
                ParentLink = thread.ParentLink,
                Posts = thread.Posts ?? new List<PostTree>(),
                Reference = new ArchiveReference()
                {
                    ArchiveDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    ThreadInfo = threadProcessService.GetShortInfo(thread)
                }
            };
            await storage.Archives.SaveArchive(archive);
            var thumbnails = archive
                .Posts
                .SelectMany(p => p.Media ?? new List<PostMediaBase>())
                .OfType<PostImageWithThumbnail>()
                .Where(m => m.Thumbnail != null)
                .Select(m => m.Thumbnail.Link)
                .Distinct(linkHashService.GetComparer())
                .ToArray();
            int counter = 0;
            foreach (var t in thumbnails)
            {
                dynamic other = new ExpandoObject();
                other.Kind = "ARCHIVE";
                var percent = ((double) counter + 1)/thumbnails.Length*100.0;
                OnProgress(new EngineProgress(engine.EngineUriService.GetBrowserLink(t).ToString(), percent, other));
                token.ThrowIfCancellationRequested();
                try
                {
                    var mediaOperation = engine.GetMediaFile(t);
                    var result = await mediaOperation.Complete(token);
                    await storage.Archives.MoveToMediaStorage(archive.Reference.Id, t, result.TempFile);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
            return archive.Reference.Id;
        }
    }
}