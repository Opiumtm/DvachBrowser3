using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Источник коллекции постов.
    /// </summary>
    public interface IPostCollectionSource : ICancellationTokenSource
    {
        /// <summary>
        /// Тип коллекции.
        /// </summary>
        PostCollectionKind Kind { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Операция обновления (может быть null, если обновления не поддерживаются).
        /// </summary>
        INetworkViewModel<PostCollectionUpdateMode> UpdateOperation { get; }

        /// <summary>
        /// Поддерживаемые режимы обновления.
        /// </summary>
        SupportedPostCollectionUpdates SupportedUpdates { get; }

        /// <summary>
        /// Коллекция загружена.
        /// </summary>
        event PostCollectionLoadedEventHandler CollectionLoaded;

        /// <summary>
        /// Предварительно загруженная коллекция (null, если нет).
        /// </summary>
        PostTreeCollection PreloadedCollection { get; }

        /// <summary>
        /// Поддерживает постинг.
        /// </summary>
        bool AllowPosting { get; }

        /// <summary>
        /// Получить флаг моего поста.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Флаг.</returns>
        bool GetMyFlag(BoardLinkBase link);

        /// <summary>
        /// Загрузить из кэша (если нет PreloadedCollection).
        /// </summary>
        /// <returns>true, если тред есть в кэше.</returns>
        Task<bool> LoadFromCache();

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        /// <returns>Результат.</returns>
        Task<bool?> CheckForUpdates();

        /// <summary>
        /// Может проверять на апдейты.
        /// </summary>
        bool CanCheckForUpdates { get; }
    }
}