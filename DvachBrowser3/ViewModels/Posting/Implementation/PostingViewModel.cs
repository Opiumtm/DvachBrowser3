using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления постинга.
    /// </summary>
    public sealed class PostingViewModel : ViewModelBase, IPostingViewModel
    {
        private readonly string customId;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pageParam">Параметр страницы.</param>
        public PostingViewModel(ExtendedPageParam2 pageParam)
        {
            var postingLink = pageParam?.Link;
            this.customId = pageParam?.CustomDataId;
            if (postingLink == null) throw new ArgumentNullException(nameof(postingLink));
            PostingLink = postingLink;
            if ((PostingLink.LinkKind & BoardLinkKind.Thread) == 0)
            {
                IsNewThread = true;
            }
            operation = new StdEngineOperationWrapper<Logic.IPostingResult>(OperationFactory);
            operation.Finished += OperationOnFinished;
            operation.ResultGot += OperationOnResultGot;
            Fields = new PostingFieldsViewModel(this);
            Fields.Flushed += FieldsOnFlushed;
            Fields.Initialized += FieldsOnInitialized;
            Fields.Initialize();
            AppHelpers.DispatchAction(GetQuote);
        }

        private async Task GetQuote()
        {
            if (customId != null)
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var customData = await storage.CustomData.LoadCustomData(customId);
                if (customData?.ContainsKey("PostQuote") ?? false)
                {
                    var oquote = customData["PostQuote"]?.ToString();
                    if (oquote != null)
                    {
                        HasQuote = true;
                        Quote = oquote;
                    }
                }
            }
        }

        [Obsolete]
        private async Task GetQuoteOld()
        {
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            var comparer = linkHash.GetComparer();
            var threadLink = linkTransform.GetThreadLinkFromAnyLink(PostingLink);
            var postLink = linkTransform.GetPostLinkFromAnyLink(PostingLink);
            if (threadLink != null && postLink != null)
            {
                var data = await storage.ThreadData.LoadThread(threadLink);
                if (data?.Posts != null)
                {
                    var post = data.Posts.FirstOrDefault(p => comparer.Equals(p.Link, postLink));
                    if (post != null)
                    {
                        var text = post.ToPlainText();
                        var sb = new StringBuilder();
                        foreach (var line in text)
                        {
                            sb.Append(">");
                            sb.AppendLine(line);
                        }
                        HasQuote = true;
                        Quote = sb.ToString();
                    }
                }
            }
        }

        private void FieldsOnFlushed(object sender, EventArgs eventArgs)
        {
            SaveTime = DateTime.Now.ToString("g");
        }

        private readonly StdEngineOperationWrapper<Logic.IPostingResult> operation;

        private void OperationOnResultGot(object sender, EventArgs e)
        {
            var result = operation.Result;
            AppHelpers.ActionOnUiThread(() =>
            {
                if (result != null)
                {
                    if (result.Kind == PostingResultKind.NeedCaptcha)
                    {
                        NeedSetCaptcha?.Invoke(this, new NeedSetCaptchaEventArgs(result.CaptchaType, result.Engine));
                    }
                    if (result.Kind == PostingResultKind.Success)
                    {
                        PostingSuccess?.Invoke(this, new PostingSuccessEventArgs(result.RedirectLink));
                    }
                }
                return Task.CompletedTask;
            });
        }

        private void OperationOnFinished(object sender, OperationProgressFinishedEventArgs e)
        {
            if (e.Error != null && !e.IsCancelled)
            {
                AppHelpers.ActionOnUiThread(async () =>
                {
                    await AppHelpers.ShowError(e.Error);
                });
            }
        }

        private IEngineOperationsWithProgress<Logic.IPostingResult, EngineProgress> OperationFactory(object o)
        {
            var data = new PostingData()
            {
                Link = PostingLink,
                SaveTime = DateTime.Now
            };
            Fields.Save(data.FieldData);
            return new PostingOperation(ServiceLocator.Current, new PostingArgument()
            {
                Captcha = captcha,
                Data = data
            });
        }

        private PostingData data;

        private bool dataGot;

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Start()
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                data = await storage.PostData.LoadPostData(PostingLink);
                dataGot = true;
                if (data == null)
                {
                    SaveTime = "";
                }
                else
                {
                    SaveTime = data.SaveTime.ToString("g");
                }
                UpdateData();
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private bool isInitialized;

        private void FieldsOnInitialized(object sender, EventArgs eventArgs)
        {
            isInitialized = true;
            UpdateData();
        }

        private void UpdateData()
        {
            if (isInitialized && dataGot)
            {
                if (data == null)
                {
                    Fields.SetDefault();
                }
                else
                {
                    Fields.Load(data.FieldData);
                }
            }
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Stop()
        {
            if (Posting.Progress.IsActive)
            {
                Posting.Cancel();
            }
            else
            {
                await Fields.Flush(true);
            }
        }

        /// <summary>
        /// Ссылка, куда постинг.
        /// </summary>
        public BoardLinkBase PostingLink { get; }

        /// <summary>
        /// Новый тред.
        /// </summary>
        public bool IsNewThread { get; }

        /// <summary>
        /// Операция постинга.
        /// </summary>
        public IOperationViewModel Posting => operation;

        /// <summary>
        /// Поля.
        /// </summary>
        public IPostingFieldsViewModel Fields { get; }

        private CaptchaPostingData captcha;

        /// <summary>
        /// Капча.
        /// </summary>
        public CaptchaPostingData Captcha
        {
            get { return captcha; }
            set
            {
                captcha = value;
                RaisePropertyChanged();
            }
        }

        private string saveTime = "";

        /// <summary>
        /// Время сохранения.
        /// </summary>
        public string SaveTime
        {
            get { return saveTime; }
            private set
            {
                saveTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Необходимо установить капчу.
        /// </summary>
        public event NeedSetCaptchaEventHandler NeedSetCaptcha;

        /// <summary>
        /// Удачный постинг.
        /// </summary>
        public event PostingSuccessEventHandler PostingSuccess;

        /// <summary>
        /// Начать постинг.
        /// </summary>
        public void Post()
        {
            Posting.Start();
        }

        private bool hasQuote;

        /// <summary>
        /// Есть цитата.
        /// </summary>
        public bool HasQuote
        {
            get { return hasQuote; }
            private set
            {
                hasQuote = value;
                RaisePropertyChanged();
            }
        }

        private string quote;

        /// <summary>
        /// Цитата.
        /// </summary>
        public string Quote
        {
            get { return quote; }
            private set
            {
                quote = value;
                RaisePropertyChanged();
            }
        }
    }
}