using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Базовый класс конфигурации.
    /// </summary>
    public abstract class AppDataConfigBase : IConfiguration
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected AppDataConfigBase()
        {
            storage = new Lazy<ApplicationDataCompositeValue>(GetValue);
        }

        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task Save()
        {
            SetValue(Storage);
        }

        private readonly Lazy<ApplicationDataCompositeValue> storage;

        public ApplicationDataCompositeValue Storage
        {
            get { return storage.Value; }
        }

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected abstract ApplicationDataCompositeValue GetValue();

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected abstract void SetValue(ApplicationDataCompositeValue value);
    }
}