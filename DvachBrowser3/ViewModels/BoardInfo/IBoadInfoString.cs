using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Строка информации борды.
    /// </summary>
    public interface IBoadInfoString : INotifyPropertyChanged
    {
        /// <summary>
        /// Значение.
        /// </summary>
        string Value { get; }
    }
}