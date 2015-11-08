using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class BoardMediaNavigationKey : BoardLinkNavigaionKey<BoardMediaLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-BoardMedia";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardMediaNavigationKey(BoardMediaLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.RelativeUri);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static BoardMediaNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new BoardMediaNavigationKey(new BoardMediaLink() { Engine = parts[0], Board = parts[1], RelativeUri = parts[2] });
        }
    }
}