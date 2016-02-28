using System;
using System.Runtime.CompilerServices;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Класс-помощник логики отображения всплывающих меню.
    /// </summary>
    public sealed class PopupLogicHelper : IDisposable
    {
        /// <summary>
        /// Меню.
        /// </summary>
        public MenuFlyout MenuFlyout { get; set; }

        private readonly UIElement uiElement;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="uiElement">Элемент UI.</param>
        public PopupLogicHelper(UIElement uiElement)
        {
            if (uiElement == null) throw new ArgumentNullException(nameof(uiElement));
            this.uiElement = uiElement;
            uiElement.Holding += UiElementOnHolding;
            uiElement.PointerPressed += UiElementOnPointerPressed;
            uiElement.RightTapped += UiElementOnRightTapped;
        }

        private void UiElementOnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
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
            if (e.HoldingState == HoldingState.Completed && uiElement != null)
            {
                var position = e.GetPosition(uiElement);
                ShowMenu(position);
                e.Handled = true;
                isPointerPressed = false;
                uiElement.CancelDirectManipulations();
            }
        }

        private void ShowMenu(Point offset)
        {
            MenuFlyout?.ShowAt(uiElement, offset);
        }

        /// <summary>
        /// Остоединение событий.
        /// </summary>
        public void Dispose()
        {
            uiElement.Holding -= UiElementOnHolding;
            uiElement.PointerPressed -= UiElementOnPointerPressed;
            uiElement.RightTapped -= UiElementOnRightTapped;
        }

        public static PopupLogicHelper Attach(UIElement uiElement, MenuFlyout mf)
        {
            return new PopupLogicHelper(uiElement) { MenuFlyout = mf };
        }
    }
}