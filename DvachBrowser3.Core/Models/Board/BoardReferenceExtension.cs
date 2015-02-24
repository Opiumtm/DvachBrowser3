using System.Runtime.Serialization;
using DvachBrowser3.Makaba;

namespace DvachBrowser3.Board
{
    /// <summary>
    /// Расширение информации о борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(MakabaBoardReferenceExtension))]
    public abstract class BoardReferenceExtension
    {
    }
}