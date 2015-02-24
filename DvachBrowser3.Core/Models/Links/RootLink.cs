using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Корневая ссылка.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class RootLink : BoardLinkBase
    {
        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Other;
        }
    }
}