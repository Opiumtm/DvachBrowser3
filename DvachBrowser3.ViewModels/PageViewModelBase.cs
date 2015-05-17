using System.Collections.Generic;

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
    }
}