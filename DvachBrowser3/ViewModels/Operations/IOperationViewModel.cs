using System.ComponentModel;
using System.Threading;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Операция.
    /// </summary>
    public interface IOperationViewModel : INotifyPropertyChanged, ICancellableViewModel
    {
        /// <summary>
        /// Можно начинать.
        /// </summary>
        bool CanStart { get; }

        /// <summary>
        /// Начать.
        /// </summary>
        void Start();

        /// <summary>
        /// Начать.
        /// </summary>
        /// <param name="arg">Аргумент.</param>
        void Start2(object arg);

        /// <summary>
        /// Прогресс.
        /// </summary>
        IOperationProgressViewModel Progress { get; }

        /// <summary>
        /// Запретить.
        /// </summary>
        void Disable();

        /// <summary>
        /// Разрешить.
        /// </summary>
        void Enable();

        /// <summary>
        /// Нужна диспетчеризация.
        /// </summary>
        bool NeedDispatch { get; set; }
    }
}