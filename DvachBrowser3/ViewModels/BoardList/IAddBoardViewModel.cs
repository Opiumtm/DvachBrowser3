using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления для добавления борды.
    /// </summary>
    public interface IAddBoardViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Движки.
        /// </summary>
        IList<INetworkEngine> Engines { get; }

        /// <summary>
        /// Выбранный движок.
        /// </summary>
        INetworkEngine SelectedEngine { get; set; }

        /// <summary>
        /// Короткое имя.
        /// </summary>
        string ShortName { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Обновить значения.
        /// </summary>
        /// <param name="selectedEngine">Выбранный движок.</param>
        /// <param name="shortName">Короткое имя.</param>
        /// <param name="description">Описание.</param>
        void ResetValues(object selectedEngine, string shortName, string description);

        /// <summary>
        /// Получить модель борды.
        /// </summary>
        /// <returns>Модель борды.</returns>
        IBoardListBoardViewModel GetBoardModel();
    }
}