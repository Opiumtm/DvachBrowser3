using System;
using System.Collections.Generic;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Адаптер размера кэша.
    /// </summary>
    internal interface ISizeCacheAdapter : IEsentTableAdapter
    {
        /// <summary>
        /// Установить размер.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="item">Элемент.</param>
        void SetFileSize(TableTransaction tt, SizeCacheEsentItem item);

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Общий размер.</returns>
        ulong GetTotalSize(TableTransaction tt);

        /// <summary>
        /// Получить общий размер и количество.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Общий размер и количество.</returns>
        Tuple<ulong, int> GetTotalSizeAndCount(TableTransaction tt);

            /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Все элементы.</returns>
        IEnumerable<SizeCacheEsentItem> GetAllItems(TableTransaction tt);

        /// <summary>
        /// Полчить все идентификаторы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Все идентификаторы.</returns>
        IEnumerable<string> GetFileAllIds(TableTransaction tt);

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        void DeleteItem(TableTransaction tt, string fileId);

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Элемент.</returns>
        SizeCacheEsentItem? GetItem(TableTransaction tt, string fileId);

        /// <summary>
        /// Проверить наличие элемента.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Результат проверки.</returns>
        bool IsItemPresent(TableTransaction tt, string fileId);

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        void DeleteAllItems(TableTransaction tt);

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Количество элементов.</returns>
        int GetCount(TableTransaction tt);
    }
}