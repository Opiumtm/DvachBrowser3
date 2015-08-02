namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Без фильтрации.
    /// </summary>
    public sealed class NoFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public NoFilterMode(IPostFiltering parent) : base(parent)
        {
        }

        /// <summary>
        /// Фильтр по умолчанию.
        /// </summary>
        public override bool IsDefault
        {
            get { return true; }
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get { return "no"; }
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public override string Name
        {
            get { return "Все посты"; }
        }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        public override bool FilterPost(IPostViewModel post)
        {
            return true;
        }
    }
}