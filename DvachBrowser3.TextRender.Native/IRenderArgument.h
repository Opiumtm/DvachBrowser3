#pragma once

#include "ILinkData.h"
#include "IRenderCallback.h"
#include "TextAttributeFlags.h"

using namespace Platform;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Windows::Foundation;

namespace DvachBrowser3_TextRender_Native
{
	public interface struct IRenderArgument
	{
	public:
		property double StrikethrougKoef { double get(); };

		property double LineHeight { double get(); };

		property String^ Text { String^ get(); };

		property TextAttributeFlags Flags { TextAttributeFlags get(); };

		property ILinkData^ Link { ILinkData^ get(); };

		property Point Placement { Point get(); };

		property Size ElementSize { Size get(); };

		property IRenderCallback^ Callback { IRenderCallback^ get(); };		
	};
}