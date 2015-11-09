namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Предварительный просмотр треда.
    /// </summary>
    public interface IThreadPreviewViewModel : IPostCollectionViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IBoardPageViewModel Parent { get; }

        /// <summary>
        /// Количство изображений.
        /// </summary>
        int ImageCount { get; }

        /// <summary>
        /// Пропущено изображений.
        /// </summary>
        int OmitImages { get; }

        /// <summary>
        /// Количество постов.
        /// </summary>
        int PostCount { get; }

        /// <summary>
        /// Пропущено постов.
        /// </summary>
        int OmitPosts { get; }

        /// <summary>
        /// Не просмотренных постов.
        /// </summary>
        int NotViewedPosts { get; }

        /// <summary>
        /// Тред скрыт.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Скрыть тред.
        /// </summary>
        void Hide();
    }
}