using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Storage;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Операция синхронизации списка борд.
    /// </summary>
    public class SyncBoardsOperation : NetworkLogicOperation<ICollection<IBoardListBoardViewModel>, bool>
    {        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public SyncBoardsOperation(bool parameter) : base(ServiceLocator.Current, parameter)
        {
        }

        private ISet<string> DisabledEngines { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public async override Task<ICollection<IBoardListBoardViewModel>> Complete(CancellationToken token)
        {
            var engines = Services.GetServiceOrThrow<INetworkEngines>();
            var networkLogic = Services.GetServiceOrThrow<INetworkLogic>();
            var storage = Services.GetServiceOrThrow<IStorageService>();
            var allEngines = engines.ListEngines().Where(id => !DisabledEngines.Contains(id)).Select(id => engines.GetEngineById(id)).Where(e => (e.Capability & EngineCapability.BoardsListRequest) != 0).ToArray();
            var result = new List<IBoardListBoardViewModel>();
            foreach (var engine in allEngines)
            {
                var rootLink = engine.RootLink;
                BoardReferences res = null;
                try
                {
                    var resOperation = networkLogic.GetBoardReferences(rootLink, Parameter);
                    resOperation.Progress += (sender, e) => OnProgress(e);
                    res = await resOperation.Complete(token);
                }
                catch (Exception ex)
                {
                    if (Parameter)
                    {
                        await AppHelpers.ShowError(ex);
                    }
                }
                if (res == null)
                {
                    try
                    {
                        res = await storage.ThreadData.LoadBoardReferences(rootLink);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
                if (res != null && res.References != null)
                {
                    result.AddRange(res.References.Select(r => new BoardListBoardViewModel(r)));
                }
            }
            return result;
        }
    }
}