using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Поведение с выпадающим меню.
    /// </summary>
    [TypeConstraint(typeof(UIElement))]
    public sealed class PopupMenuBehavior : DvachBrowserBehaviorBase
    {
        /// <summary>
        /// Действие по присоединению к объекту.
        /// </summary>
        /// <param name="associatedObject">Объект.</param>
        protected override void OnAttach(DependencyObject associatedObject)
        {
            var uiElement = associatedObject as UIElement;
            if (uiElement != null)
            {
                uiElement.Holding += UiElementOnHolding;
                uiElement.PointerPressed += UiElementOnPointerPressed;
                uiElement.RightTapped += UiElementOnRightTapped;
            }
        }

        /// <summary>
        /// Действие по отсоединению от объекта.
        /// </summary>
        /// <param name="associatedObject">Объект.</param>
        protected override void OnDetach(DependencyObject associatedObject)
        {
            var uiElement = associatedObject as UIElement;
            if (uiElement != null)
            {
                uiElement.Holding -= UiElementOnHolding;
                uiElement.PointerPressed -= UiElementOnPointerPressed;
                uiElement.RightTapped -= UiElementOnRightTapped;
            }
        }

        private void UiElementOnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (isPointerPressed)
            {
                ShowMenu(e.GetPosition(uiElement));
                e.Handled = true;
            }
        }

        private void UiElementOnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isPointerPressed = true;
        }

        private bool isPointerPressed;

        private void UiElementOnHolding(object sender, HoldingRoutedEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (e.HoldingState == HoldingState.Completed && uiElement != null)
            {
                var position = e.GetPosition(uiElement);
                ShowMenu(position);
                e.Handled = true;
                isPointerPressed = false;
                uiElement?.CancelDirectManipulations();
            }
        }

        private void ShowMenu(Point offset)
        {
            var uiElement = AssociatedObject as UIElement;
            if (MenuFlyout != null && uiElement != null)
            {
                MenuFlyout.ShowAt(uiElement, offset);
            }
        }

        /// <summary>
        /// Меню.
        /// </summary>
        public MenuFlyout MenuFlyout
        {
            get { return (MenuFlyout) GetValue(MenuFlyoutProperty); }
            set { SetValue(MenuFlyoutProperty, value); }
        }

        /// <summary>
        /// Меню.
        /// </summary>
        public static readonly DependencyProperty MenuFlyoutProperty = DependencyProperty.Register("MenuFlyout", typeof (MenuFlyout), typeof (PopupMenuBehavior), new PropertyMetadata(null));         
    }
}