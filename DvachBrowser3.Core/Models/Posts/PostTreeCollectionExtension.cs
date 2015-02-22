using System.Runtime.Serialization;
using DvachBrowser3.Makaba;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Расширение коллекции постов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(MakabaCollectionExtension))]
    public abstract class PostTreeCollectionExtension
    {
    }
}