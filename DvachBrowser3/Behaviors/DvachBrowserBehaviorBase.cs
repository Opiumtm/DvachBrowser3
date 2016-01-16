using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Behaviors
{
    /// <summary>
    /// Базовый класс поведения.
    /// </summary>
    public abstract class DvachBrowserBehaviorBase : DependencyObject, IBehavior
    {
        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            OnAttach(associatedObject);
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            OnDetach(AssociatedObject);
            AssociatedObject = null;
        }

        /// <summary>
        /// Действие по присоединению к объекту.
        /// </summary>
        /// <param name="associatedObject">Объект.</param>
        protected virtual void OnAttach(DependencyObject associatedObject)
        {
        }

        /// <summary>
        /// Действие по отсоединению от объекта.
        /// </summary>
        /// <param name="associatedObject">Объект.</param>
        protected virtual void OnDetach(DependencyObject associatedObject)
        {            
        }

        /// <summary>
        /// Связанный объект.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get { return (DependencyObject)GetValue(AssociatedObjectProperty); }
            set { SetValue(AssociatedObjectProperty, value); }
        }

        /// <summary>
        /// Связанный объект.
        /// </summary>
        public static readonly DependencyProperty AssociatedObjectProperty = DependencyProperty.Register("AssociatedObject", typeof(DependencyObject), typeof(DvachBrowserBehaviorBase),
            new PropertyMetadata(null));
    }
}