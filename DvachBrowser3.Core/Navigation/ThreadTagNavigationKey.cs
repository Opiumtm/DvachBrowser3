using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// ���� ��������� �� ���� �����.
    /// </summary>
    public sealed class ThreadTagNavigationKey : BoardLinkNavigaionKey<ThreadTagLink>
    {
        /// <summary>
        /// ���.
        /// </summary>
        public const string TypeNameConst = "Link-Tag";

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public ThreadTagNavigationKey(ThreadTagLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.Tag);
        }

        /// <summary>
        /// ���������������.
        /// </summary>
        /// <param name="data">������.</param>
        /// <returns>���� ���������.</returns>
        public static ThreadTagNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new ThreadTagNavigationKey(new ThreadTagLink() { Engine = parts[0], Board = parts[1], Tag = parts[2] });
        }
    }
}