using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Расширение данных поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(PostTreePosterExtension))]
    [KnownType(typeof(PostTreeIconExtension))]
    [KnownType(typeof(PostTreeCountryExtension))]
    public abstract class PostTreeExtension
    {
    }
}