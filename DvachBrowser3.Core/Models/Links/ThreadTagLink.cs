using System.IO;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// ������ �� ��� ������.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadTagLink : BoardLinkBase
    {
        /// <summary>
        /// �����.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// ���.
        /// </summary>
        [DataMember]
        public string Tag { get; set; }
        
        /// <summary>
        /// �������� ��� ������.
        /// </summary>
        /// <returns>��� ������.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.ThreadTag;
        }

        /// <summary>
        /// �����������.
        /// </summary>
        /// <returns>����.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new ThreadTagLink() { Board = Board, Engine = Engine, Tag = Tag };
        }
    }
}