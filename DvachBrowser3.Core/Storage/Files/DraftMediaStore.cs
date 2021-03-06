﻿using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище изображений для черновиков.
    /// </summary>
    public class DraftMediaStore : PostingMediaStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="cacheDescription">Описание.</param>
        public DraftMediaStore(IServiceProvider services, string folderName, string cacheDescription) : base(services, folderName, CacheRecycleConfig.MaxValue, cacheDescription)
        {
        }

        protected override Task DoRecycleCache(IStorageSizeCache sizes)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        /// <remarks>Хранилище изображений для черновиков не удаляет файлы автоматически.</remarks>
        public override Task RecycleCache()
        {
            return Task.FromResult(true);
        }
    }
}