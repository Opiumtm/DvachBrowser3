using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// ���� ��������� �� ��������.
    /// </summary>
    public sealed class BoardCatalogNavigationKey : BoardLinkNavigaionKey<BoardCatalogLink>
    {
        /// <summary>
        /// ���.
        /// </summary>
        public const string TypeNameConst = "Board-Catalog";

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public BoardCatalogNavigationKey(BoardCatalogLink link)
            : base(link)
        {
        }

        /// <summary>
        /// ��� ���� �����.
        /// </summary>
        public override string TypeName
        {
            get { return TypeNameConst; }
        }

        /// <summary>
        /// ������������� � ������.
        /// </summary>
        /// <returns>������.</returns>
        public override string Serialize()
        {
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, (int)Link.Sort);
        }

        /// <summary>
        /// ���������������.
        /// </summary>
        /// <param name="data">������.</param>
        /// <returns>���� ���������.</returns>
        public static BoardCatalogNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new BoardCatalogNavigationKey(new BoardCatalogLink() { Engine = parts[0], Board = parts[1], Sort = (BoardCatalogSort)int.Parse(parts[2]) });
        }
    }
}