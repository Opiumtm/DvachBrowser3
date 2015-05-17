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
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public override void OnSaveState(Dictionary<string, object> pageState)
        {
            base.OnSaveState(pageState);
            if (TokenSource != null)
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
                TokenSource = null;                
            }
        }

        public override void OnLoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            TokenSource = new CancellationTokenSource();
            base.OnLoadState(navigationParameter, pageState);
        }
    }
}