using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Текст поста.
    /// </summary>
    public interface IPostTextViewModel : IPostPartViewModel, ILinkClickCallback
    {
        /// <summary>
        /// Отобразить текст.
        /// </summary>
        /// <param name="logic">Логика рендеринга поста.</param>
        void RenderText(ITextRenderLogic logic);

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        event LinkClickEventHandler LinkClick;
    }
}