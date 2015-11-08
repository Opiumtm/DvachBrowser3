using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Строка информации о борде.
    /// </summary>
    public sealed class BoardInfoString : ViewModelBase, IBoadInfoString
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value">Значение.</param>
        public BoardInfoString(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; }
    }
}