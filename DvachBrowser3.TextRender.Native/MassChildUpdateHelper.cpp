#include "pch.h"
#include "MassChildUpdateHelper.h"

using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Platform;


namespace DvachBrowser3_TextRender_Native
{
	void MassChildUpdateHelper::UpdateChildren(UIElementCollection^ collection, const Array<UIElement^, 1>^ elements)
	{
		collection->ReplaceAll(elements);
	}
}