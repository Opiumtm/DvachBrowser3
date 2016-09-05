using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Styles;
using Ipatov.MarkupRender;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Стиль рендеринга.
    /// </summary>
    public sealed class DvachTextRenderStyle : ITextRenderStyle, IWeakEventCallback, IDisposable
    {
        private readonly Guid _callbackId;

        private IStyleManager _styleManager;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DvachTextRenderStyle()
        {
            _callbackId = Shell.IsNarrowViewChanged.AddCallback(this);
            _styleManager = StyleManagerFactory.Current.GetManager();
        }

        public string FontFace => "Segoe UI";

        public string FixedFontFace => "Consolas";

        public float FontSize => (float) (_styleManager?.Text?.PostFontSize ?? 14);

        private int? _maxLines;

        public int? MaxLines
        {
            get { return _maxLines; }
            set
            {
                _maxLines = value;
                StyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color NormalColor => (Application.Current.Resources["PostNormalTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        public Color QuoteColor => (Application.Current.Resources["PostQuoteTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        public Color SpoilerBackground => (Application.Current.Resources["PostSpoilerBackgroundBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        public Color SpoilerColor => (Application.Current.Resources["PostSpoilerTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        public Color LinkColor => (Application.Current.Resources["PostLinkTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        public event EventHandler StyleChanged;

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == Shell.IsNarrowViewChangedId)
            {
                StyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Shell.IsNarrowViewChanged.RemoveCallback(_callbackId);
            _styleManager = null;
        }
    }
}