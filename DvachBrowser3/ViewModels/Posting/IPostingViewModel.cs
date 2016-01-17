using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления постинга.
    /// </summary>
    public interface IPostingViewModel : INotifyPropertyChanged, IStartableViewModel
    {
        /// <summary>
        /// Ссылка, куда постинг.
        /// </summary>
        BoardLinkBase PostingLink { get; }

        /// <summary>
        /// Операция постинга.
        /// </summary>
        IOperationViewModel Posting { get; }
    }
}