using System;
using DvachBrowser3_TextRender_Native;

namespace DvachBrowser3.TextRender
{
    internal sealed class LinkDataWrapper : ILinkData
    {
        private readonly ITextRenderLinkAttribute src;

        public LinkDataWrapper(ITextRenderLinkAttribute src)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            this.src = src;
        }

        public object RawData => src;

        public object CustomData => src.CustomData;

        public string Uri => src.Uri;
    }
}