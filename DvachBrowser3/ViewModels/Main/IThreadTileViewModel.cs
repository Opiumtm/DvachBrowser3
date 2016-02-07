using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель треда.
    /// </summary>
    public interface IThreadTileViewModel
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Борда.
        /// </summary>
        string Board { get; } 

        /// <summary>
        /// Номер треда.
        /// </summary>
        int ThreadNumber { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        IImageSourceViewModel Image { get; }

        /// <summary>
        /// Есть новые посты.
        /// </summary>
        bool HasNewPosts { get; }

        /// <summary>
        /// Новые посты.
        /// </summary>
        int NewPosts { get; }
    }
}