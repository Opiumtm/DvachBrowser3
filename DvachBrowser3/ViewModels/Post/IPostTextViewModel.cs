using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.TextRender;
using Ipatov.MarkupRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Текст поста.
    /// </summary>
    public interface IPostTextViewModel : INotifyPropertyChanged, ILinkClickCallback
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

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
        /// Создать программу.
        /// </summary>
        /// <returns>Программа.</returns>
        ITextRender2RenderProgram CreateProgram();

        /// <summary>
        /// Получить команды для рендеринга.
        /// </summary>
        /// <returns>Команды.</returns>
        IRenderCommandsSource GetRenderCommands();

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

        /// <summary>
        /// Получить текст цитаты.
        /// </summary>
        /// <returns>Текст цитаты.</returns>
        string GetQuoteText();

        /// <summary>
        /// Вызов события по клику на ссылку.
        /// </summary>
        /// <param name="command">Элемент текста.</param>
        void OnLinkClick(IRenderCommand command);
    }
}