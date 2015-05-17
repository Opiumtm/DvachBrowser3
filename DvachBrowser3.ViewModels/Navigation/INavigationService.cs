using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис навигации.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Зарегистрировать страницу.
        /// </summary>
        /// <param name="typeName">Имя типа страницы.</param>
        /// <param name="pageType">Тип страницы.</param>
        void RegisterPage(string typeName, Type pageType);

        /// <summary>
        /// Зарегистрировать тип состояния.
        /// </summary>
        /// <param name="stateType"></param>
        void RegisterStateType(Type stateType);

        /// <summary>
        /// Типы состояний.
        /// </summary>
        IEnumerable<Type> StateTypes { get; }

        /// <summary>
        /// Перейти к странице.
        /// </summary>
        /// <param name="typeName">Тип страницы.</param>
        /// <param name="key">Ключ страницы.</param>
        void Navigate(string typeName, INavigationKey key = null);

        /// <summary>
        /// Показать YouTube.
        /// </summary>
        /// <param name="youtubeId">ID Youtube.</param>
        /// <returns>Таск.</returns>
        Task ViewYoutube(string youtubeId);
    }
}