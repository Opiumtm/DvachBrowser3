using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа тайлов основной модели.
    /// </summary>
    public interface IMainGroupViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тайлы.
        /// </summary>
        IList<IMainTileViewModel> Tiles { get; }

        /// <summary>
        /// Есть элементы.
        /// </summary>
        bool HasItems { get; }
    }
}