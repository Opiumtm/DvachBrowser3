using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public interface IPostingIconViewModel : IPostingFieldViewModel<Empty>
    {
        /// <summary>
        /// Иконки.
        /// </summary>
        IList<IPostingIconElement> Icons { get; }

        /// <summary>
        /// Выбранная иконка.
        /// </summary>
        IPostingIconElement SelectedIcon { get; set; }
    }
}