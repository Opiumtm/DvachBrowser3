using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления информации о борде.
    /// </summary>
    public interface IBoardInfoViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Информация загружается.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Нет информации.
        /// </summary>
        bool NoInfoAvailable { get; }

        /// <summary>
        /// Информация доступна.
        /// </summary>
        bool InfoAvailable { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }
        
        /// <summary>
        /// В избранном.
        /// </summary>
        bool InFavorites { get; }

        /// <summary>
        /// Информация о борде.
        /// </summary>
        ISummaryBoardInfo BoardInfo { get; }
    }
}