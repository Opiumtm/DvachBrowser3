﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище тредов.
    /// </summary>
    public class ThreadDataStorage : FolderStorage, IThreadDataStorage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша в байтах.</param>
        /// <param name="normalCacheSize">Нормальный размер кэша в байтах.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public ThreadDataStorage(IServiceProvider services, string folderName, ulong maxCacheSize, ulong normalCacheSize, string cacheDescription)
            : base(services, folderName, maxCacheSize, normalCacheSize, cacheDescription)
        {
            FavoriteThreads = new LinkCollectionStore(services, folderName, "favorite-threads.dat");
            VisitedThreads = new LinkCollectionStore(services, folderName, "visited-threads.dat");
            FavoriteBoards = new LinkCollectionStore(services, folderName, "favorite-boards.dat");
        }

        /// <summary>
        /// Избранные треды.
        /// </summary>
        public ILinkCollectionStore FavoriteThreads { get; private set; }

        /// <summary>
        /// Посещённые треды.
        /// </summary>
        public ILinkCollectionStore VisitedThreads { get; private set; }

        /// <summary>
        /// Избранные борды.
        /// </summary>
        public ILinkCollectionStore FavoriteBoards { get; private set; }

        /// <summary>
        /// Сохранить данные борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveBoardPage(BoardPageTree data)
        {
            var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Board], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.Link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, data, true);
        }

        /// <summary>
        /// Загрузить данные борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Страница борды.</returns>
        public async Task<BoardPageTree> LoadBoardPage(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Board], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                return await ReadXmlObject<BoardPageTree>(file, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить тред.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveThread(ThreadTree data)
        {
            var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Thread], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.Link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, data, true);
        }

        public async Task<ThreadTree> LoadThread(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Thread], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                return await ReadXmlObject<ThreadTree>(file, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить ссылки на борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveBoardReferences(BoardReferences data)
        {
            var folder = await GetBoardReferencesFolder();
            var fileName = string.Format("{0}.dat", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.RootLink));
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await WriteXmlObject(file, folder, data, false);
        }

        /// <summary>
        /// Загрузить ссылки на борды.
        /// </summary>
        /// <param name="rootLink">Корневая ссылка.</param>
        /// <returns>Таск.</returns>
        public async Task<BoardReferences> LoadBoardReferences(BoardLinkBase rootLink)
        {
            try
            {
                if (rootLink == null) return null;
                var folder = await GetBoardReferencesFolder();
                var fileName = string.Format("{0}.dat", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(rootLink));
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                return await ReadXmlObject<BoardReferences>(file, false);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить информацию о количестве постов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SavePostCountInfo(PostCountInfo data)
        {
            var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.PostCount], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.Link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, data, false);
        }

        /// <summary>
        /// Загрузить информацию о количестве постов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        public async Task<PostCountInfo> LoadPostCountInfo(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.PostCount], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                return await ReadXmlObject<PostCountInfo>(file, false);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить информацию о моих постах.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveMyPostsInfo(MyPostsInfo data)
        {
            var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.MyPosts], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.Link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, data, false);
        }

        /// <summary>
        /// Загрузить информацию о моих постах.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о моих постах.</returns>
        public async Task<MyPostsInfo> LoadMyPostsInfo(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.MyPosts], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                return await ReadXmlObject<MyPostsInfo>(file, false);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить штамп изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="stamp">Штамп.</param>
        /// <returns>Таск.</returns>
        public async Task SaveStamp(BoardLinkBase link, string stamp)
        {
            var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Stamp], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, new StringWrapper() { Value = stamp }, false);
        }

        /// <summary>
        /// Загрузить штамп.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Штамп.</returns>
        public async Task<string> LoadStamp(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format(CacheConsts.CacheFileTemplates[CacheConsts.Stamp], Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                var result = await ReadXmlObject<StringWrapper>(file, false);
                return result != null ? result.Value : null;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Получить директорию информации о бордах.
        /// </summary>
        /// <returns>Информация о бордах.</returns>
        protected async Task<StorageFolder> GetBoardReferencesFolder()
        {
            return await (await GetDataFolder()).CreateFolderAsync("boards", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить список файлов, имеющих иммунитет к очистке старых файлов кэша.
        /// </summary>
        /// <returns>Список файлов.</returns>
        protected override async Task<HashSet<string>> GetRecycleImmunity()
        {
            var hashService = Services.GetServiceOrThrow<ILinkHashService>();
            var favorites = await FavoriteThreads.LoadLinkCollectionForReadOnly();
            if (favorites == null || favorites.Links == null)
            {
                return new HashSet<string>();
            }
            var hashes = favorites.Links.Select(hashService.GetLinkHash);
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var h in hashes)
            {
                foreach (var t in CacheConsts.CacheFileTemplates.Values)
                {
                    result.Add(string.Format(t, h));
                }
            }
            return result;
        }

        /// <summary>
        /// Константы кэша.
        /// </summary>
        private static class CacheConsts
        {
            /// <summary>
            /// Борда.
            /// </summary>
            public const char Board = 'b';

            /// <summary>
            /// Тред.
            /// </summary>
            public const char Thread = 't';

            /// <summary>
            /// Количество постов.
            /// </summary>
            public const char PostCount = 'c';

            /// <summary>
            /// Мои посты.
            /// </summary>
            public const char MyPosts = 'm';

            /// <summary>
            /// Мои посты.
            /// </summary>
            public const char Stamp = 's';

            /// <summary>
            /// Форматы имён файлов кэша.
            /// </summary>
            public static readonly Dictionary<char, string> CacheFileTemplates = new Dictionary<char, string>()
            {
                {Board, "{0}.b.cache"},
                {Thread, "{0}.t.cache"},
                {PostCount, "{0}.pc.cache"},
                {MyPosts, "{0}.mp.cache"},
                {Stamp, "{0}.stamp"},
            };
        }
    }
}