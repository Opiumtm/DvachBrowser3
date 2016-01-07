using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using DvachBrowser3.Views;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Изменение отсупов в элементе списка.
    /// </summary>
    [TypeConstraint(typeof(ListViewItemPresenter))]
    public sealed class CleanListContentMarginBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public CleanListContentMarginBehavior()
        {
            Shell.Instance.RegisterPropertyChangedCallback(Shell.IsNarrowViewProperty, PropertyChangedCallback);
            RegisterPropertyChangedCallback(NarrowThicknessProperty, PropertyChangedCallback);
            RegisterPropertyChangedCallback(NormalThicknessProperty, PropertyChangedCallback);
        }

        private void PropertyChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            UpdateContentMargin();
        }

        private ListViewItemPresenter Presenter => AssociatedObject as ListViewItemPresenter;

        private void UpdateContentMargin()
        {
            if (Presenter != null)
            {
                if (Shell.Instance.IsNarrowView)
                {
                    Presenter.ContentMargin = NarrowThickness;
                }
                else
                {
                    Presenter.ContentMargin = NormalThickness;
                }
            }
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
    }
}