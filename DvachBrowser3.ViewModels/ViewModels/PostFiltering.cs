using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using DvachBrowser3.ViewModels.Filters;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Фильтр постов.
    /// </summary>
    public sealed class PostFiltering : PageViewModelBase, IPostFiltering
    {
        private readonly string storagePrefix;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="storagePrefix">Префикс хранилища.</param>
        public PostFiltering(IPostCollectionViewModel parent, string storagePrefix = null)
        {
            Parent = parent;
            this.storagePrefix = storagePrefix ?? "";
            Filters = new IPostCollectionFilterMode[]
            {
                new NoFilterMode(this), 
                new SearchFilterMode(this), 
                new MediaFilterMode(this), 
                new MyDialogFilterMode(this), 
                new PosterFilterMode(this), 
            };
            filterById = new Dictionary<string, IPostCollectionFilterMode>(StringComparer.OrdinalIgnoreCase);
            foreach (var f in Filters)
            {
                f.Selected += FilterOnSelected;
                filterById[f.Id] = f;
                if (f.IsDefault && currentFilter == null)
                {
                    defaultFilter = f;
                    SetFilter(f.Id);
                }
            }
            ResetFilterCommand = new ViewModelRelayCommand(RefreshFilter);
        }

        private void SetFilter(string id)
        {
            currentFilter = id;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged("CurrentFilter");            
            RefreshFilter();
        }

        private void FilterOnSelected(object sender, EventArgs eventArgs)
        {
            var filter = sender as IPostCollectionFilterMode;
            if (filter != null)
            {
                SetFilter(filter.Id);
            }
        }

        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        public IPostCollectionViewModel Parent { get; private set; }

        /// <summary>
        /// Фильтры.
        /// </summary>
        public IList<IPostCollectionFilterMode> Filters { get; private set; }

        private readonly IPostCollectionFilterMode defaultFilter;

        private readonly Dictionary<string, IPostCollectionFilterMode> filterById;

        private string currentFilter;

        /// <summary>
        /// Текущий фильтр.
        /// </summary>
        public IPostCollectionFilterMode CurrentFilter
        {
            get
            {
                if (currentFilter != null)
                {
                    if (filterById.ContainsKey(currentFilter))
                    {
                        return filterById[currentFilter];
                    }
                }
                return defaultFilter;
            }
            set
            {
                if (value != null)
                {
                    SetFilter(value.Id);                    
                }
            }
        }

        /// <summary>
        /// Сбросить фильтр (установить фильтр по умолчанию).
        /// </summary>
        public void ResetFilter()
        {
            if (defaultFilter != null)
            {
                SetFilter(defaultFilter.Id);
            }
        }

        /// <summary>
        /// Команда сброса фильтра.
        /// </summary>
        public ICommand ResetFilterCommand { get; private set; }

        /// <summary>
        /// Обновить фильтр.
        /// </summary>
        public void RefreshFilter()
        {
            try
            {
                var filter = CurrentFilter;
                var hide = HideFilteredPosts;
                if (Parent.Posts != null && filter != null)
                {
                    var pst = Parent.Posts.Where(p => p != null).SplitLookup(100);
                    foreach (var pg in pst)
                    {
                        var pa = pg.ToArray();
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            try
                            {
                                foreach (var p in pa)
                                {
                                    var a = filter.FilterPost(p);
                                    if (a)
                                    {
                                        p.ViewMode = PostViewMode.Show;
                                    }
                                    else
                                    {
                                        p.ViewMode = hide ? PostViewMode.Hide : PostViewMode.SemiHide;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                DebugHelper.BreakOnError(ex);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private bool hideFilteredPosts;

        /// <summary>
        /// Скрывать отфильтрованные посты.
        /// </summary>
        public bool HideFilteredPosts
        {
            get { return hideFilteredPosts; }
            set
            {
                hideFilteredPosts = value;
                OnPropertyChanged();
                RefreshFilter();
            }
        }

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        /// <returns>Таск.</returns>
        public async override Task OnEnterPage()
        {
            await base.OnEnterPage();
            RefreshFilter();
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public override void OnSaveState(Dictionary<string, object> pageState)
        {
            base.OnSaveState(pageState);
            var currentKey = storagePrefix + "_CurrentFilter";
            pageState[currentKey] = currentFilter;
            foreach (var f in Filters)
            {
                var key = storagePrefix + "_Filter_" + f.Id + "_FilterString";
                pageState[key] = f.Filter;
            }
        }

        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="navigationParameter">Параметр навигации.</param>
        /// <param name="pageState">Состояние страницы (null - нет состояния).</param>
        public override void OnLoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            base.OnLoadState(navigationParameter, pageState);
            if (pageState != null)
            {
                var currentKey = storagePrefix + "_CurrentFilter";
                if (pageState.ContainsKey(currentKey))
                {
                    currentFilter = (string)pageState[currentKey];
                    // ReSharper disable once ExplicitCallerInfoArgument
                    OnPropertyChanged("CurrentFilter");
                }
                foreach (var f in Filters)
                {
                    var key = storagePrefix + "_Filter_" + f.Id + "_FilterString";
                    if (pageState.ContainsKey(key))
                    {
                        f.Filter = (string)pageState[key];
                    }
                }
            }
        }
    }
}