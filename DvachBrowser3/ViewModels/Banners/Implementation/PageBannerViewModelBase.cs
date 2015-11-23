﻿using System;
using System.Threading;
using DvachBrowser3.Configuration;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовый класс баннера.
    /// </summary>
    public abstract class PageBannerViewModelBase : ViewModelBase, IPageBannerViewModel
    {
        /// <summary>
        /// Поведение.
        /// </summary>
        public PageBannerBehavior Behavior
        {
            get
            {
                if (!GetShowBannersFromConfig() || !GetShowBannersFromNetworkBehavior() || BannerLink == null || BannerImageLink == null)
                {
                    return PageBannerBehavior.Disabled;
                }
                return PageBannerBehavior.Enabled;
            }
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public abstract BoardLinkBase BannerLink { get; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public abstract BoardLinkBase BannerImageLink { get; }

        /// <summary>
        /// Тип медиа.
        /// </summary>
        public abstract PageBannerMediaType MediaType { get; }

        /// <summary>
        /// URI загруженного баннера.
        /// </summary>
        public Uri LoadedBannerUri => ServiceLocator.Current.GetServiceOrThrow<IStorageService>().FullSizeMediaFiles.GetStoredImageUri(BannerImageLink);

        /// <summary>
        /// Высота.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        public abstract int Width { get; }

        private int isCancelled = 0;

        /// <summary>
        /// Попробовать загрузить баннер.
        /// </summary>
        public async void TryLoadBanner()
        {
            IsLoaded = false;
            IsError = false;
            if (Behavior == PageBannerBehavior.Disabled)
            {
                return;
            }
            IsLoading = true;
            BannerLoadStarted?.Invoke(this, EventArgs.Empty);
            Interlocked.Exchange(ref isCancelled, 0);
            try
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    // ReSharper disable once AccessToDisposedClosure
                    cancelAction = () =>
                    {
                        tokenSource.Cancel();
                        Interlocked.Exchange(ref isCancelled, 1);
                    };
                    try
                    {
                        var operation = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadMediaFile(BannerImageLink, LoadMediaFileMode.DefaultFullSize);
                        var image = await operation.Complete(tokenSource.Token);
                        IsLoaded = true;
                        BannerLoaded?.Invoke(this, new BannerLoadedEventArgs(image));
                    }
                    finally
                    {
                        cancelAction = null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Interlocked.CompareExchange(ref isCancelled, 0, 0) == 0)
                {
                    Error = ex;
                    IsError = true;
                    BannerLoadError?.Invoke(this, EventArgs.Empty);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool isLoading;

        /// <summary>
        /// Баннер загружается.
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                RaisePropertyChanged();
            }
        }

        private bool isError;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                RaisePropertyChanged();
            }
        }

        private Exception error;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public Exception Error
        {
            get { return error; }
            private set
            {
                error = value;
                RaisePropertyChanged();
            }
        }

        private bool isLoaded;

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        public bool IsLoaded
        {
            get { return isLoaded; }
            set
            {
                isLoaded = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        public event BannerLoadedEventHandler BannerLoaded;

        /// <summary>
        /// Ошибка загрузки баннера.
        /// </summary>
        public event EventHandler BannerLoadError;

        /// <summary>
        /// Начата загрузка.
        /// </summary>
        public event EventHandler BannerLoadStarted;

        private Action cancelAction;

        /// <summary>
        /// Отменить операцию.
        /// </summary>
        public void Cancel()
        {
            cancelAction?.Invoke();
        }

        private bool GetShowBannersFromConfig()
        {
            return ServiceLocator.Current.GetServiceOrThrow<IUiConfigurationService>().UiPages.ShowBanners;
        }

        private bool GetShowBannersFromNetworkBehavior()
        {
            return ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>().CurrentProfile.ShowBanner;
        }
    }
}