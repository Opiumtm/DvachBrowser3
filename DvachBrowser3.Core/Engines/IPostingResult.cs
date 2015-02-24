using DvachBrowser3.Links;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат постинга.
    /// </summary>
    public interface IPostingResult
    {
        /// <summary>
        /// Ссылка на тред (при создании нового треда, если есть).
        /// </summary>
        BoardLinkBase RedirectLink { get; }

        /// <summary>
        /// Ссылка на новый пост с номером поста (если есть).
        /// </summary>
        BoardLinkBase PostLink { get; set; }
    }
}