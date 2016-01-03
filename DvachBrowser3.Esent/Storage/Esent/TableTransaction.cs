using System;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Транзакция таблицы.
    /// </summary>
    internal struct TableTransaction : IDisposable
    {
        /// <summary>
        /// Сессия.
        /// </summary>
        public Session Session;

        /// <summary>
        /// Идентификатор базы.
        /// </summary>
        public JET_DBID Dbid;

        /// <summary>
        /// Транзакция.
        /// </summary>
        public Transaction Transaction;

        /// <summary>
        /// Таблица.
        /// </summary>
        public Table Table;

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            try
            {
                try
                {
                    Table.Dispose();
                }
                finally
                {
                    Transaction.Dispose();
                }
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}