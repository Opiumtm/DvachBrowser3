using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тэги поста.
    /// </summary>
    public sealed class PostTags : IPostTags
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родитель.</param>
        /// <param name="data">Данные.</param>
        public PostTags(IPostViewModel parent, PostTree data)
        {
            this.Parent = parent;
            var tagExt = data?.Extensions?.OfType<PostTreeTagsExtension>()?.FirstOrDefault();
            if (tagExt?.Tags != null)
            {
                foreach (var t in tagExt.Tags)
                {
                    Tags.Add(t);
                }
                HasTags = Tags.Count > 0;
            }
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostViewModel Parent { get; }

        /// <summary>
        /// Тэги.
        /// </summary>
        public IList<string> Tags { get; } = new List<string>();

        /// <summary>
        /// Есть тэги.
        /// </summary>
        public bool HasTags { get; }
    }
}
