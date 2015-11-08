using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации по ссылке.
    /// </summary>
    public abstract class BoardLinkNavigaionKey<T> : INavigationKey where T : BoardLinkBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        protected BoardLinkNavigaionKey(T link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public T Link { get; private set; }

        /// <summary>
        /// Имя типа ключа.
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <returns>Строка.</returns>
        public abstract string Serialize();

        /// <summary>
        /// Данные.
        /// </summary>
        public object LinkData => Link;
    }
}