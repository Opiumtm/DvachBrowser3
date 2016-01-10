using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Vista;
using Microsoft.Isam.Esent.Interop.Windows10;
using Microsoft.Isam.Esent.Interop.Windows7;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Адаптер таблицы кэша размеров.
    /// </summary>
    internal sealed class SizeCacheAdapter : EsentTableAdapter, ISizeCacheAdapter
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="instanceProvider">Провайдер экземпляров.</param>
        /// <param name="tableName">Имя таблицы.</param>
        public SizeCacheAdapter(IEsentInstanceProvider instanceProvider, string tableName) : base(instanceProvider, tableName)
        {
        }

        private string PrimaryIndexName => $"PK_{TableName}";

        protected override void DoCreateTable(Session session, JET_TABLEID tableid)
        {
            JET_COLUMNID columnid;

            Api.JetAddColumn(session, tableid, "FileId", new JET_COLUMNDEF()
            {
                coltyp = JET_coltyp.Text,
                cp = JET_CP.Unicode,
                cbMax = 255,
                grbit = ColumndefGrbit.ColumnNotNULL
            }, null, 0, out columnid);

            Api.JetAddColumn(session, tableid, "Size", new JET_COLUMNDEF()
            {
                coltyp = Windows10Coltyp.UnsignedLongLong,
                grbit = ColumndefGrbit.ColumnNotNULL
            }, null, 0, out columnid);

            Api.JetAddColumn(session, tableid, "DTicks", new JET_COLUMNDEF()
            {
                coltyp = VistaColtyp.LongLong,
                grbit = ColumndefGrbit.ColumnNotNULL
            }, null, 0, out columnid);

            Api.JetAddColumn(session, tableid, "OTicks", new JET_COLUMNDEF()
            {
                coltyp = VistaColtyp.LongLong,
                grbit = ColumndefGrbit.ColumnNotNULL
            }, null, 0, out columnid);

            var indexDef = "+FileId\0\0";
            Api.JetCreateIndex(session, tableid, PrimaryIndexName, CreateIndexGrbit.IndexPrimary, indexDef, indexDef.Length, 100);
        }

        /// <summary>
        /// Установить размер.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="item">Элемент.</param>
        public void SetFileSize(TableTransaction tt, SizeCacheEsentItem item)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.MakeKey(tt.Session, tt.Table, item.FileId, Encoding.Unicode, MakeKeyGrbit.NewKey);
            if (Api.TrySeek(tt.Session, tt.Table, SeekGrbit.SeekEQ))
            {
                Api.JetDelete(tt.Session, tt.Table);
            }
            using (var updater = new Update(tt.Session, tt.Table, JET_prep.Insert))
            {
                var columnId = Api.GetTableColumnid(tt.Session, tt.Table, "FileId");
                Api.SetColumn(tt.Session, tt.Table, columnId, item.FileId, Encoding.Unicode);

                var columnSize = Api.GetTableColumnid(tt.Session, tt.Table, "Size");
                Api.SetColumn(tt.Session, tt.Table, columnSize, item.Size);

                var columnDTicks = Api.GetTableColumnid(tt.Session, tt.Table, "DTicks");
                Api.SetColumn(tt.Session, tt.Table, columnDTicks, item.DTicks);

                var columnOTicks = Api.GetTableColumnid(tt.Session, tt.Table, "OTicks");
                Api.SetColumn(tt.Session, tt.Table, columnOTicks, item.OTicks);

                updater.Save();
            }
        }

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Общий размер.</returns>
        public ulong GetTotalSize(TableTransaction tt)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);

            ulong result = 0;

            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        var columnSize = Api.GetTableColumnid(tt.Session, tt.Table, "Size");
                        result += Api.RetrieveColumnAsUInt64(tt.Session, tt.Table, columnSize) ?? 0;
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }

            return result;
        }

        /// <summary>
        /// Получить общий размер и количество.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Общий размер и количество.</returns>
        public Tuple<ulong, int> GetTotalSizeAndCount(TableTransaction tt)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);

            ulong result = 0;
            int resultCnt = 0;

            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        var columnSize = Api.GetTableColumnid(tt.Session, tt.Table, "Size");
                        result += Api.RetrieveColumnAsUInt64(tt.Session, tt.Table, columnSize) ?? 0;
                        resultCnt++;
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }

            return new Tuple<ulong, int>(result, resultCnt);
        }

        /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Все элементы.</returns>
        public IEnumerable<SizeCacheEsentItem> GetAllItems(TableTransaction tt)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        var columnId = Api.GetTableColumnid(tt.Session, tt.Table, "FileId");
                        var columnSize = Api.GetTableColumnid(tt.Session, tt.Table, "Size");
                        var columnDTicks = Api.GetTableColumnid(tt.Session, tt.Table, "DTicks");
                        var columnOTicks = Api.GetTableColumnid(tt.Session, tt.Table, "OTicks");

                        var id = Api.RetrieveColumnAsString(tt.Session, tt.Table, columnId, Encoding.Unicode);
                        var size = Api.RetrieveColumnAsUInt64(tt.Session, tt.Table, columnSize) ?? 0;
                        var dticks = Api.RetrieveColumnAsInt64(tt.Session, tt.Table, columnDTicks) ?? 0;
                        var oticks = Api.RetrieveColumnAsInt64(tt.Session, tt.Table, columnOTicks) ?? 0;

                        yield return new SizeCacheEsentItem()
                        {
                            FileId = id,
                            Size = size,
                            DTicks = dticks,
                            OTicks = oticks
                        };
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }
        }

        /// <summary>
        /// Полчить все идентификаторы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Все идентификаторы.</returns>
        public IEnumerable<string> GetFileAllIds(TableTransaction tt)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        var columnId = Api.GetTableColumnid(tt.Session, tt.Table, "FileId");
                        var id = Api.RetrieveColumnAsString(tt.Session, tt.Table, columnId, Encoding.Unicode, RetrieveColumnGrbit.RetrieveFromIndex);
                        yield return id;
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }
        }

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        public void DeleteItem(TableTransaction tt, string fileId)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.MakeKey(tt.Session, tt.Table, fileId, Encoding.Unicode, MakeKeyGrbit.NewKey);
            if (Api.TrySeek(tt.Session, tt.Table, SeekGrbit.SeekEQ))
            {
                Api.JetDelete(tt.Session, tt.Table);
            }
        }

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Элемент.</returns>
        public SizeCacheEsentItem? GetItem(TableTransaction tt, string fileId)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.MakeKey(tt.Session, tt.Table, fileId, Encoding.Unicode, MakeKeyGrbit.NewKey);
            if (Api.TrySeek(tt.Session, tt.Table, SeekGrbit.SeekEQ))
            {
                var columnId = Api.GetTableColumnid(tt.Session, tt.Table, "FileId");
                var columnSize = Api.GetTableColumnid(tt.Session, tt.Table, "Size");
                var columnDTicks = Api.GetTableColumnid(tt.Session, tt.Table, "DTicks");
                var columnOTicks = Api.GetTableColumnid(tt.Session, tt.Table, "OTicks");

                var id = Api.RetrieveColumnAsString(tt.Session, tt.Table, columnId, Encoding.Unicode);
                var size = Api.RetrieveColumnAsUInt64(tt.Session, tt.Table, columnSize) ?? 0;
                var dticks = Api.RetrieveColumnAsInt64(tt.Session, tt.Table, columnDTicks) ?? 0;
                var oticks = Api.RetrieveColumnAsInt64(tt.Session, tt.Table, columnOTicks) ?? 0;

                return new SizeCacheEsentItem()
                {
                    FileId = id,
                    Size = size,
                    DTicks = dticks,
                    OTicks = oticks
                };
            }
            return null;
        }

        /// <summary>
        /// Проверить наличие элемента.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Результат проверки.</returns>
        public bool IsItemPresent(TableTransaction tt, string fileId)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.MakeKey(tt.Session, tt.Table, fileId, Encoding.Unicode, MakeKeyGrbit.NewKey);
            return Api.TrySeek(tt.Session, tt.Table, SeekGrbit.SeekEQ);
        }

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        public void DeleteAllItems(TableTransaction tt)
        {
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        Api.JetDelete(tt.Session, tt.Table);
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }
        }

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <param name="tt">Транзакция таблицы.</param>
        /// <returns>Количество элементов.</returns>
        public int GetCount(TableTransaction tt)
        {
            int count = 0;
            Api.JetSetCurrentIndex(tt.Session, tt.Table, null);
            Api.JetSetTableSequential(tt.Session, tt.Table, Windows7Grbits.Forward);
            try
            {
                if (Api.TryMoveFirst(tt.Session, tt.Table))
                {
                    do
                    {
                        count++;
                    } while (Api.TryMoveNext(tt.Session, tt.Table));
                }
            }
            finally
            {
                Api.JetResetTableSequential(tt.Session, tt.Table, ResetTableSequentialGrbit.None);
            }
            return count;
        }
    }
}