using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа тайлов основной модели.
    /// </summary>
    public interface IMainGroupViewModel : IList<IMainTileViewModel>, INotifyPropertyChanged
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Обновить информацию.
        /// </summary>
        Task UpdateInfo();
    }
}