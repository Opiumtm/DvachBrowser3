using System;
using System.Threading;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Статический источник коллекции постов.
    /// </summary>
    public sealed class StaticPostCollectionSource : IPostCollectionSource
    {
        private readonly ICancellationTokenSource tokenSource;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kind">Тип коллекции.</param>
        /// <param name="preloadedCollection">Коллекция.</param>
        /// <param name="tokenSource">Источник токенов.</param>
        public StaticPostCollectionSource(PostCollectionKind kind, PostTreeCollection preloadedCollection, ICancellationTokenSource tokenSource = null)
        {
            if (preloadedCollection == null) throw new ArgumentNullException("preloadedCollection");
            Kind = kind;
            PreloadedCollection = preloadedCollection;
            this.tokenSource = tokenSource;
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
    }
}