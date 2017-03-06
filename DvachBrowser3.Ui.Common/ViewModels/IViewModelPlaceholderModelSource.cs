using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Источник модели.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="T">Тип модели.</typeparam>
    public interface IViewModelPlaceholderModelSource<TKey, T> where T : class 
    {
        /// <summary>
        /// Создать модель.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="key">Ключ.</param>
        /// <returns>Модель.</returns>
        Task<T> CreateModel(CoreDispatcher dispatcher, TKey key);

        /// <summary>
        /// Предлагается очистка модели.
        /// </summary>
        event EventHandler<TKey> ClearSuggested;
    }
}