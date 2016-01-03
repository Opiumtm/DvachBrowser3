using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Адаптер таблицы.
    /// </summary>
    internal abstract class EsentTableAdapter : IEsentTableAdapter
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="instanceProvider">Провайдер экземпляров.</param>
        /// <param name="tableName">Имя таблицы.</param>
        protected EsentTableAdapter(IEsentInstanceProvider instanceProvider, string tableName)
        {
            InstanceProvider = instanceProvider;
            TableName = tableName;
        }

        /// <summary>
        /// Провайдер экземпляра.
        /// </summary>
        public IEsentInstanceProvider InstanceProvider { get; }

        /// <summary>
        /// Имя таблицы.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Создать таблицу, если не существует.
        /// </summary>
        /// <returns>Таск.</returns>
        public Task CreateTableIfAbsent()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var session = new Session(Instance))
                {
                    JET_DBID dbid;
                    Api.JetAttachDatabase(session, DatabasePath, AttachDatabaseGrbit.None);
                    Api.JetOpenDatabase(session, DatabasePath, String.Empty, out dbid, OpenDatabaseGrbit.None);
                    using (var transaction = new Transaction(session))
                    {
                        var tables = Api.GetTableNames(session, dbid).ToArray();
                        if (tables.Any(t => TableName.Equals(t, StringComparison.OrdinalIgnoreCase)))
                        {
                            return;
                        }
                        JET_TABLEID tableid;
                        Api.JetCreateTable(session, dbid, TableName, 1, 100, out tableid);
                        DoCreateTable(session, tableid);
                        transaction.Commit(CommitTransactionGrbit.None);
                    }
                }
            });
        }

        /// <summary>
        /// Получить транзакцию.
        /// </summary>
        /// <param name="tableGrbit">Флаги открытия таблицы.</param>
        /// <returns>Транзакция.</returns>
        public TableTransaction GetTransaction(OpenTableGrbit tableGrbit = OpenTableGrbit.None)
        {
            var session = new Session(Instance);
            try
            {
                JET_DBID dbid;
                Api.JetAttachDatabase(session, DatabasePath, AttachDatabaseGrbit.None);
                Api.JetOpenDatabase(session, DatabasePath, String.Empty, out dbid, OpenDatabaseGrbit.None);
                var transaction = new Transaction(session);
                try
                {
                    var table = new Table(session, dbid, TableName, tableGrbit);
                    return new TableTransaction()
                    {
                        Table = table,
                        Session = session,
                        Transaction = transaction,
                        Dbid = dbid
                    };
                }
                catch
                {
                    transaction.Dispose();                    
                    throw;
                }
            }
            catch
            {
                session.Dispose();
                throw;
            }
        }

        protected abstract void DoCreateTable(Session session, JET_TABLEID tableid);

        /// <summary>
        /// Экземпляр.
        /// </summary>
        protected Instance Instance => InstanceProvider?.Instance;

        /// <summary>
        /// Путь к базе данных.
        /// </summary>
        protected string DatabasePath => InstanceProvider?.DatabasePath;

        /// <summary>
        /// Вызвать процедуру в транзакции.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="dataFunc">Функция данных.</param>
        /// <param name="tableGrbit">Флаги открытия таблицы.</param>
        /// <returns>Результат.</returns>
        protected T ExecuteInTransaction<T>(Func<Session, Table, T> dataFunc, OpenTableGrbit tableGrbit = OpenTableGrbit.None)
        {
            T results;
            using (var session = new Session(Instance))
            {
                JET_DBID dbid;
                Api.JetAttachDatabase(session, DatabasePath, AttachDatabaseGrbit.None);
                Api.JetOpenDatabase(session, DatabasePath, String.Empty, out dbid, OpenDatabaseGrbit.None);
                using (var transaction = new Transaction(session))
                {
                    using (var table = new Table(session, dbid, TableName, tableGrbit))
                    {
                        results = dataFunc(session, table);
                    }

                    transaction.Commit(CommitTransactionGrbit.None);
                }
            }

            return results;
        }
    }
}