using System.Runtime.Serialization;
using DvachBrowser3.Makaba;

namespace DvachBrowser3.Board
{
    /// <summary>
    /// Расширение информации о борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(MakabaBoardReferenceExtension))]
    [KnownType(typeof(BoardReferencePostingExtension))]
    public abstract class BoardReferenceExtension
    {
    }
}