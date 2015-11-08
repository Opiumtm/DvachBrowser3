using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Возможность постинга.
    /// </summary>
    public interface IBoardInfoPostingCapability : INotifyPropertyChanged
    {
        /// <summary>
        /// Роль.
        /// </summary>
        string Role { get; }

        /// <summary>
        /// Возможность медиа файлов.
        /// </summary>
        bool IsMediaCapability { get; }

        /// <summary>
        /// Максимальное количество медиа файлов.
        /// </summary>
        int? MaxMediaFileCount { get; }

        /// <summary>
        /// Возможность капчи.
        /// </summary>
        bool IsCaptchaCapability { get; }

        /// <summary>
        /// Типы капчи.
        /// </summary>
        IList<IBoadInfoString> CaptchaTypes { get; }

        /// <summary>
        /// Возможность комментария.
        /// </summary>
        bool IsCommentCapability { get; }

        /// <summary>
        /// Максимальный размер комментария.
        /// </summary>
        int? MaxCommentLength { get; }

        /// <summary>
        /// Разметка комментария.
        /// </summary>
        string CommentMarkup { get; }

        /// <summary>
        /// Возможность иконки.
        /// </summary>
        bool IsIconCapability { get; }

        /// <summary>
        /// Иконки.
        /// </summary>
        IList<IBoadInfoString> Icons { get; }
    }
}