using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тэги поста.
    /// </summary>
    public interface IPostTags
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Тэги.
        /// </summary>
        IList<string> Tags { get; }

        /// <summary>
        /// Есть тэги.
        /// </summary>
        bool HasTags { get; }
    }
}
