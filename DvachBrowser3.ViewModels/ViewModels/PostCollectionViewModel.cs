using System;
using System.Threading;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public sealed class PostCollectionViewModel : ViewModelBase, IPostCollectionViewModel
    {
        private readonly ICancellationTokenSource tokenSource;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="tokenSource">Источник токенов.</param>
        public PostCollectionViewModel(PostTreeCollection data, ICancellationTokenSource tokenSource = null)
        {
            if (data == null) throw new ArgumentNullException("data");
            this.Data = data;
            this.tokenSource = tokenSource;
            if (data.GetType() == typeof (ThreadTree))
            {
                Kind = PostCollectionKind.Thread;
            } else if (data.GetType() == typeof (ArchiveThreadTree))
            {
                Kind = PostCollectionKind.Archive;
            }
            else if (data.GetType() == typeof (ThreadPreviewTree))
            {
                Kind = PostCollectionKind.ThreadPreview;
            }
            else
            {
                Kind = PostCollectionKind.Other;
            }
        }

        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public void GotoPost(BoardLinkBase link)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Тип коллекции.
        /// </summary>
        public PostCollectionKind Kind { get; private set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public PostTreeCollection Data { get; private set; }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return tokenSource != null ? tokenSource.GetToken() : new CancellationToken();
        }
    }
}