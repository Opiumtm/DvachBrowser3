using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Модель представления, поддерживающая сохранение и восстановление данных.
    /// </summary>
    /// <typeparam name="TVisualState">Визуальное состояние.</typeparam>
    public abstract class PersistentViewModelBase<TVisualState> : ViewModelBase<TVisualState> where TVisualState : VisualState
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected PersistentViewModelBase(CoreDispatcher dispatcher) : base(dispatcher)
        {
        }

        /// <summary>
        /// Получить сервис хранения данных.
        /// </summary>
        /// <returns>Сервис хранения данных.</returns>
        protected abstract Task<IViewModelPersistService> GetPersistService();

        /// <summary>
        /// Получить время, до которого данные считаются валидными.
        /// </summary>
        /// <returns>Время.</returns>
        protected virtual DateTime GetPersistedStateValidPeriod()
        {
            return DateTime.Now.AddMinutes(30);
        }
    }
}