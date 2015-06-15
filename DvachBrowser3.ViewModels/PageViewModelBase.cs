using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Базовый класс модели представления страницы.
    /// </summary>
    public abstract class PageViewModelBase : ViewModelBase, IPageStateAware
    {
        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="navigationParameter">Параметр навигации.</param>
        /// <param name="pageState">Состояние страницы (null - нет состояния).</param>
        public virtual void OnLoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public virtual void OnSaveState(Dictionary<string, object> pageState)
        {            
        }

        /// <summary>
        /// Выход со страницы.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task OnLeavePage()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task OnEnterPage()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Перед загрузкой состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task BeforeLoadState()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task AfterLoadState()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Перед сохранением состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task BeforeSaveState()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task AfterSaveState()
        {
            return Task.FromResult(true);
        }
    }
}