using System.Runtime.Serialization;
using DvachBrowser3.Makaba;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Расширение страницы.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(MakabaBoardPageExtension))]
    public abstract class BoardPageTreeExtension
    {         
    }
}