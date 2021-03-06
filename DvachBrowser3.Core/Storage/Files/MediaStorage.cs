﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище медиа файлов.
    /// </summary>
    public class MediaStorage : FolderStorage, IMediaStorage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="recycleConfig">Конфигурация очистки.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public MediaStorage(IServiceProvider services, string folderName, CacheRecycleConfig recycleConfig, string cacheDescription)
            : base(services, folderName, recycleConfig, cacheDescription)
        {
        }

        /// <summary>
        /// Записать файл в медиа-хранилище (временный файл будет удалён).
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="tempFile">Временный файл.</param>
        /// <returns>Файл в хранилище.</returns>
        public async Task<StorageFile> MoveToMediaStorage(BoardLinkBase link, StorageFile tempFile)
        {
            var cacheDir = await GetCacheFolder();
            var newFile = await cacheDir.CreateFileAsync(GetMediaFileName(link), CreationCollisionOption.OpenIfExists);
            await WriteCacheMediaFile(newFile, tempFile, true);
            return newFile;
        }

        /// <summary>
        /// Получить файл из медиа-хранилища.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл (null, если не найден).</returns>
        public async Task<StorageFile> GetFromMediaStorage(BoardLinkBase link)
        {
            var fileName = GetMediaFileName(link);
            var hasFile = await FindFileInCache(fileName);
            if (!hasFile)
            {
                return null;
            }
            var cacheDir = await GetCacheFolder();
            try
            {
                return await cacheDir.GetFileAsync(fileName);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Получить URI ссылку на хранимый медиа файл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>URI ссылка на медиа файл.</returns>
        public Uri GetStoredImageUri(BoardLinkBase link)
        {
            var fileName = GetMediaFileName(link);
            return new Uri($"ms-appdata:///local/data/{FolderName}/cache/{fileName}", UriKind.Absolute);
        }

        /// <summary>
        /// Получить имя медиа файла.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public string GetMediaFileName(BoardLinkBase link)
        {
            return string.Format("{0}.cache", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
        }
    }
}