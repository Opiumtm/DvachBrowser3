using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;
using Nito.AsyncEx;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция постинга.
    /// </summary>
    public class PostOperation : NetworkLogicOperation<BoardLinkBase, PostOperationParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public PostOperation(IServiceProvider services, PostOperationParameter parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Лок постинга.
        /// </summary>
        protected static readonly AsyncLock PostLock = new AsyncLock();

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<BoardLinkBase> Complete(CancellationToken token)
        {
            SignalProcessing("Ожидание очереди постинга...", "WAIT");
            using (await PostLock.LockAsync())
            {
                if (Parameter.Data is DraftPostingData)
                {
                    throw new ArgumentException("Прямой постинг черновиков не поддерживается");
                }
                if (Parameter.Data.Link == null)
                {
                    throw new ArgumentException("Не указано, куда отправлять сообщение");
                }
                var engine = GetEngine(Parameter.Data.Link);
                var storage = Services.GetServiceOrThrow<IStorageService>();
                var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
                var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
                var mediaIds = new PostingMediaFile[0];
                if (Parameter.Data.FieldData.ContainsKey(PostingFieldSemanticRole.MediaFile))
                {
                    var d = Parameter.Data.FieldData[PostingFieldSemanticRole.MediaFile] as PostingMediaFiles;
                    if (d != null && d.Files != null)
                    {
                        mediaIds = d.Files.Where(f => f != null).ToArray();                    
                    }
                }
                var mediaFiles = new List<MediaFilePostingData>();
                foreach (var id in mediaIds)
                {
                    var file = await storage.PostData.MediaStorage.GetMediaFile(id.MediaFileId);
                    if (file != null)
                    {
                        if (file.Value.File != null)
                        {
                            mediaFiles.Add(new MediaFilePostingData()
                            {
                                FileName = id.OriginalName,
                                TempFile = file.Value.File
                            });                            
                        }
                    }
                }
                var entryData = new PostEntryData()
                {
                    Link = Parameter.Data.Link,
                    CommonData = new Dictionary<PostingFieldSemanticRole, object>(),
                    Captcha = Parameter.Captcha,
                    MediaFiles = mediaFiles.ToArray()
                };
                foreach (var p in Parameter.Data.FieldData)
                {
                    if (p.Key != PostingFieldSemanticRole.MediaFile)
                    {
                        entryData.CommonData[p.Key] = p.Value;
                    }
                }
                var postOperation = engine.Post(entryData);
                postOperation.Progress += (sender, e) => OnProgress(e);
                var result = await postOperation.Complete(token);
                try
                {
                    await storage.PostData.DeletePostingData(Parameter.Data.Link);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
                foreach (var f in mediaIds)
                {
                    try
                    {
                        await storage.PostData.MediaStorage.DeleteMediaFile(f.MediaFileId);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
                if (result.PostLink != null)
                {
                    if (Parameter.Data.Link.LinkKind == BoardLinkKind.Thread)
                    {
                        var threadLink = linkTransformService.ThreadLinkFromThreadPartLink(Parameter.Data.Link);
                        if (threadLink != null)
                        {
                            var myPosts = await storage.ThreadData.LoadMyPostsInfo(threadLink);
                            if (myPosts == null)
                            {
                                myPosts = new MyPostsInfo() { Link = threadLink, MyPosts = new List<BoardLinkBase>() };
                            }
                            if (myPosts.MyPosts == null)
                            {
                                myPosts.MyPosts = new List<BoardLinkBase>();
                            }
                            myPosts.MyPosts.Add(result.PostLink);
                            myPosts.MyPosts = myPosts.MyPosts.Distinct(linkHashService.GetComparer()).ToList();
                            await storage.ThreadData.SaveMyPostsInfo(myPosts);
                        }
                    }
                    return result.PostLink;
                }
                if (result.RedirectLink != null)
                {
                    return result.RedirectLink;
                }
                return null;
            }
        }
    }
}