#pragma once

using namespace Platform;

namespace DvachBrowser3_TextRender_Native
{
	public interface struct ILinkData
	{
	public:
		property String^ Uri { String^ get(); };

		property Object^ CustomData { Object^ get(); };

		property Object^ RawData { Object^ get(); };
	};
}