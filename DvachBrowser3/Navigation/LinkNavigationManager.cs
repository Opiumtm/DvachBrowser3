using System;
using System.Threading.Tasks;
using Windows.System;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.TextRender;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Менеджер навигации по ссылкам.
    /// </summary>
    public sealed class LinkNavigationManager : IWeakEventCallback
    {
        private Guid? eventId;

        /// <summary>
        /// Начать обработку.
        /// </summary>
        public void Start()
        {
            if (eventId == null)
            {
                eventId = ViewModelEvents.LinkClick.AddCallback(this);
            }
        }

        /// <summary>
        /// Прекратить обработку.
        /// </summary>
        public void Stop()
        {
            if (eventId != null)
            {
                ViewModelEvents.LinkClick.RemoveCallback(eventId.Value);
            }
        }

        /// <summary>
        /// Вызвать событие перехода по ссылке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="uri">Ссылка.</param>
        /// <param name="linkContext">Контекст ссылки.</param>
        public void RaiseLinkNavigationUri(object sender, string uri, object linkContext)
        {
            ViewModelEvents.LinkClick.RaiseEvent(sender, new LinkClickEventArgs(new LinkTextRenderAttribute(uri)) { LinkContext = linkContext });
        }

        /// <summary>
        /// Вызвать событие перехода по ссылке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="data">Данные.</param>
        /// <param name="linkContext">Контекст ссылки.</param>
        public void RaiseLinkNavigationObject(object sender, object data, object linkContext)
        {
            ViewModelEvents.LinkClick.RaiseEvent(sender, new LinkClickEventArgs(new LinkTextRenderAttribute("[data]", data)) { LinkContext = linkContext });
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        void IWeakEventCallback.ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            var l = e as LinkClickEventArgs;
            if (channel?.Id == ViewModelEvents.LinkClickId && l?.Link != null)
            {
                AppHelpers.ActionOnUiThread(async () =>
                {
                    await HandleNavigationLinkClick(sender, l);
                }, true);
            }
        }

        /// <summary>
        /// Обработать событие по переходу по ссылке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        private async Task HandleNavigationLinkClick(object sender, LinkClickEventArgs e)
        {
            if (e.Link.Uri == null && e.Link.CustomData == null)
            {
                return;
            }
            var currentWindow = Shell.HamburgerMenu?.NavigationService?.FrameFacade?.Content as INavigationLinkCallback;
            if (currentWindow != null)
            {
                if (currentWindow.HandleNavigationLinkClick(sender, e))
                {
                    return;
                }
            }
            var link = e.Link.CustomData as BoardLinkBase;
            string uri = e.Link.Uri;
            if (link != null)
            {
                if ((link.LinkKind & BoardLinkKind.Thread) != 0 || (link.LinkKind & BoardLinkKind.Post) != 0)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(link));
                    return;
                }
                if ((link.LinkKind & BoardLinkKind.BoardPage) != 0)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardInfoNavigationTarget(link));
                    return;
                }
                if ((link.LinkKind & BoardLinkKind.Catalog) != 0 || (link.LinkKind & BoardLinkKind.ThreadTag) != 0)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardCatalogNavigationTarget(link));
                    return;
                }
                if ((link.LinkKind & BoardLinkKind.Media) != 0)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new MediaNavigationTarget(link));
                    return;
                }
                if ((link.LinkKind & BoardLinkKind.Youtube) != 0)
                {
                    if (await NavigateYoutube(link))
                    {
                        return;
                    }
                }
                if (uri == null || uri == "[data]")
                {
                    var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                    var engine = engines.FindEngine(link.Engine);
                    if (engine != null)
                    {
                        uri = engine.EngineUriService.GetBrowserLink(link)?.ToString();
                    }
                }
            }
            if (uri != null && uri != "[data]")
            {
                var uriObj = new Uri(uri, UriKind.Absolute);
                var success = await Windows.System.Launcher.LaunchUriAsync(uriObj);
                if (!success)
                {
                    throw new InvalidOperationException($"Ошибка запуска URL \"{uri}\"");
                }
            }
        }

        private async Task<bool> NavigateYoutube(BoardLinkBase link)
        {
            var youtubeLink = link as YoutubeLink;
            if (youtubeLink != null)
            {
                var uriService = ServiceLocator.Current.GetServiceOrThrow<IYoutubeUriService>();
                var appUri = uriService.GetLaunchApplicationUri(youtubeLink.YoutubeId);
                var status = await Launcher.QueryUriSupportAsync(appUri, LaunchQuerySupportType.Uri);
                if (status == LaunchQuerySupportStatus.Available)
                {
                    await Launcher.LaunchUriAsync(appUri);
                    return true;
                }
            }
            return false;
        }
    }
}