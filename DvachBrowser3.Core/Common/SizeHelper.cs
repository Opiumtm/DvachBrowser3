using Windows.Foundation;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник с размерами.
    /// </summary>
    public static class SizeHelper
    {
        /// <summary>
        /// Изменить размеры так, чтобы они входили в границы.
        /// </summary>
        /// <param name="src">Исходные размеры.</param>
        /// <param name="bounds">Границы.</param>
        /// <returns></returns>
        public static Size ScaleTo(this Size src, Size bounds)
        {
            if (src.IsEmpty || bounds.IsEmpty)
            {
                return new Size();
            }
            var h0 = src.Height;
            var w0 = src.Width;

            var h1 = bounds.Height;
            var w1 = bounds.Width;

            var h2 = h1;
            var w2 = h0 > 0.01 ? h1/h0*w0 : 0;

            var h3 = w0 > 0.01 ? w1/w0*h0 : 0;
            var w3 = w1;

            return w2 <= w1 ? new Size(w2, h2) : new Size(w3, h3);
        }
    }
}