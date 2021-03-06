﻿using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using DvachBrowser3.Views;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Изменение отсупов в элементе списка.
    /// </summary>
    [TypeConstraint(typeof(FrameworkElement))]
    public sealed class CleanListContentMarginBehavior : DependencyObject, IBehavior, IWeakEventCallback
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public CleanListContentMarginBehavior()
        {
            Shell.IsNarrowViewChanged.AddCallback(this);
            RegisterPropertyChangedCallback(NarrowThicknessProperty, PropertyChangedCallback);
            RegisterPropertyChangedCallback(NormalThicknessProperty, PropertyChangedCallback);
        }

        private void PropertyChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            UpdateContentMargin();
        }

        private FrameworkElement Presenter => AssociatedObject as FrameworkElement;

        private void UpdateContentMargin()
        {
            AppHelpers.ActionOnUiThread(() =>
            {
                if (Presenter != null)
                {
                    if (Shell.Instance.IsNarrowView)
                    {
                        Presenter.Margin = NarrowThickness;
                    }
                    else
                    {
                        Presenter.Margin = NormalThickness;
                    }
                }
                return Task.FromResult(true);
            });
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            UpdateContentMargin();
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            AssociatedObject = null;
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
        public static readonly DependencyProperty AssociatedObjectProperty = DependencyProperty.Register("AssociatedObject", typeof (DependencyObject), typeof (CleanListContentMarginBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Ширина в узком состоянии.
        /// </summary>
        public Thickness NarrowThickness
        {
            get { return (Thickness) GetValue(NarrowThicknessProperty); }
            set { SetValue(NarrowThicknessProperty, value); }
        }

        /// <summary>
        /// Ширина в узком состоянии.
        /// </summary>
        public static readonly DependencyProperty NarrowThicknessProperty = DependencyProperty.Register("NarrowThickness", typeof (Thickness), typeof (CleanListContentMarginBehavior),
            new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Ширина в обычном состоянии.
        /// </summary>
        public Thickness NormalThickness
        {
            get { return (Thickness) GetValue(NormalThicknessProperty); }
            set { SetValue(NormalThicknessProperty, value); }
        }

        /// <summary>
        /// Ширина в обычном состоянии.
        /// </summary>
        public static readonly DependencyProperty NormalThicknessProperty = DependencyProperty.Register("NormalThickness", typeof (Thickness), typeof (CleanListContentMarginBehavior),
            new PropertyMetadata(new Thickness(0)));

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
                UpdateContentMargin();
            }
        }
    }
}