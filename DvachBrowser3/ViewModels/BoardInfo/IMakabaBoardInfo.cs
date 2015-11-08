using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Информация о макабе.
    /// </summary>
    public interface IMakabaBoardInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// Иконки.
        /// </summary>
        IList<IBoardInfoIcon> Icons { get; }

        /// <summary>
        /// Есть иконки.
        /// </summary>
        bool HasIcons { get; }

        /// <summary>
        /// Бамп лимит.
        /// </summary>
        int? BumpLimit { get; }

        /// <summary>
        /// Имя по умолчанию.
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        /// Количество страниц.
        /// </summary>
        int Pages { get; }

        /// <summary>
        /// Поддержка сажи.
        /// </summary>
        bool Sage { get; }

        /// <summary>
        /// Поддержка трипкодов.
        /// </summary>
        bool TripCodes { get; }

        /// <summary>
        /// Максимальный размер поста.
        /// </summary>
        int? MaxCommentSize { get; }
    }
}