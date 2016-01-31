using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public interface IPostingIconViewModel : IPostingFieldViewModel<IPostingIconElement>
    {
        /// <summary>
        /// Иконки.
        /// </summary>
        IList<IPostingIconElement> Icons { get; }

        /// <summary>
        /// Иконка по умолчанию.
        /// </summary>
        IPostingIconElement DefaultIcon { get; }

        /// <summary>
        /// Как базовый интерфейс.
        /// </summary>
        IPostingFieldViewModel<IPostingIconElement> AsBaseIntf { get; }
    }
}