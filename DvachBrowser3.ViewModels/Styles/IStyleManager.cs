using System.ComponentModel;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Менеджер стилей.
    /// </summary>
    public interface IStyleManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Текущий стиль.
        /// </summary>
        IBrowserStyle CurrentStyle { get; } 
    }
}