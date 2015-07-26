using System;
using System.Windows.Input;

namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Базовый класс фильтров.
    /// </summary>
    public abstract class PostCollectionFilterModeBase : ViewModelBase, IPostCollectionFilterMode
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель представления.</param>
        protected PostCollectionFilterModeBase(IPostFiltering parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
            SelectCommand = new ViewModelRelayCommand(Select);
        }

        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        public IPostFiltering Parent { get; private set; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Есть строка фильтра.
        /// </summary>
        public virtual bool HasFilterString
        {
            get { return false; }
        }

        /// <summary>
        /// Фильтр.
        /// </summary>
        private string filter;

        /// <summary>
        /// Фильтр.
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбрать фильтр.
        /// </summary>
        public void Select()
        {
            OnSelected();
        }

        /// <summary>
        /// Команда выбора фильтра.
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        /// <summary>
        /// Фильтр выбран.
        /// </summary>
        public event EventHandler Selected;

        /// <summary>
        /// Фильтр выбран.
        /// </summary>
        protected virtual void OnSelected()
        {
            EventHandler handler = Selected;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        public abstract bool FilterPost(IPostViewModel post);

        /// <summary>
        /// Фильтр по умолчанию.
        /// </summary>
        public virtual bool IsDefault
        {
            get { return false; }
        }
    }
}