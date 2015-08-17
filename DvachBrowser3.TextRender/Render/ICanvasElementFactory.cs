using Windows.UI.Xaml;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Фабрика элементов канвы.
    /// </summary>
    public interface ICanvasElementFactory
    {
        /// <summary>
        /// Создать элемент.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Элемент.</returns>
        FrameworkElement Create(ITextRenderCommand command);
    }
}