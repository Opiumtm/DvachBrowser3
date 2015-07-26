using System;

namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// ����� �� ������.
    /// </summary>
    public sealed class PosterFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parent">������������ ������.</param>
        public PosterFilterMode(IPostFiltering parent)
            : base(parent)
        {
        }

        /// <summary>
        /// �������������.
        /// </summary>
        public override string Id
        {
            get { return "poster"; }
        }

        /// <summary>
        /// ���.
        /// </summary>
        public override string Name
        {
            get { return "�� ������"; }
        }

        /// <summary>
        /// ���� ������ �������.
        /// </summary>
        public override bool HasFilterString
        {
            get { return true; }
        }

        /// <summary>
        /// ����������� ����.
        /// </summary>
        /// <param name="post">����.</param>
        /// <returns>true, ���� ����� ���������� ����.</returns>
        public override bool FilterPost(IPostViewModel post)
        {            
            if (string.IsNullOrWhiteSpace(Filter))
            {
                return true;
            }
            if (post.Poster == null)
            {
                return false;
            }
            var f = Filter.Trim();
            return (post.Poster.Name ?? "").IndexOf(f, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}