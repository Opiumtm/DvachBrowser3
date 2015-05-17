namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления иконки с именем.
    /// </summary>
    public interface IIconWithNameViewModel : IIconViewModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; } 
    }
}