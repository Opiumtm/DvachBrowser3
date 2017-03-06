using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Модель представления, поддерживающая сохранение и восстановление данных.
    /// </summary>
    /// <typeparam name="TVisualState">Визуальное состояние.</typeparam>
    /// <typeparam name="TPersistState">Сохранённое состояние.</typeparam>
    public abstract class PersistentViewModelBase<TVisualState, TPersistState> : ViewModelBase<TVisualState> where TVisualState : VisualState
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
        /// Получить сохранённое состояние по умолчанию.
        /// </summary>
        /// <returns>Состояние по умолчанию.</returns>
        protected virtual TPersistState GetDefaultPersistedState()
        {
            return default(TPersistState);
        }

        /// <summary>
        /// Получить время, до которого данные считаются валидными.
        /// </summary>
        /// <returns>Время.</returns>
        protected virtual DateTime GetPersistedStateValidPeriod()
        {
            return DateTime.Now.AddMinutes(30);
        }

        /// <summary>
        /// Получить ключ.
        /// </summary>
        /// <returns>Ключ сохранённых данных.</returns>
        protected abstract string GetPersistenceKey();

        /// <summary>
        /// Начать работу модели.
        /// </summary>
        /// <returns>Таск, сигнализирующий о начале работы.</returns>
        public override async Task Start()
        {
            CheckThreadAccess();
            await LoadPersistedState();
            await base.Start();
        }

        /// <summary>
        /// Завершить работу.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о завершении работы.</returns>
        public override async Task Close(TVisualState visualState)
        {
            CheckThreadAccess();
            await SavePersistedState(visualState);
            await base.Close(visualState);
        }

        /// <summary>
        /// Приостановить.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о приостановке.</returns>
        public override async Task Suspend(TVisualState visualState)
        {
            CheckThreadAccess();
            await SavePersistedState(visualState);
            await base.Suspend(visualState);
        }

        /// <summary>
        /// Загрузить сохранённое состояние.
        /// </summary>
        /// <returns>Таск.</returns>
        protected virtual async Task LoadPersistedState()
        {
            try
            {
                var service = await GetPersistService();
                if (service != null)
                {
                    await OnLoad(await service.LoadData(GetPersistenceKey(), GetDefaultPersistedState()));
                }
                else
                {
                    await OnLoad(GetDefaultPersistedState());
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                await OnLoad(GetDefaultPersistedState());
            }
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск.</returns>
        protected virtual async Task SavePersistedState(TVisualState visualState)
        {
            try
            {
                var service = await GetPersistService();
                if (service != null)
                {
                    await service.SaveData(GetPersistenceKey(), await OnSave(visualState), GetPersistedStateValidPeriod());
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="state">Состояние.</param>
        protected virtual Task OnLoad(TPersistState state)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Состояние.</returns>
        protected virtual Task<TPersistState> OnSave(TVisualState visualState)
        {
            return Task.FromResult(GetDefaultPersistedState());
        }
    }
}