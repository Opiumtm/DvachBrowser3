#pragma once

#include "TextAttributeFlags.h"
#include "MappingHelperArg.h"

using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::Graphics::Canvas::Text;
using namespace Windows::Foundation::Collections;

namespace DvachBrowser3_TextRender_Native
{
	public ref class MappingHelper sealed
	{
	private:
		void ApplyAttribute(CanvasTextLayout^ tl, MappingHelperArg^ attribute, double fontSize);
	public:
		MappingHelper() {};

		void ApplyAttributes(CanvasTextLayout^ tl, IVector<MappingHelperArg^>^ attributes, double fontSize);
	};
}