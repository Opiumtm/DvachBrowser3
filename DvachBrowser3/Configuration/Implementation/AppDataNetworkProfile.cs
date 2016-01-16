using System;
using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Профиль, хранящийся в контейнере данных.
    /// </summary>
    public sealed class AppDataNetworkProfile : ObservableObject, INetworkProfile
    {
        private readonly ApplicationDataContainer container;

        private ApplicationDataCompositeValue compositeValue;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public AppDataNetworkProfile(ApplicationDataContainer container, string id)
        {
            this.container = container;
            Id = id;
            compositeValue = new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Сохранить.
        /// </summary>
        public void Save()
        {
            container.Values[Id] = compositeValue;
        }

        /// <summary>
        /// Загружено.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Загрузить.
        /// </summary>
        public void Load()
        {
            if (container.Values.ContainsKey(Id))
            {
                compositeValue = container.Values[Id] as ApplicationDataCompositeValue;
            }
            if (compositeValue == null)
            {
                compositeValue = new ApplicationDataCompositeValue();
            }
            IsLoaded = true;
        }

        /// <summary>
        /// Скопировать из источника.
        /// </summary>
        /// <param name="profile">Профиль.</param>
        public void CopyFrom(INetworkProfile profile)
        {
            if (profile == null)
            {
                return;
            }
            this.AutoLoadImageThumbnails = profile.AutoLoadImageThumbnails;
            this.AutoUpdateThreadAfterPost = profile.AutoUpdateThreadAfterPost;
            this.CheckFavoritesForUpdates = profile.CheckFavoritesForUpdates;
            this.CheckForUpdatesInsteadOfLoad = profile.CheckForUpdatesInsteadOfLoad;
            this.CheckThreadForUpdatesSec = profile.CheckThreadForUpdatesSec;
            this.Name = profile.Name;
            this.PreferPartialLoad = profile.PreferPartialLoad;
            this.ShowBanner = profile.ShowBanner;
            this.UpdateBoardPageOnEntry = profile.UpdateBoardPageOnEntry;
            this.UpdateThreadPageOnEntry = profile.UpdateThreadPageOnEntry;
        }

        /// <summary>
        /// Стандартный.
        /// </summary>
        public bool IsStandard => false;

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    return (string)compositeValue["Name"];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                compositeValue["Name"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Обновлять страницу борды при входе.
        /// </summary>
        public bool UpdateBoardPageOnEntry
        {
            get
            {
                try
                {
                    return (bool)compositeValue["UpdateBoardPageOnEntry"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["UpdateBoardPageOnEntry"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Показывать баннер.
        /// </summary>
        public bool ShowBanner
        {
            get
            {
                try
                {
                    return (bool)compositeValue["ShowBanner"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                compositeValue["ShowBanner"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Проверять избранное на изменения.
        /// </summary>
        public bool CheckFavoritesForUpdates
        {
            get
            {
                try
                {
                    return (bool)compositeValue["CheckFavoritesForUpdates"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["CheckFavoritesForUpdates"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Авоматически обновлять после поста.
        /// </summary>
        public bool AutoUpdateThreadAfterPost
        {
            get
            {
                try
                {
                    return (bool)compositeValue["AutoUpdateThreadAfterPost"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["AutoUpdateThreadAfterPost"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Обновлять страницу треда при входе.
        /// </summary>
        public bool UpdateThreadPageOnEntry
        {
            get
            {
                try
                {
                    return (bool)compositeValue["UpdateThreadPageOnEntry"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["UpdateThreadPageOnEntry"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Автоматически загружать предварительный просмотр изображений.
        /// </summary>
        public bool AutoLoadImageThumbnails
        {
            get
            {
                try
                {
                    return (bool)compositeValue["AutoLoadImageThumbnails"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["AutoLoadImageThumbnails"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Проверять тред на апдейты (интервал проверки в секундах, null - не проверять).
        /// </summary>
        public double? CheckThreadForUpdatesSec
        {
            get
            {
                try
                {
                    var d = (double)compositeValue["CheckThreadForUpdatesSec"];
                    return d > 0.1 ? (double?)d : null;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                compositeValue["CheckThreadForUpdatesSec"] = value ?? 0.0;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Проверять на обновления вместо загрузки.
        /// </summary>
        public bool CheckForUpdatesInsteadOfLoad
        {
            get
            {
                try
                {
                    return (bool)compositeValue["CheckForUpdatesInsteadOfLoad"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                compositeValue["CheckForUpdatesInsteadOfLoad"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Предпочитать частичную загрузку.
        /// </summary>
        public bool PreferPartialLoad
        {
            get
            {
                try
                {
                    return (bool)compositeValue["PreferPartialLoad"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["PreferPartialLoad"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Предупреждать о полной перезагрузке.
        /// </summary>
        public bool WarningFullReload
        {
            get
            {
                try
                {
                    return (bool)compositeValue["WarningFullReload"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["WarningFullReload"] = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Предупреждать об открытии каталога.
        /// </summary>
        public bool WarningCatalog
        {
            get
            {
                try
                {
                    return (bool)compositeValue["WarningCatalog"];
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                compositeValue["WarningCatalog"] = value;
                OnPropertyChanged();
            }
        }
    }
}