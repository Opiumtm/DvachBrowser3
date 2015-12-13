using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для скроллинга.
    /// </summary>
    public static class ScrollViewHelper
    {
        /// <summary>
        /// Элемент находится в границах.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Результат проверки.</returns>
        public static bool IsInWindowBounds(this FrameworkElement element)
        {
            if (element == null)
            {
                return false;
            }
            try
            {
                var p = new Point(0, 0);
                var r = Window.Current.Bounds;
                var gt = element.TransformToVisual(Window.Current.Content);
                var ofs = gt.TransformPoint(p);
                var xr = (ofs.X + element.ActualWidth) >= 0 && ofs.X < r.Width;
                var yr = (ofs.Y + element.ActualHeight) >= 0 && ofs.Y < r.Height;
                return xr && yr;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Получить список индексов, видимых в окне.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Список индексов.</returns>
        public static IEnumerable<int> GetVisibleToWindowIndexes(this ItemsControl element)
        {
            var e = element?.ItemsSource as IEnumerable;
            if (e == null)
            {
                yield break;
            }
            int cnt = 0;
            foreach (var i in e)
            {
                var c = element.ContainerFromIndex(cnt) as FrameworkElement;
                if (c.IsInWindowBounds())
                {
                    yield return cnt;
                }
                cnt++;
            }
        }

        /// <summary>
        /// Получить список индексов, видимых в окне.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Список элементов.</returns>
        public static IEnumerable<Tuple<int, T>> GetVisibleToWindowElements<T>(this ItemsControl element) where T: class 
        {
            var e = element?.ItemsSource as IEnumerable;
            if (e == null)
            {
                yield break;
            }
            int cnt = 0;
            foreach (var i in e)
            {
                var c = element.ContainerFromIndex(cnt) as FrameworkElement;
                if (c.IsInWindowBounds())
                {
                    yield return new Tuple<int, T>(cnt, i as T);
                }
                cnt++;
            }
        }
    }
}