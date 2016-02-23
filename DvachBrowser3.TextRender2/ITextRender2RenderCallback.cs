using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Обратный вызов рендеринга текста.
    /// </summary>
    public interface ITextRender2RenderCallback
    {
        /// <summary>
        /// Обратный вызов для ссылки.
        /// </summary>
        /// <param name="result">Результат рендеринга.</param>
        /// <param name="linkAttribute">Ссылка.</param>
        void RenderLinkCallback(FrameworkElement result, ITextRenderLinkAttribute linkAttribute);

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        double PostFontSize { get; }

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        Brush PostNormalTextBrush { get; }

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        Color PostNormalTextColor { get; }

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        Brush PostSpoilerBackgroundBrush { get; }

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        Color PostSpoilerBackgroundColor { get; }

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        Brush PostSpoilerTextBrush { get; }

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        Color PostSpoilerTextColor { get; }

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        Brush PostQuoteTextBrush { get; }

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        Color PostQuoteTextColor { get; }

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        Brush PostLinkTextBrush { get; }

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        Color PostLinkTextColor { get; }

        /// <summary>
        /// Идентификатор программы.
        /// </summary>
        Guid ProgramId { get; }
    }
}