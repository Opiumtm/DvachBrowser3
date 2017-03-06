using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Коллекция операция моделей представления.
    /// </summary>
    public interface IViewModelOperationsCollection : INotifyPropertyChanged
    {
        /// <summary>
        /// Текущая операция.
        /// </summary>
        IViewModelOperation CurrentOperation { get; }

        /// <summary>
        /// Активные операции.
        /// </summary>
        ObservableCollection<IViewModelOperation> ActiveOperations { get; }

        /// <summary>
        /// Есть какая-то активная работа (основная или фоновая).
        /// </summary>
        bool AnyActiveWork { get; }

        /// <summary>
        /// Найти операцию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Операция (null, если не найдено).</returns>
        IViewModelOperation FindOperationById(int id);

        /// <summary>
        /// Можно отменять.
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        /// Отменить все операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        Task CancelAll();

        /// <summary>
        /// Отменить все операции в случае приостановки модели представления.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        Task CancelAllOnSuspend();
    }
}