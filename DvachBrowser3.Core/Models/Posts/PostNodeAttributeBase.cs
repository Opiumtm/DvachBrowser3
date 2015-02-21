using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Базовый класс атрибута поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(PostNodeAttribute))]
    [KnownType(typeof(PostNodeLinkAttribute))]
    public class PostNodeAttributeBase
    {
    }
}