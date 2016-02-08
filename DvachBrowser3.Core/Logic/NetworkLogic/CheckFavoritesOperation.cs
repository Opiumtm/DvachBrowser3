using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция проверки избранных тредов.
    /// </summary>
    public class CheckFavoritesOperation : NetworkLogicOperation<LinkCollection, CheckFavoritesParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public CheckFavoritesOperation(IServiceProvider services, CheckFavoritesParameter parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<LinkCollection> Complete(CancellationToken token)
        {
            try
            {
                var storage = Services.GetServiceOrThrow<IStorageService>();
                var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
                var liveTileService = Services.GetServiceOrThrow<ILiveTileService>();
                var asyncCheck = (Parameter.Mode & CheckFavoriteThreadsMode.Async) != 0;
                var saveData = (Parameter.Mode & CheckFavoriteThreadsMode.UpdateData) != 0;
                var favorites = await storage.ThreadData.FavoriteThreads.LoadLinkCollection();
                var favorites2 = favorites as ThreadLinkCollection;
                if (favorites2 == null || favorites2.ThreadInfo == null)
                {
                    return null;
                }
                var linkWithData = favorites.Links
                    .WithKeys(linkHashService.GetLinkHash)
                    .Where(l => favorites2.ThreadInfo.ContainsKey(l.Key))
                    .Select(l => new CheckData() { Link = l.Value, Info = favorites2.ThreadInfo[l.Key] as FavoriteThreadInfo, Key = l.Key })
                    .Where(l => l.Info != null)
                    .ToArray();
                var totalCount = linkWithData.Length;
                var counter = 0;
                maxCount = 0;
                var result = new Dictionary<string, CheckResult>();
                SignalProgress(-1, totalCount);
                if (asyncCheck)
                {
                    var checks = linkWithData.Select(l => CheckForUpdateMp(l.Link, l.Info, l.Key, token, result)).ToArray();
                    await Task.WhenAll(checks);
                }
                else
                {
                    foreach (var item in linkWithData)
                    {
                        var d = await CheckForUpdate(item.Link, item.Info, token);
                        if (d != null)
                        {
                            result[item.Key] = d;
                        }
                        counter++;
                        SignalProgress(counter, totalCount);
                    }
                }
                foreach (var kv in result)
                {
                    if (favorites2.ThreadInfo.ContainsKey(kv.Key))
                    {
                        var d = favorites2.ThreadInfo[kv.Key] as FavoriteThreadInfo;
                        if (d != null)
                        {
                            d.CountInfo = kv.Value.Data;
                        }
                    }
                    if (saveData)
                    {
                        if (asyncCheck)
                        {
                            SaveCountInfoAsync(storage, kv.Value);
                        }
                        else
                        {
                            await SaveCountInfo(storage, kv.Value);
                        }
                    }
                }
                if (saveData)
                {
                    if (asyncCheck)
                    {
                        await storage.ThreadData.FavoriteThreads.SaveLinkCollection(favorites2);
                    }
                    else
                    {
                        await storage.ThreadData.FavoriteThreads.SaveLinkCollectionSync(favorites2);
                    }
                    await liveTileService.UpdateFavoritesTile(favorites2);
                }
                return favorites2;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        private async Task SaveCountInfo(IStorageService service, CheckResult result)
        {
            await service.ThreadData.SavePostCountInfo(result.Data);
        }

        private void SaveCountInfoAsync(IStorageService service, CheckResult result)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await SaveCountInfo(service, result);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            });
        }

        private void SignalProgress(int count, int totalCount)
        {
            try
            {
                var msg = string.Format("Проверка {0}/{1}", count + 1, totalCount);
                var percent = totalCount > 0 ? (double) (count + 1)/totalCount * 100.0 : 0.0;
                dynamic other = new ExpandoObject();
                other.Kind = "THREAD STATUS";
                OnProgress(new EngineProgress(msg, percent, other));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);                
            }
        }

        private readonly object signalLock = new object();

        private int maxCount;

        private void SignalProgressAsync(int count, int totalCount)
        {
            Task.Factory.StartNew(() =>
            {
                lock (signalLock)
                {
                    maxCount = Math.Max(maxCount, count);
                }
                SignalProgress(maxCount, totalCount);
            });
        }

        private struct CheckData
        {
            public BoardLinkBase Link;
            public FavoriteThreadInfo Info;
            public string Key;
        }

        private class CheckResult
        {
            public PostCountInfo Data;
        }

        private static readonly IConcurrenctyDispatcher<CheckResult> MaxParallel = new MaxConcurrencyAccessManager<CheckResult>(3);

        private async Task CheckForUpdateMp(BoardLinkBase link, FavoriteThreadInfo info, string key, CancellationToken token, Dictionary<string, CheckResult> result)
        {
            var d = await MaxParallel.QueueAction(async () => await CheckForUpdate(link, info, token));
            result[key] = d;
        }

        private async Task<CheckResult> CheckForUpdate(BoardLinkBase link, FavoriteThreadInfo info, CancellationToken token)
        {
            try
            {
                if (info == null)
                {
                    return null;
                }
                var engine = GetEngine(link);
                if ((engine.Capability & EngineCapability.ThreadStatusRequest) == 0)
                {
                    return null;
                }
                PostCountInfo countInfo;
                if (info.CountInfo == null)
                {
                    countInfo = PostCountInfo.Create(link);
                }
                else
                {
                    countInfo = info.CountInfo.Clone();
                }
                token.ThrowIfCancellationRequested();
                var statusOperation = engine.GetThreadStatus(link);
                var result = await statusOperation.Complete(token);
                if (result == null || !result.IsFound || result.TotalPosts == null)
                {
                    return null;
                }
                countInfo.LastChange = result.LastUpdate;
                countInfo.PostCount = result.TotalPosts.Value;
                return new CheckResult()
                {
                    Data = countInfo
                };
            }
            catch
            {
                return null;
            }
        }
    }
}