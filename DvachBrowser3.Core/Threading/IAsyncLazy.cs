using System.Threading.Tasks;

namespace DvachBrowser3
{

    /// <summary>
    /// Асинхронное отложенное получение значения. 
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public interface IAsyncLazy<T>
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        Task<T> GetValue();
    }
}