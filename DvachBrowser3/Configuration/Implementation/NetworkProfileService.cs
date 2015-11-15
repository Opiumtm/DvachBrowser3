using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис профилей сети.
    /// </summary>
    public sealed class NetworkProfileService : ServiceBase, INetworkProfileService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NetworkProfileService(IServiceProvider services) : base(services)
        {
            container = ApplicationData.Current.LocalSettings.CreateContainer("NetworkProfiles", ApplicationDataCreateDisposition.Always);
            roamingContainer = ApplicationData.Current.RoamingSettings.CreateContainer("NetworkProfiles", ApplicationDataCreateDisposition.Always);
        }

        private readonly ApplicationDataContainer container;

        private readonly ApplicationDataContainer roamingContainer;

        /// <summary>
        /// Получить текущий профиль.
        /// </summary>
        public INetworkProfile CurrentProfile { get; private set; }

        /// <summary>
        /// Идентификатор текущего профиля.
        /// </summary>
        public string CurrentProfileId
        {
            get
            {
                try
                {
                    if (!container.Values.ContainsKey("ProfileId"))
                    {
                        return DefaultProfileId;
                    }
                    return (string)container.Values["ProfileId"] ?? DefaultProfileId;
                }
                catch
                {
                    return DefaultProfileId;
                }
            }
            set
            {
                container.Values["ProfileId"] = value ?? DefaultProfileId;
                SetCurrentProfile();
            }
        }

        private void SetCurrentProfile()
        {
            var curId = CurrentProfileId;
            if (BuiltInNetworkProfiles.Profiles.ContainsKey(curId))
            {
                CurrentProfile = BuiltInNetworkProfiles.Profiles[curId];
                return;
            }
            CurrentProfile = new AppDataNetworkProfile(roamingContainer, curId);
        }

        private bool? isMobile;

        private bool IsMobile
        {
            get
            {
                if (isMobile == null)
                {
                    isMobile = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
                }
                return isMobile.Value;
            }
        }

        /// <summary>
        /// Идентификатор профиля по умолчанию.
        /// </summary>
        public string DefaultProfileId => IsMobile ? BuiltInNetworkProfiles.DefaultMobile.Id : BuiltInNetworkProfiles.DefaultDesktop.Id;

        /// <summary>
        /// Получить профиль по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Профиль.</returns>
        public Task<INetworkProfile> GetProfileById(string id)
        {
            if (id == null)
            {
                return Task.FromResult(BuiltInNetworkProfiles.Profiles[DefaultProfileId]);
            }
            if (BuiltInNetworkProfiles.Profiles.ContainsKey(id))
            {
                return Task.FromResult(BuiltInNetworkProfiles.Profiles[id]);
            }
            return Task.FromResult(new AppDataNetworkProfile(roamingContainer, id) as INetworkProfile);
        }

        /// <summary>
        /// Удалить профиль.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Задача.</returns>
        public Task DeleteProfile(string id)
        {
            if (id == null)
            {
                return Task.FromResult(true);
            }
            if (!roamingContainer.Values.ContainsKey(id))
            {
                return Task.FromResult(true);
            }
            roamingContainer.Values.Remove(id);
            if (id == CurrentProfileId)
            {
                CurrentProfileId = DefaultProfileId;
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Зарегистрировать профиль.
        /// </summary>
        /// <param name="profile">Профиль.</param>
        /// <returns>Задача.</returns>
        public Task RegisterProfile(INetworkProfile profile)
        {
            if (profile?.Id == null)
            {
                return Task.FromResult(true);
            }
            var profileData = new AppDataNetworkProfile(roamingContainer, profile.Id);
            profileData.CopyFrom(profile);
            profileData.Save();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Перечислить профили.
        /// </summary>
        /// <returns>Идентификаторы профилей.</returns>
        public Task<IList<string>> ListProfiles()
        {
            return Task.FromResult(DoListProfiles().ToList() as IList<string>);
        }

        private IEnumerable<string> DoListProfiles()
        {
            yield return DefaultProfileId;
            yield return BuiltInNetworkProfiles.Wifi.Id;
            yield return BuiltInNetworkProfiles.Good3G.Id;
            yield return BuiltInNetworkProfiles.Bad3G.Id;
            yield return BuiltInNetworkProfiles.Minimal.Id;
            var keys = roamingContainer.Values.Keys;
            foreach (var k in keys)
            {
                yield return k;
            }
        }

        /// <summary>
        /// Инициализировать.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Initialize()
        {
            SetCurrentProfile();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Получить новый ID для профиля.
        /// </summary>
        /// <returns>Новый ID.</returns>
        public string NewProfileId()
        {
            return $"Custom.{Guid.NewGuid().ToString().ToLowerInvariant()}";
        }
    }
}