using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using DvachBrowser3.Configuration;
using DvachBrowser3.Logic;
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

        private async Task DoNavigate(PageNavigationTargetBase target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (target is BoardInfoNavigationTarget)
            {
                await NavigateToBoardInfo((BoardInfoNavigationTarget)target);
                return;
            }
            if (target is BoardPageNavigationTarget)
            {
                await NavigateToBoardPage((BoardPageNavigationTarget)target);
                return;
            }
            if (target is BoardCatalogNavigationTarget)
            {
                await NavigateToBoardCatalog((BoardCatalogNavigationTarget)target);
                return;
            }
            if (target is ThreadNavigationTarget)
            {
                await NavigateToThread((ThreadNavigationTarget)target);
                return;
            }
            throw new ArgumentException($"Неизвестный тип цели навигации \"{target.GetType().FullName}\"", nameof(target));
        }

        private async Task NavigateToBoardInfo(BoardInfoNavigationTarget target)
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

        private async Task NavigateToBoardPage(BoardPageNavigationTarget target)
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

        private async Task NavigateToBoardCatalog(BoardCatalogNavigationTarget target)
        {
            var profile = NetworkProfileHelper.CurrentProfile;
            if (profile.WarningCatalog)
            {
                var dialog = new MessageDialog("Открыть каталог треда? Это может привести к большому расходу трафика.", "Внимание!")
                {
                    Commands = { new UICommand("Да", async command =>
                    {
                        await DoNavigateToBoardCatalog(target);
                    }), new UICommand("Нет")}
                };
                dialog.CancelCommandIndex = 1;
                dialog.DefaultCommandIndex = 0;
                await dialog.ShowAsync();
            }
            else
            {
                await DoNavigateToBoardCatalog(target);
            }
        }

        private async Task DoNavigateToBoardCatalog(BoardCatalogNavigationTarget target)
        {
            var nkey1 = target.Link?.GetNavigationKey();
            if (nkey1 != null)
            {
                var nkey = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Serialize(nkey1);
                if (nkey != null)
                {
                    Shell.HamburgerMenu.NavigationService.Navigate(typeof(CatalogPage), nkey);
                }
            }
            else
            {
                throw new ArgumentException("Невозможно получить ключ навигации", nameof(target));
            }
        }

        private async Task NavigateToThread(ThreadNavigationTarget target)
        {
            var nkey1 = target.Link?.GetNavigationKey();
            if (nkey1 != null)
            {
                var nkey = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Serialize(nkey1);
                if (nkey != null)
                {
                    Shell.HamburgerMenu.NavigationService.Navigate(typeof(ThreadPage), nkey);
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
            AppHelpers.DispatchAction(async () =>
            {
                await DoNavigate(target);
            }, reportError);
        }
    }
}