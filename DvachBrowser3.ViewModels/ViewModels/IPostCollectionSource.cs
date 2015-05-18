using System;
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
        /// Операция обновления (может быть null, если обновления не поддерживаются).
        /// </summary>
        INetworkViewModel<PostCollectionUpdateMode> UpdateOperation { get; }

        /// <summary>
        /// Поддерживаемые режимы обновления.
        /// </summary>
        SupportedPostCollecetionUpdates SupportedUpdates { get; }

        /// <summary>
        /// Коллекция загружена.
        /// </summary>
        event PostCollectionLoadedEventHandler CollectionLoaded;

        /// <summary>
        /// Предварительно загруженная коллекция (null, если нет).
        /// </summary>
        PostTreeCollection PreloadedCollection { get; }
    }
}