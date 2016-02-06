namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель треда.
    /// </summary>
    public interface IThreadTileViewModel
    {
        /// <summary>
        /// Борда.
        /// </summary>
        string Board { get; } 

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        IImageSourceViewModel Image { get; }
    }
}