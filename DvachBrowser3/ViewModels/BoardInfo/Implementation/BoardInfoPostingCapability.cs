using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Posting;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Возможность постинга.
    /// </summary>
    public sealed class BoardInfoPostingCapability : ViewModelBase, IBoardInfoPostingCapability
    {
        private readonly PostingCapability capability;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="capability">Возможность.</param>
        public BoardInfoPostingCapability(PostingCapability capability)
        {
            if (capability == null) throw new ArgumentNullException(nameof(capability));
            this.capability = capability;
            var media = capability as PostingMediaFileCapability;
            if (media != null)
            {
                IsMediaCapability = true;
                MaxMediaFileCount = media.MaxFileCount;
            }
            var captcha = capability as PostingCaptchaCapability;
            if (captcha != null)
            {
                IsCaptchaCapability = true;
                if ((captcha.CaptchaTypes & Posting.CaptchaTypes.Yandex) != 0)
                {
                    CaptchaTypes.Add(new BoardInfoString("Yandex"));
                }
                if ((captcha.CaptchaTypes & Posting.CaptchaTypes.Recaptcha) != 0)
                {
                    CaptchaTypes.Add(new BoardInfoString("Google ReCaptcha"));
                }
                if ((captcha.CaptchaTypes & Posting.CaptchaTypes.GoogleRecaptcha2СhV1) != 0)
                {
                    CaptchaTypes.Add(new BoardInfoString("Google ReCaptcha V1"));
                }
                if ((captcha.CaptchaTypes & Posting.CaptchaTypes.GoogleRecaptcha2СhV2) != 0)
                {
                    CaptchaTypes.Add(new BoardInfoString("Google ReCaptcha V2"));
                }
                if ((captcha.CaptchaTypes & Posting.CaptchaTypes.DvachCaptcha) != 0)
                {
                    CaptchaTypes.Add(new BoardInfoString("Двач.Капча"));
                }
            }
            var comment = capability as PostingCommentCapability;
            if (comment != null)
            {
                IsCommentCapability = true;
                switch (comment.MarkupType)
                {
                    case PostingMarkupType.Makaba:
                        CommentMarkup = "Makaba";
                        break;
                    default:
                        CommentMarkup = "Другой тип разметки";
                        break;
                }
                MaxCommentLength = comment.MaxLength;
            }
            var icon = capability as PostingIconCapability;
            if (icon != null)
            {
                IsIconCapability = true;
                foreach (var i in (icon.Icons ?? new List<PostingCapabilityIcon>()).Where(i => i != null).OrderBy(i => i.Number))
                {
                    Icons.Add(new BoardInfoString(i.Name));
                }
            }
        }

        private string GetRoleName(PostingFieldSemanticRole role)
        {
            switch (role)
            {
                case PostingFieldSemanticRole.Captcha:
                    return "Ввод капчи";
                case PostingFieldSemanticRole.Comment:
                    return "Комментарий";
                case PostingFieldSemanticRole.Email:
                    return "Адрес электронной почты";
                case PostingFieldSemanticRole.Icon:
                    return "Иконка";
                case PostingFieldSemanticRole.MediaFile:
                    return "Медиа файлы";
                case PostingFieldSemanticRole.OpFlag:
                    return "Флаг ОП";
                case PostingFieldSemanticRole.PosterName:
                    return "Имя постера";
                case PostingFieldSemanticRole.PosterTrip:
                    return "Трипкод";
                case PostingFieldSemanticRole.SageFlag:
                    return "SAGE";
                case PostingFieldSemanticRole.Title:
                    return "Заголовок поста";
                case PostingFieldSemanticRole.WatermarkFlag:
                    return "Ватермарка";
                case PostingFieldSemanticRole.ThreadTag:
                    return "Тэг треда";
                default:
                    return "Другая роль";
            }
        }

        /// <summary>
        /// Роль.
        /// </summary>
        public string Role => GetRoleName(capability.Role);

        /// <summary>
        /// Возможность медиа файлов.
        /// </summary>
        public bool IsMediaCapability { get; }

        /// <summary>
        /// Максимальное количество медиа файлов.
        /// </summary>
        public int? MaxMediaFileCount { get; }

        /// <summary>
        /// Возможность капчи.
        /// </summary>
        public bool IsCaptchaCapability { get; }

        /// <summary>
        /// Типы капчи.
        /// </summary>
        public IList<IBoadInfoString> CaptchaTypes { get; } = new List<IBoadInfoString>();

        /// <summary>
        /// Возможность комментария.
        /// </summary>
        public bool IsCommentCapability { get; }

        /// <summary>
        /// Максимальный размер комментария.
        /// </summary>
        public int? MaxCommentLength { get; }

        /// <summary>
        /// Разметка комментария.
        /// </summary>
        public string CommentMarkup { get; }

        /// <summary>
        /// Возможность иконки.
        /// </summary>
        public bool IsIconCapability { get; }

        /// <summary>
        /// Иконки.
        /// </summary>
        public IList<IBoadInfoString> Icons { get; } = new List<IBoadInfoString>();
    }
}