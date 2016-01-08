﻿using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления треда.
    /// </summary>
    public sealed class ThreadViewModel : UpdateablePostCollectionViewModelBase<IThreadLoaderResult>, IThreadViewModel
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DispatcherTimer updateCheckTimer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ThreadViewModel(BoardLinkBase link)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            Link = link;
            updateCheckTimer = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(1) };
            updateCheckTimer.Tick += UpdateCheckTimerOnTick;
        }

        private void UpdateCheckTimerOnTick(object sender, object o)
        {
            var profile = NetworkProfileHelper.CurrentProfile;
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.FindEngine(Link.Engine);
            if (profile.CheckThreadForUpdatesSec != null && engine != null && ((engine.Capability & EngineCapability.LastModifiedRequest) != 0))
            {
                AppHelpers.DispatchAction(async () =>
                {
                    var operation = new ThreadLoaderOperation(ServiceLocator.Current, new ThreadLoaderArgument()
                    {
                        ThreadLink = Link,
                        UpdateMode = ThreadLoaderUpdateMode.CheckForUpdates
                    });
                    var result = await operation.Complete();
                    IsUpdated = result.IsUpdated;
                });
            }
        }

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public override Task Start()
        {
            DoStart(false);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск.</returns>
        public override Task Resume()
        {
            DoStart(true);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public override async Task Stop()
        {
            await base.Stop();
            updateCheckTimer.Stop();
        }

        private void DoStart(bool resume)
        {
            if (!(resume && HasData))
            {
                DefaultLoadThread(resume);
            }
            SetupTimerTime();
        }

        private void DefaultLoadThread(bool resume)
        {
            var profile = NetworkProfileHelper.CurrentProfile;
            if (profile.UpdateThreadPageOnEntry && !IsBackNavigatedToViewModel && !resume)
            {
                Update.Start2(profile.PreferPartialLoad ? ThreadLoaderUpdateMode.Load : ThreadLoaderUpdateMode.LoadFull);
            }
            else
            {
                Update.Start2(ThreadLoaderUpdateMode.GetFromCache);
            }
        }

        private void SetupTimerTime()
        {
            var profile = NetworkProfileHelper.CurrentProfile;
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.FindEngine(Link.Engine);
            if (profile.CheckThreadForUpdatesSec != null && engine != null && ((engine.Capability & EngineCapability.LastModifiedRequest) != 0))
            {
                updateCheckTimer.Interval = TimeSpan.FromSeconds(profile.CheckThreadForUpdatesSec.Value);
                updateCheckTimer.Start();
            }
        }

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected override IPostViewModel CreatePostViewModel(PostTree post)
        {
            return new PostViewModel(post, this);
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="o">Данные.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<IThreadLoaderResult, EngineProgress> UpdateOperationFactory(object o)
        {
            var updateMode = ThreadLoaderUpdateMode.Load;
            if (o != null)
            {
                if (o.GetType() == typeof(ThreadLoaderUpdateMode))
                {
                    updateMode = (ThreadLoaderUpdateMode)o;
                }
            }
            return new ThreadLoaderOperation(ServiceLocator.Current, new ThreadLoaderArgument()
            {
                UpdateMode = updateMode,
                ThreadLink = Link
            });
        }

        /// <summary>
        /// Данные получены.
        /// </summary>
        /// <param name="result">Результат.</param>
        protected override void UpdateOperationOnResultGot(IThreadLoaderResult result)
        {
            base.UpdateOperationOnResultGot(result);
            IsUpdated = result.IsUpdated;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// Тред обновлён.
        /// </summary>
        private bool isUpdated;

        /// <summary>
        /// Тред обновлён.
        /// </summary>
        public bool IsUpdated
        {
            get { return isUpdated; }
            private set
            {
                isUpdated = value;
                RaisePropertyChanged();
            }
        }

        private bool isBackNavigatedToViewModel;

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        public bool IsBackNavigatedToViewModel
        {
            get { return isBackNavigatedToViewModel; }
            set
            {
                isBackNavigatedToViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Синхронизировать данные.
        /// </summary>
        public void Synchronize()
        {
            if (!HasData)
            {
                DefaultLoadThread(false);
                return;
            }
            var profile = NetworkProfileHelper.CurrentProfile;
            if (profile.CheckForUpdatesInsteadOfLoad && !IsUpdated)
            {
                Update.Start2(ThreadLoaderUpdateMode.CheckForUpdates);
            }
            else
            {
                Update.Start2(ThreadLoaderUpdateMode.Load);
            }
        }

        /// <summary>
        /// Полностью перезагрузить.
        /// </summary>
        public void FullReload()
        {
            Update.Start2(ThreadLoaderUpdateMode.LoadFull);
        }

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        public void CheckForUpdates()
        {
            Update.Start2(ThreadLoaderUpdateMode.CheckForUpdates);
        }
    }
}