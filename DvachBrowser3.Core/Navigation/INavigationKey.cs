namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации.
    /// </summary>
    public interface INavigationKey
    {
        /// <summary>
        /// Имя типа ключа.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <returns>Строка.</returns>
        string Serialize();

        /// <summary>
        /// Данные.
        /// </summary>
        object LinkData { get; }
    }
}