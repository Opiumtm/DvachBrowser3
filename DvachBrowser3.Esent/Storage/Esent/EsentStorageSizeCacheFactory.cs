using System;
using System.Threading.Tasks;
using DvachBrowser3.Storage.Files;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Фабрика кэша размеров файлов.
    /// </summary>
    internal sealed class EsentStorageSizeCacheFactory : ServiceBase, IStorageSizeCacheFactory
    {
        private readonly IEsentInstanceProvider instanceProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public EsentStorageSizeCacheFactory(IServiceProvider services) : base(services)
        {
            instanceProvider = new EsentInstanceProvider();
        }

        /// <summary>
        /// Получить кэш.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="readOnly">Только для чтения.</param>
        /// <returns>Кэш.</returns>
        public async Task<IStorageSizeCache> Get(string id, bool readOnly)
        {
            var session = new SizeCacheAdapter(instanceProvider, GetTableName(id));
            return new EsentStorageSizeCache(await session.GetTransaction(readOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None), session, id);
        }

        /// <summary>
        /// Инициализировать глобально.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task InitializeGlobal()
        {
            await instanceProvider.Initialize();
            await instanceProvider.CreateDatabase();
        }

        /// <summary>
        /// Инициализировать кэш.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Результат.</returns>
        public async Task InitializeCache(string id)
        {
            var session = new SizeCacheAdapter(instanceProvider, GetTableName(id));
            await session.CreateTableIfAbsent();
        }

        private string GetTableName(string id)
        {
            return $"sizes_{id}";
        }
    }
}