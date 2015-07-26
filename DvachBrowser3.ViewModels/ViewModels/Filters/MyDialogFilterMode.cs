namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Фильтр по наличию медиа файлов.
    /// </summary>
    public sealed class MyDialogFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public MyDialogFilterMode(IPostFiltering parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get { return "my"; }
        }

        public override string Name
        {
            get { return "Ответы на мои посты"; }
        }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        public override bool FilterPost(IPostViewModel post)
        {
            if (post == null)
            {
                return false;
            }
            return post.Parent.IsMyPostDialog(post.Data.Link);
        }
    }
}