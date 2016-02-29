#pragma once

using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Platform;

namespace DvachBrowser3_TextRender_Native
{
	public ref class MassChildUpdateHelper sealed
	{
	public:
		void UpdateChildren(UIElementCollection^ collection, const Array<UIElement^, 1>^ elements);
	};
}