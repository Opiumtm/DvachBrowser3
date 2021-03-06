﻿using System;
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
            var setPage = SetPageObj;
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

        private WeakReference<Page> setPageHandle;

        private Page SetPageObj
        {
            get
            {
                if (setPageHandle == null)
                {
                    return null;
                }
                Page obj;
                if (setPageHandle.TryGetTarget(out obj))
                {
                    return obj;
                }
                return null;
            }
        }


        /// <summary>
        /// Прикрепиться к странице.
        /// </summary>
        /// <param name="page">Страница.</param>
        public void Attach(Page page)
        {
            setPageHandle = page != null ? new WeakReference<Page>(page) : null;
            var setPage = SetPageObj;
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