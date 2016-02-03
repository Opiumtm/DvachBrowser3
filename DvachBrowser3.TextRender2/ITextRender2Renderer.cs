using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство рендеринга разметки.
    /// </summary>
    public interface ITextRender2Renderer
    {
        /// <summary>
        /// Обратный вызов.
        /// </summary>
        ITextRender2RenderCallback Callback { get; }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        /// <param name="map">Карта расположений.</param>
        /// <returns>Результат.</returns>
        FrameworkElement Render(ITextRender2MeasureMap map);
    }
}