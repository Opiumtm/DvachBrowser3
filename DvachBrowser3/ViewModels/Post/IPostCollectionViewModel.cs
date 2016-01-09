using System;
using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Links;

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
        /// ОП-пост.
        /// </summary>
        IPostViewModel OpPost { get; }

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

        /// <summary>
        /// Есть данные.
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// Посты обновлены.
        /// </summary>
        event EventHandler PostsUpdated;
    }
}