using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации по тегу борды.
    /// </summary>
    public sealed class ThreadTagNavigationKey : BoardLinkNavigaionKey<ThreadTagLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-Tag";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public ThreadTagNavigationKey(ThreadTagLink link)
            : base(link)
        {
        }

        /// <summary>
        /// Имя типа ключа.
        /// </summary>
        public override string TypeName
        {
            get { return TypeNameConst; }
        }

        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <returns>Строка.</returns>
        public override string Serialize()
        {
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.Tag);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static ThreadTagNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new ThreadTagNavigationKey(new ThreadTagLink() { Engine = parts[0], Board = parts[1], Tag = parts[2] });
        }
    }
}