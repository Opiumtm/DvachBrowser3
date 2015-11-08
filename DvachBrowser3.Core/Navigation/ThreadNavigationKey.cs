using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class ThreadNavigationKey : BoardLinkNavigaionKey<ThreadLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-Thread";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public ThreadNavigationKey(ThreadLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.Thread);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static ThreadNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new ThreadNavigationKey(new ThreadLink() { Engine = parts[0], Board = parts[1], Thread = int.Parse(parts[2]) });
        }
    }
}