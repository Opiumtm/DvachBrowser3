using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DvachBrowser3_TextRender_Native;

namespace DvachBrowser3.TextRender
{
    internal sealed class RenderCallbackWrapper : IRenderCallback
    {
        private readonly ITextRender2RenderCallback src;

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Object"/>.
        /// </summary>
        public RenderCallbackWrapper(ITextRender2RenderCallback src)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            this.src = src;
        }

        public void RenderLinkCallback(FrameworkElement result, ILinkData linkAttribute)
        {
            src.RenderLinkCallback(result, linkAttribute.RawData as ITextRenderLinkAttribute);
        }

        public FontFamily FixedFont => CourierNew;

        private static readonly FontFamily SegoeUi = new FontFamily("Segoe UI");

        private static readonly FontFamily CourierNew = new FontFamily("Courier New");

        public FontFamily Font => SegoeUi;

        public object RawData => src;

        public Brush PostLinkTextBrush => src.PostLinkTextBrush;

        public Brush PostQuoteTextBrush => src.PostQuoteTextBrush;

        public Brush PostSpoilerTextBrush => src.PostSpoilerTextBrush;

        public Brush PostSpoilerBackgroundBrush => src.PostSpoilerBackgroundBrush;

        public Brush PostNormalTextBrush => src.PostNormalTextBrush;

        public double FontSize => src.PostFontSize;
    }
}