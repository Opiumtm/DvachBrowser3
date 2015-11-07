using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public interface IPostCollectionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Посты.
        /// </summary>
        IList<IPostViewModel> Posts { get; }

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Может обновляться.
        /// </summary>
        bool CanUpdate { get; }

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        IOperationViewModel Update { get; }
    }
}