using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище любых данных.
    /// </summary>
    public interface ICustomDataStore : ICacheFolderInfo
    {
        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveCustomData(string key, Dictionary<string, object> data);

        /// <summary>
        /// Загрузить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Данные.</returns>
        Task<Dictionary<string, object>> LoadCustomData(string key);

        /// <summary>
        /// Удалить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Данне.</returns>
        Task DeleteCustomData(string key);

        /// <summary>
        /// Зарегистрировать тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        void RegisterType(Type type);
    }
}