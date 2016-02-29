#pragma once

#include "TextAttributeFlags.h"

using namespace Platform;
using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::Graphics::Canvas::Text;
using namespace Windows::Foundation::Collections;

namespace DvachBrowser3_TextRender_Native
{
	public interface class MappingHelperArg
	{
	public:
		property int Index { int get(); };
		property String^ RenderString { String^ get(); };
		property TextAttributeFlags Flags { TextAttributeFlags get(); };
		property Object^ Command { Object^ get(); };
	};
}