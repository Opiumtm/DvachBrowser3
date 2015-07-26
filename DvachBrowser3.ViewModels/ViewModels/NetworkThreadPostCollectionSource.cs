using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Источник коллекции постов треда из сети.
    /// </summary>
    public sealed class NetworkThreadPostCollectionSource : ViewModelBase, IPostCollectionSource
    {
        private readonly ICancellationTokenSource tokenSource;

        private HashSet<string> myPosts;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kind">Тип коллекции.</param>
        /// <param name="link">Ссылка.</param>
        /// <param name="tokenSource">Источник токенов.</param>
        public NetworkThreadPostCollectionSource(PostCollectionKind kind, BoardLinkBase link,
            ICancellationTokenSource tokenSource = null)
        {
            if (link == null) throw new ArgumentNullException("link");
            Link = link;
            this.tokenSource = tokenSource;
            Kind = kind;
            supportedUpdates = new Lazy<SupportedPostCollectionUpdates>(GetSupportedUpdates);
            canCheckForUpdates = new Lazy<bool>(GetCheckForUpdates);
            updateOperation = new NetworkThreadPostCollectionUpdateOperation(Link, GetToken);
            updateOperation.SetResult += UpdateOperationOnSetResult;
        }

        private void UpdateOperationOnSetResult(object sender, PostCollectionLoadedEventArgs e)
        {
            if (e != null && e.Collection != null)
            {
                SetData(e.Collection, e.MyPosts, e.UpdateMode);
            }
        }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return tokenSource != null ? tokenSource.GetToken() : new CancellationToken();
        }

        /// <summary>
        /// Тип коллекции.
        /// </summary>
        public PostCollectionKind Kind { get; private set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; private set; }

        private readonly NetworkThreadPostCollectionUpdateOperation updateOperation;

        /// <summary>
        /// Операция обновления (может быть null, если обновления не поддерживаются).
        /// </summary>
        public INetworkViewModel<PostCollectionUpdateMode> UpdateOperation
        {
            get { return updateOperation; }
        }

        private SupportedPostCollectionUpdates GetSupportedUpdates()
        {
            try
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var flags = engines.GetEngineById(Link.Engine).Capability;
                var supportPartial = (flags & EngineCapability.PartialThreadRequest) != 0;
                return supportPartial ? SupportedPostCollectionUpdates.FullAndPartial : SupportedPostCollectionUpdates.FullOnly;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return SupportedPostCollectionUpdates.FullAndPartial;
            }
        }

        private bool GetCheckForUpdates()
        {
            try
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var flags = engines.GetEngineById(Link.Engine).Capability;
                return (flags & EngineCapability.LastModifiedRequest) != 0;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return false;
            }
        }

        private readonly Lazy<SupportedPostCollectionUpdates> supportedUpdates;

        private readonly Lazy<bool> canCheckForUpdates;

        /// <summary>
        /// Поддерживаемые режимы обновления.
        /// </summary>
        public SupportedPostCollectionUpdates SupportedUpdates
        {
            get { return supportedUpdates.Value; }
        }

        /// <summary>
        /// Коллекция загружена.
        /// </summary>
        public event PostCollectionLoadedEventHandler CollectionLoaded;

        private void OnCollectionLoaded(PostCollectionLoadedEventArgs e)
        {
            PostCollectionLoadedEventHandler handler = CollectionLoaded;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Предварительно загруженная коллекция (null, если нет).
        /// </summary>
        public PostTreeCollection PreloadedCollection
        {
            get { return null; }
        }

        /// <summary>
        /// Поддерживает постинг.
        /// </summary>
        public bool AllowPosting
        {
            get { return true; }
        }

        /// <summary>
        /// Получить флаг моего поста.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Флаг.</returns>
        public bool GetMyFlag(BoardLinkBase link)
        {
            if (myPosts == null)
            {
                return false;
            }
            return myPosts.Contains(ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
        }

        /// <summary>
        /// Загрузить из кэша (если нет PreloadedCollection).
        /// </summary>
        /// <returns>true, если тред есть в кэше.</returns>
        public async Task<bool> LoadFromCache()
        {
            var service = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var d = await service.ThreadData.LoadThread(Link);
            if (d != null)
            {
                var mp = await service.ThreadData.LoadMyPostsInfo(Link);
                SetData(d, mp, PostCollectionUpdateMode.FromCache);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        /// <returns>Результат.</returns>
        public async Task<bool?> CheckForUpdates()
        {
            try
            {
                var logic = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>();
                var operation = logic.CheckThread(Link);
                return await operation.Complete();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Может проверять на апдейты.
        /// </summary>
        public bool CanCheckForUpdates
        {
            get { return canCheckForUpdates.Value; }
        }

        private void SetData(PostTreeCollection collection, MyPostsInfo myPostsInfo, PostCollectionUpdateMode updateMode)
        {
            if (myPostsInfo != null)
            {
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                myPosts = new HashSet<string>((myPostsInfo.MyPosts ?? new List<BoardLinkBase>()).Select(hashService.GetLinkHash));
            }
            else
            {
                myPosts = new HashSet<string>();
            }
            OnCollectionLoaded(new PostCollectionLoadedEventArgs(collection, myPostsInfo, updateMode));
        }
    }
}