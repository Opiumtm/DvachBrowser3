#include "pch.h"
#include "MappingHelper.h"

#include "TextAttributeFlags.h"
#include "MappingHelperArg.h"

using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::Graphics::Canvas::Text;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Text;

namespace DvachBrowser3_TextRender_Native
{
	void MappingHelper::ApplyAttribute(CanvasTextLayout^ tl, MappingHelperArg^ attribute, double fontSize)
	{
		auto sz = attribute->RenderString->Length();
		TextAttributeFlags flags = attribute->Flags;
		int index = attribute->Index;
		if ((flags & TextAttributeFlags::Bold) == TextAttributeFlags::Bold)
		{
			tl->SetFontWeight(index, sz, FontWeights::Bold);
		}
		if ((flags & TextAttributeFlags::Italic) == TextAttributeFlags::Italic)
		{
			tl->SetFontStyle(index, sz, FontStyle::Italic);
		}
		if ((flags & TextAttributeFlags::Fixed) == TextAttributeFlags::Fixed)
		{
			tl->SetFontFamily(index, sz, "Courier New");
		}
		if (((flags & TextAttributeFlags::Subscript) == TextAttributeFlags::Subscript) || ((flags & TextAttributeFlags::Superscript) == TextAttributeFlags::Superscript))
		{
			float fs = fontSize;
			fs = fs * 2.0 / 3.0;
			tl->SetFontSize(index, sz, fs);
		}
		Platform::Object^ command = attribute->Command;
		tl->SetCustomBrush(index, sz, command);
	}

	void MappingHelper::ApplyAttributes(CanvasTextLayout^ tl, IVector<MappingHelperArg^>^ attributes, double fontSize)
	{
		for (int i = 0; i < attributes->Size; i++)
		{
			MappingHelperArg^ attribute = attributes->GetAt(i);
			ApplyAttribute(tl, attribute, fontSize);
		}
	}
}