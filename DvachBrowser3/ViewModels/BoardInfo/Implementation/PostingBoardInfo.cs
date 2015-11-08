using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Board;
using DvachBrowser3.Posting;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Возможности постинга.
    /// </summary>
    public sealed class PostingBoardInfo : ViewModelBase, IPostingBoardInfo
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="extension">Расширение.</param>
        public PostingBoardInfo(BoardReferencePostingExtension extension)
        {
            if (extension == null) throw new ArgumentNullException(nameof(extension));
            foreach (var cap in (extension.Capabilities ?? new List<PostingCapability>()).Where(c => c != null))
            {
                Capabilities.Add(new BoardInfoPostingCapability(cap));
            }
        }

        /// <summary>
        /// Возможности.
        /// </summary>
        public IList<IBoardInfoPostingCapability> Capabilities { get; } = new List<IBoardInfoPostingCapability>();
    }
}