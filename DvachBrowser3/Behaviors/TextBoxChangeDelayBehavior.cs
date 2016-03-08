using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;
using Template10.Common;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Задержка при изменении текста.
    /// </summary>
    [TypeConstraint(typeof(TextBox))]
    [ContentProperty(Name = nameof(Actions))]
    public sealed class TextBoxChangeDelayBehavior : DependencyObject, IBehavior
    {
        private TextBox AssociatedTextBox => AssociatedObject as TextBox;

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            AssociatedTextBox.TextChanged += TextBoxOnTextChanged;
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            AssociatedTextBox.TextChanged -= TextBoxOnTextChanged;
        }

        private bool isWaiting;

        private bool isRefreshed;

        private DateTime stamp;

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != AssociatedObject)
            {
                return;
            }
            AppHelpers.DispatchAction(DelayAction, false, 0);
        }

        private async Task DelayAction()
        {
            stamp = DateTime.Now;
            isRefreshed = true;
            if (isWaiting)
            {
                return;
            }
            isWaiting = true;
            try
            {
                while (isRefreshed)
                {
                    isRefreshed = false;
                    var toWait = (stamp.AddSeconds(DelaySeconds) - DateTime.Now);
                    if (toWait.Ticks > 0)
                    {
                        await Task.Delay(toWait);
                    }
                }
            }
            finally
            {
                isWaiting = false;
            }
            Interaction.ExecuteActions(AssociatedObject, this.Actions, null);
        }

        /// <summary>
        /// Связанный объект.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get { return (DependencyObject) GetValue(AssociatedObjectProperty); }
            set { SetValue(AssociatedObjectProperty, value); }
        }

        /// <summary>
        /// Связанный объект.
        /// </summary>
        public static readonly DependencyProperty AssociatedObjectProperty = DependencyProperty.Register("AssociatedObject", typeof (DependencyObject), typeof (TextBoxChangeDelayBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Задержка в секундах.
        /// </summary>
        public double DelaySeconds
        {
            get
            {
                return (double) GetValue(DelaySecondsProperty);
            }
            set { SetValue(DelaySecondsProperty, value); }
        }

        /// <summary>
        /// Задержка в секундах.
        /// </summary>
        public static readonly DependencyProperty DelaySecondsProperty = DependencyProperty.Register("DelaySeconds", typeof (double), typeof (TextBoxChangeDelayBehavior),
            new PropertyMetadata(1.0));


        /// <summary>
        /// Действия.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                var actions = (ActionCollection) GetValue(ActionsProperty);
                if (actions == null)
                {
                    base.SetValue(ActionsProperty, actions = new ActionCollection());
                }
                return actions;
            }
        }

        /// <summary>
        /// Действия.
        /// </summary>
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof (ActionCollection), typeof (TextBoxChangeDelayBehavior),
            new PropertyMetadata(null));

    }
}