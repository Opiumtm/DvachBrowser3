using System.Threading;
using DvachBrowser3.Links;

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
        /// <param name="tokenSource">Источник токенов.</param>
        public PostCollectionViewModel(ICancellationTokenSource tokenSource = null)
        {
            this.tokenSource = tokenSource;
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
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return tokenSource != null ? tokenSource.GetToken() : new CancellationToken();
        }
    }
}