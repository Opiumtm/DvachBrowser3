#pragma once

using namespace Platform::Metadata;

namespace DvachBrowser3_TextRender_Native
{
	[Flags]
	public enum class TextAttributeFlags : unsigned int
	{
		Bold = 0x0001,
		Italic = 0x0002,
		Fixed = 0x0004,
		Subscript = 0x0008,
		Superscript = 0x0010
	};
}