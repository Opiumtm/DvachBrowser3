using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Результат загрузки.
    /// </summary>
    public interface IBoardPageLoaderResult
    {
        /// <summary>
        /// Данные.
        /// </summary>
        BoardPageTree Data { get; }

        /// <summary>
        /// Обновлено (для режимов с проверкой обновления).
        /// </summary>
        bool IsUpdated { get; }
    }
}