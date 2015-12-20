using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации по каталогу.
    /// </summary>
    public sealed class BoardCatalogNavigationKey : BoardLinkNavigaionKey<BoardCatalogLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Board-Catalog";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardCatalogNavigationKey(BoardCatalogLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, (int)Link.Sort);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static BoardCatalogNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new BoardCatalogNavigationKey(new BoardCatalogLink() { Engine = parts[0], Board = parts[1], Sort = (BoardCatalogSort)int.Parse(parts[2]) });
        }
    }
}