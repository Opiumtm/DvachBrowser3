using System;
using Windows.UI.Xaml.Controls;
using DvachBrowser3.Markup;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Аргумент события применения разметки.
    /// </summary>
    public sealed class ApplyMarkupEventArgs : EventArgs
    {
        private readonly IMarkupProvider provider;

        private readonly MarkupTag tag;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер.</param>
        /// <param name="tag">Тэг.</param>
        public ApplyMarkupEventArgs(IMarkupProvider provider, MarkupTag tag)
        {
            this.provider = provider;
            this.tag = tag;
        }

        /// <summary>
        /// Применить к текстовому полю.
        /// </summary>
        /// <param name="box">Текстовое поле.</param>
        public void ApplyToTextBox(TextBox box)
        {
            if (box != null)
            {
                box.Text = provider.SetMarkup(box.Text, tag, box.SelectionStart, box.SelectionLength);
            }
        }
    }

    /// <summary>
    /// Обработчик события.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Событие.</param>
    public delegate void ApplyMarkupEventHandler(object sender, ApplyMarkupEventArgs e);
}