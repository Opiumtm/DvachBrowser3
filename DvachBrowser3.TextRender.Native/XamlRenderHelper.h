#pragma once

#include "ILinkData.h"
#include "IRenderCallback.h"
#include "TextAttributeFlags.h"
#include "IRenderArgument.h"

using namespace Platform;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Windows::Foundation;

namespace DvachBrowser3_TextRender_Native
{
	public ref class XamlRenderHelper sealed
	{
	public:
		UIElement^ RenderElement(IRenderArgument^ arg);
	};
}