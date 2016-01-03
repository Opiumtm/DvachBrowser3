using System.Threading.Tasks;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Адаптер таблицы.
    /// </summary>
    internal interface IEsentTableAdapter
    {
        /// <summary>
        /// Провайдер экземпляров.
        /// </summary>
        IEsentInstanceProvider InstanceProvider { get; }

        /// <summary>
        /// Имя таблицы.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Создать таблицу, если не существует.
        /// </summary>
        /// <returns>Таск.</returns>
        Task CreateTableIfAbsent();

        /// <summary>
        /// Получить транзакцию.
        /// </summary>
        /// <param name="tableGrbit">Флаги открытия таблицы.</param>
        /// <returns>Транзакция.</returns>
        TableTransaction GetTransaction(OpenTableGrbit tableGrbit = OpenTableGrbit.None);
    }
}