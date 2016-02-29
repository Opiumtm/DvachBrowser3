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

		Superscript = 0x0010,
		Spoiler = 0x0020,
		Quote = 0x0040,
		Link = 0x0080,

		Undeline = 0x0100,
		Overline = 0x0200,
		Strikethrough = 0x0400
	};
}