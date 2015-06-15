using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;

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
        /// <returns>Таск.</returns>
        Task OnLeavePage();

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        /// <returns>Таск.</returns>
        Task OnEnterPage();

        /// <summary>
        /// Перед загрузкой состояния.
        /// </summary>
        /// <returns>Таск.</returns>

        Task BeforeLoadState();

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        Task AfterLoadState();

        /// <summary>
        /// Перед сохранением состояния.
        /// </summary>
        /// <returns>Таск.</returns>

        Task BeforeSaveState();

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        Task AfterSaveState();
    }
}