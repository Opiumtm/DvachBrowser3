using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Общая информация о борде.
    /// </summary>
    public interface ISummaryBoardInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// Короткое имя.
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Категория.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Ресурс.
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// Движок.
        /// </summary>
        string Engine { get; }

        /// <summary>
        /// Логотип.
        /// </summary>
        ImageSource EngineLogo { get; }

        /// <summary>
        /// Не для работы.
        /// </summary>
        bool NotSafeForWork { get; }

        /// <summary>
        /// Информация по макабе.
        /// </summary>
        IMakabaBoardInfo MakabaInfo { get; }

        /// <summary>
        /// Постинг.
        /// </summary>
        IPostingBoardInfo Posting { get; }

        /// <summary>
        /// Возможности движка.
        /// </summary>
        IList<IBoadInfoString> EngineCapabilities { get; }
    }
}