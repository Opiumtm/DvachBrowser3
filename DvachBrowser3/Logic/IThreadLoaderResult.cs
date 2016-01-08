using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Результат загрузки.
    /// </summary>
    public interface IThreadLoaderResult : IPostTreeListSource
    {
        /// <summary>
        /// Данные.
        /// </summary>
        ThreadTree Data { get; }

        /// <summary>
        /// Обновлено (для режимов с проверкой обновления).
        /// </summary>
        bool IsUpdated { get; }
    }
}