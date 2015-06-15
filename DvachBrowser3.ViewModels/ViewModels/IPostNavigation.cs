using System.Windows.Input;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Навигация по постам.
    /// </summary>
    public interface IPostNavigation : IPageStateAware
    {
        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        void GotoPost(BoardLinkBase link);

        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="number">Номер.</param>
        void GotoPost(int number);

        /// <summary>
        /// Пост попал в отображение.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        void PostGotIntoView(BoardLinkBase id);

        /// <summary>
        /// Отобразить пост.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        void BringIntoView(BoardLinkBase id);

        /// <summary>
        /// Нужно отобразить пост.
        /// </summary>
        event BringIntoViewEventHandler NeedBringIntoView;

        /// <summary>
        /// Верхний отображаемый пост.
        /// </summary>
        BoardLinkBase TopViewPost { get; }

        /// <summary>
        /// Можно ли идти назад.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Перейти назад.
        /// </summary>
        void Back();

        /// <summary>
        /// Команда назад.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Команда сброса навигации.
        /// </summary>
        ICommand ClearNavigationCommand { get; }

        /// <summary>
        /// Очистить стек навигации.
        /// </summary>
        void ClearNavigationStack();

        /// <summary>
        /// Топовый пост навигации.
        /// </summary>
        BoardLinkBase TopNavigation { get; }
    }
}