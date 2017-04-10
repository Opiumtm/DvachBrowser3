using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Сервис хранения данных моделей представления.
    /// </summary>
    public interface IViewModelPersistService
    {
        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Данные.</param>
        /// <param name="validUntil">Данные актуальны до указанной даты.</param>
        /// <returns>Таск.</returns>
        Task SaveData<T>(string key, T data, DateTime validUntil);

        /// <summary>
        /// Загрузить данные.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Таск.</returns>
        Task<T> LoadData<T>(string key, T defaultValue = default(T));
    }
}