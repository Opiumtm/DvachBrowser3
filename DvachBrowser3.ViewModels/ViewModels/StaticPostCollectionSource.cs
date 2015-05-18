using System.Threading;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Статический источник коллекции постов.
    /// </summary>
    public sealed class StaticPostCollectionSource : IPostCollectionSource
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kind">Тип коллекции.</param>
        /// <param name="preloadedCollection">Коллекция.</param>
        public StaticPostCollectionSource(PostCollectionKind kind, PostTreeCollection preloadedCollection)
        {
            Kind = kind;
            PreloadedCollection = preloadedCollection;
        }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return new CancellationToken();
        }

        public PostCollectionKind Kind { get; private set; }

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