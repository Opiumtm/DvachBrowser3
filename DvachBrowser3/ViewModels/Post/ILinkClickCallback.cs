using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обратный вызов клика на ссылку.
    /// </summary>
    public interface ILinkClickCallback
    {
        /// <summary>
        /// Обратный вызов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        void OnLinkClick(ITextRenderLinkAttribute link);
    }
}