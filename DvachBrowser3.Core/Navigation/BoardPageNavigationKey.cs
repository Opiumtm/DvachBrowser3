using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class BoardPageNavigationKey : BoardLinkNavigaionKey<BoardPageLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-BoardPage";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardPageNavigationKey(BoardPageLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.Page);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static BoardPageNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new BoardPageNavigationKey(new BoardPageLink() { Engine = parts[0], Board = parts[1], Page = int.Parse(parts[2])});
        }
    }
}