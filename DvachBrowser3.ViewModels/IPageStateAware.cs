using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Объект, поддерживающий сохранение состояния.
    /// </summary>
    public interface IPageStateAware
    {
        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="navigationParameter">Параметр навигации.</param>
        /// <param name="pageState">Состояние страницы (null - нет состояния).</param>
        void OnLoadState(object navigationParameter, Dictionary<string, object> pageState);

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        void OnSaveState(Dictionary<string, object> pageState);

        /// <summary>
        /// Выход со страницы.
        /// </summary>
        void OnLeavePage();

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        void OnEnterPage();
    }
}