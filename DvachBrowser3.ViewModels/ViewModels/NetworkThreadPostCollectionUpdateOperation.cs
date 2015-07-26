using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Операция обновления треда.
    /// </summary>
    public sealed class NetworkThreadPostCollectionUpdateOperation : NetworkOperationWrapperBase, INetworkViewModel<PostCollectionUpdateMode>
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public NetworkThreadPostCollectionUpdateOperation(BoardLinkBase link, Func<CancellationToken> cancellationToken = null) : base(cancellationToken)
        {
            if (link == null) throw new ArgumentNullException("link");
            Link = link;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="arg">Аргумент.</param>
        public async void ExecuteOperation(PostCollectionUpdateMode arg)
        {
            var fullThread = ServiceLocator.Current.GetServiceOrThrow<IViewConfigurationService>().NetworkProfile.PartialThreadLoad;
            UpdateThreadMode updateMode;
            switch (arg)
            {
                case PostCollectionUpdateMode.Full:
                    updateMode = UpdateThreadMode.DefaultFull;
                    break;
                case PostCollectionUpdateMode.Partial:
                    updateMode = UpdateThreadMode.DefaultPartial;
                    break;
                case PostCollectionUpdateMode.Reload:
                    updateMode = UpdateThreadMode.DefaultReload;
                    break;
                default:
                    updateMode = fullThread ? UpdateThreadMode.DefaultFull : UpdateThreadMode.DefaultPartial;
                    break;
            }
            await DoExecuteOperation(arg, updateMode);
        }

        private async Task DoExecuteOperation(PostCollectionUpdateMode arg, UpdateThreadMode updateMode)
        {
            try
            {
                if (!IsCanExecute)
                {
                    return;
                }
                IsCanExecute = false;
                IsExecuting = true;
                try
                {
                    IsOk = false;
                    IsError = false;
                    OnStarted();
                    var operation = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadThread(Link, updateMode);
                    operation.Progress += OperationOnProgress;
                    try
                    {
                        var r = await operation.Complete(CancellationToken != null ? CancellationToken() : new CancellationToken());
                        var my = await ServiceLocator.Current.GetServiceOrThrow<IStorageService>().ThreadData.LoadMyPostsInfo(Link);
                        IsOk = true;
                        IsError = false;
                        ErrorText = null;
                        OnSetResult(new PostCollectionLoadedEventArgs(r, my, arg));
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        IsOk = false;
                        ErrorText = ex.Message;
                        OnError();
                    }
                }
                finally
                {
                    IsCanExecute = true;
                    IsExecuting = false;
                    OnCompleted();
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }            
        }

        private void OperationOnProgress(object sender, EngineProgress engineProgress)
        {
            OnProgress(engineProgress);
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public override void Execute(object parameter)
        {
            if (parameter == null)
            {
                ExecuteOperation(PostCollectionUpdateMode.Default);
                return;
            }
            if (parameter.GetType() != typeof (PostCollectionUpdateMode))
            {
                ExecuteOperation(PostCollectionUpdateMode.Default);
                return;                
            }
            ExecuteOperation((PostCollectionUpdateMode)parameter);
        }        

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        public override void ExecuteOperation()
        {
            ExecuteOperation(PostCollectionUpdateMode.Default);
        }

        /// <summary>
        /// Результат.
        /// </summary>
        public event PostCollectionLoadedEventHandler SetResult;

        private void OnSetResult(PostCollectionLoadedEventArgs e)
        {
            PostCollectionLoadedEventHandler handler = SetResult;
            if (handler != null) handler(this, e);
        }
    }
}