namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Источник изображения с размером.
    /// </summary>
    public interface IImageSourceViewModelWithSize : IImageSourceViewModel
    {
        /// <summary>
        /// Высота.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        int Width { get; }
    }
}