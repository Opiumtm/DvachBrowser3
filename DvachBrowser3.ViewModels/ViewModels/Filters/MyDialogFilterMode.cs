namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// ������ �� ������� ����� ������.
    /// </summary>
    public sealed class MyDialogFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parent">������������ ������.</param>
        public MyDialogFilterMode(IPostFiltering parent)
            : base(parent)
        {
        }

        /// <summary>
        /// �������������.
        /// </summary>
        public override string Id
        {
            get { return "my"; }
        }

        public override string Name
        {
            get { return "������ �� ��� �����"; }
        }

        /// <summary>
        /// ����������� ����.
        /// </summary>
        /// <param name="post">����.</param>
        /// <returns>true, ���� ����� ���������� ����.</returns>
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