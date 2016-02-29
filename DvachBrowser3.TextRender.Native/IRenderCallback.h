#pragma once

#include "ILinkData.h"

using namespace Platform;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;

namespace DvachBrowser3_TextRender_Native
{
	public interface struct IRenderCallback
	{
	public:
		void RenderLinkCallback(FrameworkElement^ result, ILinkData^ linkAttribute);

		property double FontSize { double get(); };

		property Brush^ PostNormalTextBrush { Brush^ get(); };

		property Brush^ PostSpoilerBackgroundBrush { Brush^ get(); };

		property Brush^ PostSpoilerTextBrush { Brush^ get(); };

		property Brush^ PostQuoteTextBrush { Brush^ get(); };

		property Brush^ PostLinkTextBrush { Brush^ get(); };

		property Object^ RawData { Object^ get(); };

		property FontFamily^ Font { FontFamily^ get(); };

		property FontFamily^ FixedFont { FontFamily^ get(); };
	};
}