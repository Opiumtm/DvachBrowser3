using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// ���� ��������� �����.
    /// </summary>
    public sealed class RootNavigationKey : BoardLinkNavigaionKey<RootLink>
    {
        /// <summary>
        /// ���.
        /// </summary>
        public const string TypeNameConst = "Link-Root";

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public RootNavigationKey(RootLink link)
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
            return Link.Engine;
        }

        /// <summary>
        /// ���������������.
        /// </summary>
        /// <param name="data">������.</param>
        /// <returns>���� ���������.</returns>
        public static RootNavigationKey Deserialize(string data)
        {
            return new RootNavigationKey(new RootLink() { Engine = data });
        }
    }
}