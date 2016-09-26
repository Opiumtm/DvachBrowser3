using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Поведение для установки диагностических данных.
    /// </summary>
    [TypeConstraint(typeof(TextBlock))]
    public sealed class DiagnosticValueBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            Restart();
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            AssociatedObject = null;
            Restart();
        }

        private void Restart()
        {
            var uwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var block = AssociatedBlock;
                if (block != null)
                {
                    block.Text = "...";
                    try
                    {
                        var v = DiagnosticValue;
                        if (v != null)
                        {
                            block.Text = await v.QueryAsync();
                        }
                    }
                    catch
                    {
                        block.Text = "Ошибка";
                    }
                }
            });
        }

        private void ValueChanged(IDiagnosticValue oldValue, IDiagnosticValue newValue)
        {
            if (oldValue != null)
            {
                oldValue.ValueChanged -= DiagnosticValueOnValueChanged;
            }
            if (newValue != null)
            {
                newValue.ValueChanged += DiagnosticValueOnValueChanged;
            }
            Restart();
        }

        private void DiagnosticValueOnValueChanged(object sender, EventArgs eventArgs)
        {
            Restart();
        }

        public TextBlock AssociatedBlock => AssociatedObject as TextBlock;

        public DependencyObject AssociatedObject { get; private set; }

        public static readonly DependencyProperty DiagnosticValueProperty = DependencyProperty.Register(
            "DiagnosticValue", typeof (IDiagnosticValue), typeof (DiagnosticValueBehavior), new PropertyMetadata(default(IDiagnosticValue), DiagnosticValueChangedCallback));

        private static void DiagnosticValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DiagnosticValueBehavior)?.ValueChanged(e.OldValue as IDiagnosticValue, e.NewValue as IDiagnosticValue);
        }

        public IDiagnosticValue DiagnosticValue
        {
            get { return (IDiagnosticValue) GetValue(DiagnosticValueProperty); }
            set { SetValue(DiagnosticValueProperty, value); }
        }
    }
}