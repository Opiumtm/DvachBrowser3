using System;
using Windows.Foundation;
using DvachBrowser3_TextRender_Native;

namespace DvachBrowser3.TextRender
{
    internal sealed class NativeRenderArgument : IRenderArgument
    {
        public NativeRenderArgument(ITextRender2MeasureMap map, ITextRender2MeasureMapLine line, TextRender2MeasureMapElement el, ITextRender2RenderCallback callback)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (line == null) throw new ArgumentNullException(nameof(line));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var command = el.Command;
            string text;
            var textCnt = command.Content as ITextRenderTextContent;
            if (textCnt != null)
            {
                text = textCnt.Text ?? "";
            }
            else
            {
                text = "";
            }
            Text = text;

            ElementSize = el.Size;
            Placement = el.Placement;
            LineHeight = line.Height;
            StrikethrougKoef = map.StrikethrougKoef;

            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                var linkAttribute = command.Attributes.Attributes[CommonTextRenderAttributes.Link] as ITextRenderLinkAttribute;
                if (linkAttribute != null)
                {
                    Link = new LinkDataWrapper(linkAttribute);
                }
            }

            Callback = new RenderCallbackWrapper(callback);

            TextAttributeFlags flags = 0;

            var attr = command.Attributes.Attributes;

            if (attr.ContainsKey(CommonTextRenderAttributes.Link))
            {
                flags = flags | TextAttributeFlags.Link;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Bold))
            {
                flags = flags | TextAttributeFlags.Bold;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Fixed))
            {
                flags = flags | TextAttributeFlags.Fixed;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Italic))
            {
                flags = flags | TextAttributeFlags.Italic;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Overline))
            {
                flags = flags | TextAttributeFlags.Overline;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Quote))
            {
                flags = flags | TextAttributeFlags.Quote;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Spoiler))
            {
                flags = flags | TextAttributeFlags.Spoiler;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Strikethrough))
            {
                flags = flags | TextAttributeFlags.Strikethrough;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Subscript))
            {
                flags = flags | TextAttributeFlags.Subscript;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Superscript))
            {
                flags = flags | TextAttributeFlags.Superscript;
            }
            if (attr.ContainsKey(CommonTextRenderAttributes.Undeline))
            {
                flags = flags | TextAttributeFlags.Undeline;
            }
            Flags = flags;
        }

        public IRenderCallback Callback { get; }

        public Size ElementSize { get; }

        public Point Placement { get; }

        public ILinkData Link { get; }

        public TextAttributeFlags Flags { get; }

        public string Text { get; }

        public double LineHeight { get; }

        public double StrikethrougKoef { get; }
    }
}