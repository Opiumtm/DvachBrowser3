﻿using System;
using DvachBrowser3.Views;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    public sealed class PageNavigationService : ServiceBase, IPageNavigationService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public PageNavigationService(IServiceProvider services) : base(services)
        {
        }

        private void DoNavigate(PageNavigationTargetBase target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (target is BoardInfoNavigationTarget)
            {
                NavigateToBoardInfo((BoardInfoNavigationTarget)target);
                return;
            }
            if (target is BoardPageNavigationTarget)
            {
                NavigateToBoardPage((BoardPageNavigationTarget)target);
                return;
            }
            throw new ArgumentException($"Неизвестный тип цели навигации \"{target.GetType().FullName}\"", nameof(target));
        }

        private void NavigateToBoardInfo(BoardInfoNavigationTarget target)
        {
            var nkey1 = target.Link?.GetNavigationKey();
            if (nkey1 != null)
            {
                var nkey = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Serialize(nkey1);
                if (nkey != null)
                {
                    Shell.HamburgerMenu.NavigationService.Navigate(typeof (BoardInfoPage), nkey);
                }
            }
            else
            {
                throw new ArgumentException("Невозможно получить ключ навигации", nameof(target));
            }
        }

        private void NavigateToBoardPage(BoardPageNavigationTarget target)
        {
            var nkey1 = target.Link?.GetNavigationKey();
            if (nkey1 != null)
            {
                var nkey = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Serialize(nkey1);
                if (nkey != null)
                {
                    Shell.HamburgerMenu.NavigationService.Navigate(typeof(BoardPage), nkey);
                }
            }
            else
            {
                throw new ArgumentException("Невозможно получить ключ навигации", nameof(target));
            }
        }

        /// <summary>
        /// Произвести навигацию.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="reportError">Показать ошибку.</param>
        public void Navigate(PageNavigationTargetBase target, bool reportError = true)
        {
            AppHelpers.DispatchAction(() =>
            {
                DoNavigate(target);
            }, reportError);
        }
    }
}