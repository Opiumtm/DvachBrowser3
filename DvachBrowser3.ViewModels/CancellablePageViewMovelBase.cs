using System.Collections.Generic;
using System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Базовый класс модели представления страницы с возможностью отмены.
    /// </summary>
    public abstract class CancellablePageViewMovelBase : PageViewModelBase, ICancellationTokenSource
    {
        /// <summary>
        /// Источник токенов отмены.
        /// </summary>
        protected CancellationTokenSource TokenSource { get; set; }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return TokenSource != null ? TokenSource.Token : new CancellationToken();
        }

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        public override void OnEnterPage()
        {
            base.OnEnterPage();
            if (TokenSource != null)
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
                TokenSource = null;
            }
        }

        /// <summary>
        /// Выход со страницы.
        /// </summary>
        public override void OnLeavePage()
        {
            TokenSource = new CancellationTokenSource();
            base.OnLeavePage();
        }
    }
}