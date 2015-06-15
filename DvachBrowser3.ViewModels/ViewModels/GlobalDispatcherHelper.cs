using Windows.UI.Core;
using Windows.UI.Xaml;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Помощник глобальной диспетчеризации.
    /// </summary>
    public static class GlobalDispatcherHelper
    {
        /// <summary>
        /// Глобальный диспетчер.
        /// </summary>
        public static CoreDispatcher Dispatcher
        {
            get { return Window.Current.Dispatcher; }
        }
    }
}