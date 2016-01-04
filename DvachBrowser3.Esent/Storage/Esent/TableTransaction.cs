using System;
using System.Threading;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Транзакция таблицы.
    /// </summary>
    internal struct TableTransaction : IDisposable
    {
        /// <summary>
        /// Экземпляр.
        /// </summary>
        public IEsentInstance Instance;

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
        /// Контекст выполнения.
        /// </summary>
        public IThreadExecutionContext ExecutionContext;

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public async void Dispose()
        {
            try
            {
                var r = this;
                await ExecutionContext.Execute(() =>
                {
                    r.Table.Dispose();
                    r.Transaction.Dispose();
                    r.Session.Dispose();
                    r.Instance.Dispose();
                });
            }
            finally
            {
                ExecutionContext.Dispose();
            }
        }
    }
}