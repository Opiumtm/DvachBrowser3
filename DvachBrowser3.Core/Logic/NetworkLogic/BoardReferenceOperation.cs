using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция получения ссылок на борду.
    /// </summary>
    public class BoardReferenceOperation : NetworkLogicOperation<BoardReferences, BoardReferencesParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public BoardReferenceOperation(IServiceProvider services, BoardReferencesParameter parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public async override Task<BoardReferences> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.RootLink);
            if ((engine.Capability & EngineCapability.BoardsListRequest) == 0)
            {
                return null;
            }
            var storage = GetStorage();
            var oldData = await storage.ThreadData.LoadBoardReferences(Parameter.RootLink);
            var notNeedRefresh = !Parameter.ForceUpdate && oldData != null && oldData.CheckTime.Date == DateTime.Now.Date;
            if (notNeedRefresh)
            {
                return oldData;
            }
            var newDataOperation = engine.GetBoardsList();
            newDataOperation.Progress += (sender, e) => OnProgress(e);
            var newDataList = await newDataOperation.Complete(token);
            if (newDataList.Boards == null)
            {
                return null;
            }
            var newData = new BoardReferences()
            {
                RootLink = Parameter.RootLink,
                CheckTime = DateTime.Now,
                References = newDataList.Boards
            };
            await storage.ThreadData.SaveBoardReferences(newData);
            return newData;
        }
    }
}