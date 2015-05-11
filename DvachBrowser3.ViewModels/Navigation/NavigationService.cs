using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис навигации.
    /// </summary>
    public sealed class NavigationService : ServiceBase, INavigationService
    {
        /// <summary>
        /// Типы состояний.
        /// </summary>
        private readonly List<Type> stateTypes = new List<Type>();

        /// <summary>
        /// Типы страниц.
        /// </summary>
        private readonly Dictionary<string, Type> pageTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NavigationService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Зарегистрировать страницу.
        /// </summary>
        /// <param name="typeName">Имя типа страницы.</param>
        /// <param name="pageType">Тип страницы.</param>
        public void RegisterPage(string typeName, Type pageType)
        {
            pageTypes[typeName] = pageType;
        }

        /// <summary>
        /// Зарегистрировать тип состояния.
        /// </summary>
        /// <param name="stateType"></param>
        public void RegisterStateType(Type stateType)
        {
            stateTypes.Add(stateType);
        }

        /// <summary>
        /// Типы состояний.
        /// </summary>
        public IEnumerable<Type> StateTypes
        {
            get { return stateTypes; }
        }

        /// <summary>
        /// Перейти к странице.
        /// </summary>
        /// <param name="typeName">Тип страницы.</param>
        /// <param name="key">Ключ страницы.</param>
        public void Navigate(string typeName, INavigationKey key = null)
        {
            if (pageTypes.ContainsKey(typeName))
            {
                var pageType = pageTypes[typeName];
                var frame = (Frame)Window.Current.Content;
                if (key != null)
                {
                    var str = Services.GetServiceOrThrow<INavigationKeyService>().Serialize(key);
                    frame.Navigate(pageType, str);
                }
                else
                {
                    frame.Navigate(pageType);
                }
            }
            throw new ArgumentException(string.Format("Неизвестный тип страницы {0}", typeName));
        }
    }
}