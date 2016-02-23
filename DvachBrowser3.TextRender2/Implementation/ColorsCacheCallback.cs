using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Кэш цветов для рендеринга.
    /// </summary>
    public sealed class ColorsCacheCallback : ITextRender2RenderCallback
    {
        private readonly ITextRender2RenderCallback source;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="source">Источник.</param>
        public ColorsCacheCallback(ITextRender2RenderCallback source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            this.source = source;
            postFontSize = new Lazy<double>(() => source.PostFontSize);
            postNormalTextBrush = new Lazy<Brush>(() => source.PostNormalTextBrush);
            postNormalTextColor = new Lazy<Color>(() => source.PostNormalTextColor);
            postSpoilerBackgroundBrush = new Lazy<Brush>(() => source.PostSpoilerBackgroundBrush);
            postSpoilerBackgroundColor = new Lazy<Color>(() => source.PostSpoilerBackgroundColor);
            postSpoilerTextBrush = new Lazy<Brush>(() => source.PostSpoilerTextBrush);
            postSpoilerTextColor = new Lazy<Color>(() => source.PostSpoilerTextColor);
            postQuoteTextBrush = new Lazy<Brush>(() => source.PostQuoteTextBrush);
            postQuoteTextColor = new Lazy<Color>(() => source.PostQuoteTextColor);
            postLinkTextBrush = new Lazy<Brush>(() => source.PostLinkTextBrush);
            postLinkTextColor = new Lazy<Color>(() => source.PostLinkTextColor);
        }

        /// <summary>
        /// Обратный вызов для ссылки.
        /// </summary>
        /// <param name="result">Результат рендеринга.</param>
        /// <param name="linkAttribute">Ссылка.</param>
        public void RenderLinkCallback(FrameworkElement result, ITextRenderLinkAttribute linkAttribute)
        {
            source.RenderLinkCallback(result, linkAttribute);
        }

        private readonly Lazy<double> postFontSize;

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        public double PostFontSize => postFontSize.Value;

        private readonly Lazy<Brush> postNormalTextBrush;

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        public Brush PostNormalTextBrush => postNormalTextBrush.Value;

        private readonly Lazy<Color> postNormalTextColor;

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        public Color PostNormalTextColor => postNormalTextColor.Value;

        private readonly Lazy<Brush> postSpoilerBackgroundBrush;

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        public Brush PostSpoilerBackgroundBrush => postSpoilerBackgroundBrush.Value;

        private readonly Lazy<Color> postSpoilerBackgroundColor;

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        public Color PostSpoilerBackgroundColor => postSpoilerBackgroundColor.Value;

        private readonly Lazy<Brush> postSpoilerTextBrush;

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        public Brush PostSpoilerTextBrush => postSpoilerTextBrush.Value;

        private readonly Lazy<Color> postSpoilerTextColor;

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        public Color PostSpoilerTextColor => postSpoilerTextColor.Value;

        private readonly Lazy<Brush> postQuoteTextBrush;

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        public Brush PostQuoteTextBrush => postQuoteTextBrush.Value;

        private readonly Lazy<Color> postQuoteTextColor;

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        public Color PostQuoteTextColor => postQuoteTextColor.Value;

        private readonly Lazy<Brush> postLinkTextBrush;

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        public Brush PostLinkTextBrush => postLinkTextBrush.Value;

        private readonly Lazy<Color> postLinkTextColor;

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        public Color PostLinkTextColor => postLinkTextColor.Value;

        /// <summary>
        /// Идентификатор программы.
        /// </summary>
        public Guid ProgramId => source.ProgramId;
    }
}