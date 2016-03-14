using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис обработки данных треда.
    /// </summary>
    public interface IThreadTreeProcessService
    {
        /// <summary>
        /// Слить тред с его частью.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        /// <param name="part">Часть треда.</param>
        /// <returns></returns>
        void MergeTree(ThreadTree src, ThreadTreePartial part);

        /// <summary>
        /// Установить обратные ссылки.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        void SetBackLinks(PostTreeCollection src);

        /// <summary>
        /// Сортировать дерево по возрастанию номеров постов.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        void SortThreadTree(PostTreeCollection src);

        /// <summary>
        /// Получить короткую информацию о треде.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        /// <returns>Короткая информация.</returns>
        ShortThreadInfo GetShortInfo(ThreadTree src);

        /// <summary>
        /// Получить короткую информацию о треде.
        /// </summary>
        /// <param name="src">ОП-пост.</param>
        /// <returns>Короткая информация.</returns>
        ShortThreadInfo GetShortInfo(PostTree src);
    }
}