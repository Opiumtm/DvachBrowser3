using System.Collections.Generic;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Фабрика ключей навигации.
    /// </summary>
    public interface INavigationKeyFactory
    {
        /// <summary>
        /// Имена типов ключей.
        /// </summary>
        IEnumerable<string> TypeNames { get; }

        /// <summary>
        /// Десериализовать ключ.
        /// </summary>
        /// <param name="typeName">Тип ключа.</param>
        /// <param name="data">Строка.</param>
        /// <returns>Ключ.</returns>
        INavigationKey Deserialize(string typeName, string data);
    }
}