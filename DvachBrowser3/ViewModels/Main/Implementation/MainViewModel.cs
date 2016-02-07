using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Navigation;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Главная модель представления.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase, IMainViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel()
        {
            CheckForUpdates = new StdEngineOperationWrapper<LinkCollection>(OperationFactory);
            CheckForUpdates.Progress.Finished += (sender, e) =>
            {
                ViewModelEvents.FavoritesListRefreshed.RaiseEvent(this, null);
            };
            AppHelpers.DispatchAction(Initialize);
        }

        private IEngineOperationsWithProgress<LinkCollection, EngineProgress> OperationFactory(object o)
        {
            return new CheckFavoritesOperation(ServiceLocator.Current, new CheckFavoritesParameter() { Mode = CheckFavoriteThreadsMode.Default });
        }

        private async Task Initialize()
        {
            try
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var ids = engines.ListEngines();
                foreach (var id in ids)
                {
                    var engine = engines.GetEngineById(id);
                    NetworkEngines.Add(new MainStringReference(engine.EngineId, engine.ResourceName));
                }
                CurrentEngine = NetworkEngines.FirstOrDefault();
                var profiles = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>();
                var profileList = await profiles.ListProfiles();
                foreach (var id in profileList)
                {
                    var profile = await profiles.GetProfileById(id);
                    NetworkProfiles.Add(new MainStringReference(id, profile.Name));
                }
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(CurrentNetworkProfile));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Start()
        {
            var profiles = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>();
            var profile = profiles.CurrentProfile;
            if (profile.CheckFavoritesForUpdates)
            {
                CheckForUpdates.Start();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Stop()
        {
            CheckForUpdates.Cancel();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Группы.
        /// </summary>
        public IList<IMainGroupViewModel> Groups { get; } = new ObservableCollection<IMainGroupViewModel>();

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        public IOperationViewModel CheckForUpdates { get; }

        /// <summary>
        /// Навигация по адресу.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <param name="addr">Адрес.</param>
        public async void NavigateToAddr(string engine, string addr)
        {
            try
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var nengine = engines.GetEngineById(engine);
                var link = nengine.EngineUriService.TryParsePostLink(addr);
                if (link != null && ((link.LinkKind & BoardLinkKind.Thread) != 0))
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(link));
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Текущий профиль.
        /// </summary>
        public MainStringReference CurrentNetworkProfile
        {
            get
            {
                try
                {
                    var profiles = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>();
                    var current = profiles.CurrentProfileId ?? profiles.DefaultProfileId;
                    return NetworkProfiles.FirstOrDefault(p => StringComparer.OrdinalIgnoreCase.Equals(current, p.Id));
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    var profiles = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>();
                    profiles.CurrentProfileId = value?.Id ?? profiles.DefaultProfileId;
                    RaisePropertyChanged();
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
        }

        private MainStringReference currentEngine;

        /// <summary>
        /// Текущий движок.
        /// </summary>
        public MainStringReference CurrentEngine
        {
            get { return currentEngine; }
            set
            {
                currentEngine = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Установить сетевой профиль.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public async void SetNetworkProfile(string id)
        {
            try
            {
                var profiles = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>();
                profiles.CurrentProfileId = id;
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Движки.
        /// </summary>
        public IList<MainStringReference> NetworkEngines { get; } = new ObservableCollection<MainStringReference>();

        /// <summary>
        /// Профили.
        /// </summary>
        public IList<MainStringReference> NetworkProfiles { get; } = new ObservableCollection<MainStringReference>();
    }
}