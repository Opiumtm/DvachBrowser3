using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Статический источник коллекции постов.
    /// </summary>
    public sealed class StaticPostCollectionSource : IPostCollectionSource
    {
        private readonly ICancellationTokenSource tokenSource;

        private readonly HashSet<string> myPosts;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kind">Тип коллекции.</param>
        /// <param name="preloadedCollection">Коллекция.</param>
        /// <param name="myPosts">Мои посты.</param>
        /// <param name="allowPosting">Поддерживает постинг.</param>
        /// <param name="tokenSource">Источник токенов.</param>
        public StaticPostCollectionSource(PostCollectionKind kind, PostTreeCollection preloadedCollection, MyPostsInfo myPosts, bool allowPosting, ICancellationTokenSource tokenSource = null)
        {
            if (preloadedCollection == null) throw new ArgumentNullException("preloadedCollection");
            Kind = kind;
            PreloadedCollection = preloadedCollection;
            AllowPosting = allowPosting;
            this.tokenSource = tokenSource;
            if (myPosts != null)
            {
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                this.myPosts = new HashSet<string>((myPosts.MyPosts ?? new List<BoardLinkBase>()).Select(hashService.GetLinkHash));
            }
            else
            {
                this.myPosts = new HashSet<string>();
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

        public PostCollectionKind Kind { get; private set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link
        {
            get { return PreloadedCollection.Link; }
        }

        /// <summary>
        /// Операция обновления (может быть null, если обновления не поддерживаются).
        /// </summary>
        public INetworkViewModel<PostCollectionUpdateMode> UpdateOperation
        {
            get { return null; }
        }

        /// <summary>
        /// Поддерживаемые режимы обновления.
        /// </summary>
        public SupportedPostCollecetionUpdates SupportedUpdates
        {
            get
            {
                return SupportedPostCollecetionUpdates.None;
            }
        }

        /// <summary>
        /// Коллекция загружена.
        /// </summary>
        public event PostCollectionLoadedEventHandler CollectionLoaded;

        /// <summary>
        /// Предварительно загруженная коллекция (null, если нет).
        /// </summary>
        public PostTreeCollection PreloadedCollection { get; private set; }

        /// <summary>
        /// Поддерживает постинг.
        /// </summary>
        public bool AllowPosting { get; private set; }

        /// <summary>
        /// Получить флаг моего поста.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Флаг.</returns>
        public bool GetMyFlag(BoardLinkBase link)
        {
            return myPosts.Contains(ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
        }
    }
}