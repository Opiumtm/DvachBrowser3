using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Главная модель представления.
    /// </summary>
    public interface IMainViewModel : INotifyPropertyChanged, IStartableViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        IList<IMainGroupViewModel> Groups { get; }
        
        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        IOperationViewModel CheckForUpdates { get; }

        /// <summary>
        /// Навигация по адресу.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <param name="addr">Адрес.</param>
        void NavigateToAddr(string engine, string addr);

        /// <summary>
        /// Текущий профиль.
        /// </summary>
        MainStringReference CurrentNetworkProfile { get; set; }

        /// <summary>
        /// Текущий движок.
        /// </summary>
        MainStringReference CurrentEngine { get; set; }

        /// <summary>
        /// Движки.
        /// </summary>
        IList<MainStringReference> NetworkEngines { get; }

        /// <summary>
        /// Профили.
        /// </summary>
        IList<MainStringReference> NetworkProfiles { get; }
    }
}