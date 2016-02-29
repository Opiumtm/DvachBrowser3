#include "pch.h"
#include "ILinkData.h"
#include "IRenderCallback.h"
#include "TextAttributeFlags.h"
#include "IRenderArgument.h"
#include "XamlRenderHelper.h"

using namespace Platform;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Text;
using namespace Windows::Foundation;

namespace DvachBrowser3_TextRender_Native
{
	UIElement^ XamlRenderHelper::RenderElement(IRenderArgument^ arg)
	{
		auto r = ref new TextBlock();
		r->FontFamily = arg->Callback->Font;
		r->TextWrapping = TextWrapping::NoWrap;
		r->TextTrimming = TextTrimming::None;
		r->Foreground = arg->Callback->PostNormalTextBrush;
		r->FontSize = arg->Callback->FontSize;
		r->IsTextSelectionEnabled = false;
		r->TextAlignment = TextAlignment::Left;
		r->Text = arg->Text;

		FrameworkElement^ result = r;

		Border^ b = nullptr;
		Grid^ g = nullptr;
		Grid^ g2 = nullptr;
		Border^ strikeBorder = nullptr;

		auto flags = arg->Flags;

		if ((flags & TextAttributeFlags::Bold) == TextAttributeFlags::Bold)
		{
			r->FontWeight = FontWeights::Bold;
		}
		if ((flags & TextAttributeFlags::Italic) == TextAttributeFlags::Italic)
		{
			r->FontStyle = FontStyle::Italic;
		}
		if ((flags & TextAttributeFlags::Fixed) == TextAttributeFlags::Fixed)
		{
			r->FontFamily = arg->Callback->FixedFont;
		}
		if ((flags & TextAttributeFlags::Spoiler) == TextAttributeFlags::Spoiler)
		{
			if (b == nullptr)
			{
				b = ref new Border();
			}
			b->BorderThickness = Thickness(0);
			b->Background = arg->Callback->PostSpoilerBackgroundBrush;
			r->Foreground = arg->Callback->PostSpoilerTextBrush;
		}
		if ((flags & TextAttributeFlags::Quote) == TextAttributeFlags::Quote)
		{
			r->Foreground = arg->Callback->PostQuoteTextBrush;
		}
		if ((flags & TextAttributeFlags::Link) == TextAttributeFlags::Link)
		{
			r->Foreground = arg->Callback->PostLinkTextBrush;
		}
		if (((flags & TextAttributeFlags::Undeline) == TextAttributeFlags::Undeline) || ((flags & TextAttributeFlags::Link) == TextAttributeFlags::Link))
		{
			if (b == nullptr)
			{
				b = ref new Border();
			}
			Thickness ot = b->BorderThickness;
			Thickness nt = Thickness(ot.Left, ot.Top, ot.Right, 1.2);
			b->BorderThickness = nt;
		}
		if ((flags & TextAttributeFlags::Overline) == TextAttributeFlags::Overline)
		{
			if (b == nullptr)
			{
				b = ref new Border();
			}
			Thickness ot = b->BorderThickness;
			Thickness nt = Thickness(ot.Left, 1.2, ot.Right, ot.Bottom);
			b->BorderThickness = nt;
		}
		if ((flags & TextAttributeFlags::Strikethrough) == TextAttributeFlags::Strikethrough)
		{
			if (g == nullptr)
			{
				g = ref new Grid();
			}
			strikeBorder = ref new Border();
			strikeBorder->Background = r->Foreground;
			strikeBorder->Height = 1.2;
			strikeBorder->HorizontalAlignment = HorizontalAlignment::Stretch;
			strikeBorder->VerticalAlignment = VerticalAlignment::Top;
			g->Children->Append(strikeBorder);
		}
		if (((flags & TextAttributeFlags::Subscript) == TextAttributeFlags::Subscript) || ((flags & TextAttributeFlags::Superscript) == TextAttributeFlags::Superscript))
		{
			if (g2 == nullptr)
			{
				g2 = ref new Grid();
			}
			double fh = arg->LineHeight;
			double fh2 = fh * 2.0 / 3.0;
			double delta = fh - fh2;
			r->FontSize = r->FontSize * 2.0 / 3.0;
			if (((flags & TextAttributeFlags::Subscript) == TextAttributeFlags::Subscript) && !((flags & TextAttributeFlags::Superscript) == TextAttributeFlags::Superscript))
			{
				g2->Padding = Thickness(0, delta, 0, 0);
			}
			else if (!((flags & TextAttributeFlags::Subscript) == TextAttributeFlags::Subscript) && ((flags & TextAttributeFlags::Superscript) == TextAttributeFlags::Superscript))
			{
				g2->Padding = Thickness(0, 0, 0, delta);
			}
			else
			{
				g2->Padding = Thickness(0, delta / 2.0, 0, delta / 2.0);
			}
		}

		if (strikeBorder != nullptr)
		{
			double oh = arg->ElementSize.Height;
			strikeBorder->Margin = Thickness(0, arg->StrikethrougKoef*oh, 0, 0);
		}

		if (g != nullptr)
		{
			g->Children->Append(result);
			result = g;
		}

		if (b != nullptr)
		{
			b->BorderBrush = r->Foreground;
			b->Child = result;
			result = b;
		}

		if (g2 != nullptr)
		{
			g2->Children->Append(result);
			result = g2;
		}

		Canvas::SetLeft(result, arg->Placement.X);
		Canvas::SetTop(result, arg->Placement.Y);

		ILinkData^ link = arg->Link;
		if (link != nullptr)
		{
			arg->Callback->RenderLinkCallback(result, link);
		}

		return result;
	}
}