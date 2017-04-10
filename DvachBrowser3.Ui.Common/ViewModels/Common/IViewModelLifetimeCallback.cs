using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Обратный вызов по операции изменения состояния модели представления.
    /// </summary>
    public interface IViewModelLifetimeCallback<TVisualState> where TVisualState : VisualState
    {
        /// <summary>
        /// Приоритет (Start и Resume).
        /// </summary>
        int ToActivePriority { get; }

        /// <summary>
        /// Приоритет (Suspend и Close).
        /// </summary>
        int ToInactivePriority { get; }

        /// <summary>
        /// Действие по старту работы. 
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <returns>Таск.</returns>
        Task OnStart(IViewModel<TVisualState> viewModel);

        /// <summary>
        /// Действие по возобновлению работы.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <returns>Таск.</returns>
        Task OnResume(IViewModel<TVisualState> viewModel);

        /// <summary>
        /// Действие по приостановке работы.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск.</returns>
        Task OnSuspend(IViewModel<TVisualState> viewModel, TVisualState visualState);

        /// <summary>
        /// Действие по остановке работы.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск.</returns>
        Task OnClose(IViewModel<TVisualState> viewModel, TVisualState visualState);
    }
}