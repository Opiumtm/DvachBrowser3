using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class BoardNavigationKey : BoardLinkNavigaionKey<BoardLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-Board";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardNavigationKey(BoardLink link) : base(link)
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
            return string.Format("{0}|{1}", Link.Engine, Link.Board);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static BoardNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new BoardNavigationKey(new BoardLink() {Engine = parts[0], Board = parts[1]});
        }
    }
}