using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Текст поста.
    /// </summary>
    public interface IPostTextViewModel : IPostPartViewModel, ILinkClickCallback
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// Отобразить текст.
        /// </summary>
        /// <param name="logic">Логика рендеринга поста.</param>
        void RenderText(ITextRenderLogic logic);

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Квоты.
        /// </summary>
        IList<IPostQuoteViewModel> Quotes { get; }

        /// <summary>
        /// Есть квоты.
        /// </summary>
        bool HasQuotes { get; }

        /// <summary>
        /// Получить текст без разметки.
        /// </summary>
        /// <returns>Текст.</returns>
        IList<string> GetPlainText();
    }
}