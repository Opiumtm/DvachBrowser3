using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Результат постинга.
    /// </summary>
    public interface IPostingResult
    {
        /// <summary>
        /// Тип результата.
        /// </summary>
        PostingResultKind Kind { get; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        CaptchaType CaptchaType { get; }

        /// <summary>
        /// Движок.
        /// </summary>
        string Engine { get; }

        /// <summary>
        /// Ссылка для перенаправления.
        /// </summary>
        BoardLinkBase RedirectLink { get; }
    }
}