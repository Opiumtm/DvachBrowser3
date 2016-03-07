using Windows.UI.Xaml;

namespace DvachBrowser3
{
    /// <summary>
    /// Обратная связь с компилированными привязками.
    /// </summary>
    public interface IBindingControlCallback
    {
        /// <summary>
        /// Обновить привязки.
        /// </summary>
        void UpdateBindings();

        /// <summary>
        /// Прекратить слежение привязок.
        /// </summary>
        void StopTrackingBindings();
    }

    /// <summary>
    /// Класс-помщник по связи с привязками.
    /// </summary>
    public static class BindingControlHelper
    {
        /// <summary>
        /// Привязать прекращение слежения привязок.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект.</param>
        public static void AttachBindingRelease<T>(this T obj) where T : FrameworkElement, IBindingControlCallback
        {
            if (obj != null)
            {
            }
        }
    }
}