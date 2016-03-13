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
	public interface struct IUIElementWithCoord
	{
	public:
		property UIElement^ Element { UIElement^ get(); };
		property float X { float get(); };
		property float Y { float get(); };
	};

	public ref class XamlRenderHelper sealed
	{
	public:
		UIElement^ RenderElement(IRenderArgument^ arg);
		IUIElementWithCoord^ RenderElementWithCoord(IRenderArgument^ arg);
		void RenderToCanvas(Canvas^ canvas, const Array<IRenderArgument^, 1>^ elements);
	};
}