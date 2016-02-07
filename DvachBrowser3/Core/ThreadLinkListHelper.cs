using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;
using DvachBrowser3.Storage;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник по изменению списка ссылок.
    /// </summary>
    public static class ThreadLinkListHelper
    {
        /// <summary>
        /// Удалить из посещённых тредов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="reportError">Показать ошибку на экране.</param>
        /// <returns>Треды.</returns>
        public static async Task RemoveFromVisitedThreads(this BoardLinkBase link, bool reportError = true)
        {
            if (link != null)
            {
                try
                {
                    var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                    var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                    var favs = await storage.ThreadData.VisitedThreads.LoadLinkCollection() as ThreadLinkCollection;
                    if (favs?.Links != null)
                    {
                        var hash = linkHash.GetLinkHash(link);
                        if (favs.ThreadInfo?.ContainsKey(hash) ?? false)
                        {
                            favs.ThreadInfo.Remove(hash);
                        }
                        favs.Links = favs.Links.Where(l => linkHash.GetLinkHash(l) != hash).ToList();
                        await storage.ThreadData.VisitedThreads.SaveLinkCollection(favs);
                    }
                }
                catch (Exception ex)
                {
                    if (reportError)
                    {
                        await AppHelpers.ShowError(ex);
                    }
                    else
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
                finally
                {
                    ViewModelEvents.VisitedListRefreshed.RaiseEvent(link, null);
                }
            }
        }

        /// <summary>
        /// Удалить из избранных тредов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="reportError">Показать ошибку на экране.</param>
        /// <returns>Треды.</returns>
        public static async Task RemoveFromFavoriteThreads(this BoardLinkBase link, bool reportError = true)
        {
            if (link != null)
            {
                try
                {
                    var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                    var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                    var favs = await storage.ThreadData.FavoriteThreads.LoadLinkCollection() as ThreadLinkCollection;
                    if (favs?.Links != null)
                    {
                        var hash = linkHash.GetLinkHash(link);
                        if (favs.ThreadInfo?.ContainsKey(hash) ?? false)
                        {
                            favs.ThreadInfo.Remove(hash);
                        }
                        favs.Links = favs.Links.Where(l => linkHash.GetLinkHash(l) != hash).ToList();
                        await storage.ThreadData.FavoriteThreads.SaveLinkCollection(favs);
                    }
                }
                catch (Exception ex)
                {
                    if (reportError)
                    {
                        await AppHelpers.ShowError(ex);
                    }
                    else
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
                finally
                {
                    ViewModelEvents.FavoritesListRefreshed.RaiseEvent(link, null);
                }
            }
        }

        /// <summary>
        /// Добавить в избранные треды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="info">Информация о треде.</param>
        /// <param name="reportError">Показать ошибку на экране.</param>
        /// <returns>Треды.</returns>
        public static async Task AddToFavoriteThreads(this BoardLinkBase link, ShortThreadInfo info = null, bool reportError = true)
        {
            if (link != null)
            {
                try
                {
                    var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                    var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                    var threadProcess = ServiceLocator.Current.GetServiceOrThrow<IThreadTreeProcessService>();
                    var comparer = linkHash.GetComparer();
                    var hash = linkHash.GetLinkHash(link);
                    var favs = await storage.ThreadData.FavoriteThreads.LoadLinkCollection() as ThreadLinkCollection;
                    if (favs == null)
                    {
                        favs = new ThreadLinkCollection();
                    }
                    if (favs.Links == null)
                    {
                        favs.Links = new List<BoardLinkBase>();
                    }
                    if (favs.ThreadInfo == null)
                    {
                        favs.ThreadInfo = new Dictionary<string, ShortThreadInfo>();
                    }
                    if (favs.Links.Any(f => comparer.Equals(f, link)))
                    {
                        return;
                    }
                    if (info == null)
                    {
                        var visited = await storage.ThreadData.VisitedThreads.LoadLinkCollectionForReadOnly() as ThreadLinkCollection;
                        if (visited?.ThreadInfo != null)
                        {
                            if (visited.ThreadInfo.ContainsKey(hash))
                            {
                                info = visited.ThreadInfo[hash];
                            }
                        }
                    }
                    if (info == null)
                    {
                        var data = await storage.ThreadData.LoadThread(link);
                        if (data != null)
                        {
                            info = threadProcess.GetShortInfo(data);
                        }
                    }
                    if (info == null)
                    {
                        throw new InvalidOperationException("Информация о треде не найдена");
                    }
                    var pc = await storage.ThreadData.LoadPostCountInfo(link) ?? PostCountInfo.Create(link);
                    favs.Links.Add(link);
                    favs.Links = favs.Links.Deduplicate(f => f, comparer).ToList();
                    var favInfo = new FavoriteThreadInfo()
                    {
                        CountInfo = pc,
                        SmallImage = info.SmallImage,
                        Title = info.Title,
                        AddedDate = DateTime.Now,
                        CreatedDate = info.CreatedDate,
                        UpdatedDate = info.UpdatedDate,
                        ViewDate = info.ViewDate
                    };
                    favs.ThreadInfo[hash] = favInfo;
                    await storage.ThreadData.FavoriteThreads.SaveLinkCollection(favs);
                }
                catch (Exception ex)
                {
                    if (reportError)
                    {
                        await AppHelpers.ShowError(ex);
                    }
                    else
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
                finally
                {
                    ViewModelEvents.FavoritesListRefreshed.RaiseEvent(link, null);
                }
            }
        }
    }
}