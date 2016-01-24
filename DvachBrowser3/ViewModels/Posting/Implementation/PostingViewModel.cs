﻿using System;
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
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postingLink">Ссылка.</param>
        public PostingViewModel(BoardLinkBase postingLink)
        {
            if (postingLink == null) throw new ArgumentNullException(nameof(postingLink));
            PostingLink = postingLink;
            operation = new StdEngineOperationWrapper<Logic.IPostingResult>(OperationFactory);
            operation.Finished += OperationOnFinished;
            operation.ResultGot += OperationOnResultGot;
            Fields = new PostingFieldsViewModel(this);
            Fields.Flushed += FieldsOnFlushed;
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

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Start()
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var data = await storage.PostData.LoadPostData(PostingLink);
                if (data == null)
                {
                    Fields.SetDefault();
                    SaveTime = "";
                }
                else
                {
                    Fields.Load(data.FieldData);
                    SaveTime = data.SaveTime.ToString("g");
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
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
    }
}