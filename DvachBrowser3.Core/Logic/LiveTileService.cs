using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using DvachBrowser3.Links;
using DvachBrowser3.Notifications;
using DvachBrowser3.Notifications.TileContent;
using DvachBrowser3.System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис обновления живых плиток.
    /// </summary>
    public sealed class LiveTileService : ServiceBase, ILiveTileService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public LiveTileService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Обновить тайл с информацией об избранных тредах.
        /// </summary>
        /// <param name="favorites">Избранные треды.</param>
        /// <returns>Таск.</returns>
        // ReSharper disable once CSharpWarnings::CS1998
        public async Task UpdateFavoritesTile(LinkCollection favorites)
        {
            var sysInfo = Services.GetServiceOrThrow<ISystemInfo>();
            var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
            var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
            var favorites2 = favorites as ThreadLinkCollection;
            Tuple<BoardLinkBase, FavoriteThreadInfo>[] updated;
            if (favorites2 == null || favorites2.Links == null || favorites2.ThreadInfo == null)
            {
                updated = new Tuple<BoardLinkBase, FavoriteThreadInfo>[0];
            }
            else
            {
                var links = favorites.Links.WithKeys(linkHashService.GetLinkHash).ToArray();
                updated = links
                    .Where(l => favorites2.ThreadInfo.ContainsKey(l.Key))
                    .Select(l => new Tuple<BoardLinkBase, FavoriteThreadInfo>(l.Value, favorites2.ThreadInfo[l.Key] as FavoriteThreadInfo))
                    .Where(l => l.Item2 != null && IsUpdated(l.Item2))
                    .Where(l => l.Item2.CountInfo != null)
                    .OrderByDescending(l => l.Item2.CountInfo.LastChange)
                    .ToArray();
            }
            var lastUpdate = updated.FirstOrDefault();
            var count = updated.Length;
            ITileNotificationContent notification;
            switch (sysInfo.Platform)
            {
                case AppPlatform.WindowsPhone:
                    var n = TileContentFactory.CreateTileWide310x150IconWithBadgeAndText();
                    n.ImageIcon.Src = sysInfo.AppIcon;
                    n.TextHeading.Text = lastUpdate != null ? linkTransformService.GetLinkDisplayString(lastUpdate.Item1) : null;
                    n.TextBody1.Text = lastUpdate != null ? lastUpdate.Item2.Title : null;
                    n.TextBody2.Text = lastUpdate != null ? lastUpdate.Item2.CountInfo.LastChange.ToString() : null;
                    var n2 = TileContentFactory.CreateTileSquare150x150IconWithBadge();
                    n2.ImageIcon.Src = sysInfo.AppIcon;
                    n.Square150x150Content = n2;
                    var n3 = TileContentFactory.CreateTileSquare71x71IconWithBadge();
                    n3.ImageIcon.Src = sysInfo.SmallAppIcon;
                    n2.Square71x71Content = n3;
                    notification = n;
                    break;
                case AppPlatform.Windows:
                    var nw = TileContentFactory.CreateTileSquare310x310TextList01();
                    nw.Branding = TileBranding.Logo;
                    var lastUpdates = updated.Take(3).ToArray();
                    for (int i = 0; i < Math.Min(lastUpdates.Length, 3); i++)
                    {
                        switch (i)
                        {
                            case 0:
                                nw.TextHeading1.Text = linkTransformService.GetBackLinkDisplayString(lastUpdates[i].Item1);
                                nw.TextBodyGroup1Field1.Text = lastUpdates[i].Item2.Title;
                                nw.TextBodyGroup1Field2.Text = lastUpdates[i].Item2.CountInfo.LastChange.ToString();
                                break;
                            case 1:
                                nw.TextHeading2.Text = linkTransformService.GetBackLinkDisplayString(lastUpdates[i].Item1);
                                nw.TextBodyGroup2Field1.Text = lastUpdates[i].Item2.Title;
                                nw.TextBodyGroup2Field2.Text = lastUpdates[i].Item2.CountInfo.LastChange.ToString();
                                break;
                            case 2:
                                nw.TextHeading3.Text = linkTransformService.GetBackLinkDisplayString(lastUpdates[i].Item1);
                                nw.TextBodyGroup3Field1.Text = lastUpdates[i].Item2.Title;
                                nw.TextBodyGroup3Field2.Text = lastUpdates[i].Item2.CountInfo.LastChange.ToString();
                                break;
                        }
                    }
                    var nw2 = TileContentFactory.CreateTileWide310x150Text01();
                    nw2.Branding = TileBranding.Logo;
                    nw2.TextHeading.Text = lastUpdate != null ? linkTransformService.GetLinkDisplayString(lastUpdate.Item1) : null;
                    nw2.TextBody1.Text = lastUpdate != null ? lastUpdate.Item2.Title : null;
                    nw2.TextBody2.Text = lastUpdate != null ? lastUpdate.Item2.CountInfo.LastChange.ToString() : null;
                    nw.Wide310x150Content = nw2;
                    var nw3 = TileContentFactory.CreateTileSquare150x150Text01();
                    nw3.Branding = TileBranding.Logo;
                    nw3.TextHeading.Text = lastUpdate != null ? linkTransformService.GetLinkDisplayString(lastUpdate.Item1) : null;
                    nw3.TextBody1.Text = lastUpdate != null ? lastUpdate.Item2.Title : null;
                    nw3.TextBody2.Text = lastUpdate != null ? lastUpdate.Item2.CountInfo.LastChange.ToString() : null;
                    nw2.Square150x150Content = nw3;
                    var nw4 = TileContentFactory.CreateTileSquare71x71Image();
                    nw4.Branding = TileBranding.Name;
                    nw4.Image.Src = sysInfo.AppIcon;
                    nw3.Square71x71Content = nw4;
                    notification = nw;
                    break;
                default:
                    notification = null;
                    break;
            }
            if (notification != null)
            {
                var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
                tileUpdater.Update(notification.CreateNotification());
            }
            var badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            var badgeContent = new BadgeNumericNotificationContent((uint)count);
            badgeUpdater.Update(badgeContent.CreateNotification());
        }

        private static bool IsUpdated(ShortThreadInfo info)
        {
            var info2 = info as FavoriteThreadInfo;
            if (info2 == null || info2.CountInfo == null)
            {
                return false;
            }
            return info2.CountInfo.PostCount > info2.CountInfo.LoadedPostCount;
        }
    }
}