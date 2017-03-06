using System.ComponentModel;
using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Инициализируемая по требованию модель представления.
    /// </summary>
    /// <typeparam name="T">Тип модели.</typeparam>
    public interface IViewModelPlaceholder<out T> : INotifyPropertyChanged where T : class 
    {
        /// <summary>
        /// Модель.
        /// </summary>
        T Model { get; }

        /// <summary>
        /// Модель действительна.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Создать модель.
        /// </summary>
        /// <returns>Таск.</returns>
        Task CreateModel();

        /// <summary>
        /// Сбросить модель (если на модель назначен биндинг - она будет пересоздана).
        /// </summary>
        /// <returns>Таск.</returns>
        Task Clear();
    }
}