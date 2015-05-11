using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// ���� ��������� �����.
    /// </summary>
    public sealed class ThreadNavigationKey : BoardLinkNavigaionKey<ThreadLink>
    {
        /// <summary>
        /// ���.
        /// </summary>
        public const string TypeNameConst = "Link-Thread";

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public ThreadNavigationKey(ThreadLink link)
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
            return string.Format("{0}|{1}|{2}", Link.Engine, Link.Board, Link.Thread);
        }

        /// <summary>
        /// ���������������.
        /// </summary>
        /// <param name="data">������.</param>
        /// <returns>���� ���������.</returns>
        public static ThreadNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new ThreadNavigationKey(new ThreadLink() { Engine = parts[0], Board = parts[1], Thread = int.Parse(parts[2]) });
        }
    }
}