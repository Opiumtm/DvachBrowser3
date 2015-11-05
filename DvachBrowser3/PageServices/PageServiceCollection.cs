using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    [ContentProperty(Name = nameof(Services))]
    public sealed class PageServiceCollection : DependencyObject, IPageService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PageServiceCollection()
        {
            services.CollectionChanged += ServicesOnCollectionChanged;
        }

        private void ServicesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IPageService ta in e.NewItems)
                    {
                        ta.Attach(setPage);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    foreach (IPageService ta in e.OldItems)
                    {
                        ta.Attach(null);
                    }
                    if (e.NewItems != null)
                    {
                        foreach (IPageService ta in e.NewItems)
                        {
                            ta.Attach(setPage);
                        }
                    }
                    break;
            }
        }

        private Page setPage;

        /// <summary>
        /// Прикрепиться к странице.
        /// </summary>
        /// <param name="page">Страница.</param>
        public void Attach(Page page)
        {
            setPage = page;
            foreach (var s in Services)
            {
                s.Attach(setPage);
            }
        }

        private readonly ObservableCollection<IPageService> services = new ObservableCollection<IPageService>();

        /// <summary>
        /// Сервисы.
        /// </summary>
        public IList<IPageService> Services => services;
    }
}