using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов c возможностью обновления.
    /// </summary>
    /// <typeparam name="T">Тип коллекции.</typeparam>
    public abstract class UpdateablePostCollectionViewModelBase<T> : PostCollectionViewModelBase, IStartableViewModelWithResume where T : IPostTreeListSource
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected UpdateablePostCollectionViewModelBase()
        {
            UpdateOperation = new StdEngineOperationWrapper<T>(UpdateOperationFactory) { HighPriority = true };
            UpdateOperation.ResultGot += UpdateOperationOnResultGot;
            UpdateOperation.Finished += UpdateOperationOnFinished;
        }

        /// <summary>
        /// Получение данных завершено.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Cобытие.</param>
        protected async virtual void UpdateOperationOnFinished(object sender, OperationProgressFinishedEventArgs e)
        {
            if (e.Error != null)
            {
                await AppHelpers.ShowError(e.Error);
            }
        }

        /// <summary>
        /// Данные получены.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Cобытие.</param>
        protected virtual void UpdateOperationOnResultGot(object sender, EventArgs e)
        {
            var result = UpdateOperation.Result;
            UpdateOperationOnResultGot(result);
        }

        /// <summary>
        /// Данные получены.
        /// </summary>
        /// <param name="result">Результат.</param>
        protected virtual void UpdateOperationOnResultGot(T result)
        {
            if (result != null)
            {
                SetData(result);
                OnPostsUpdated();
            }
        }

        /// <summary>
        /// Установить данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        protected async virtual void SetData(T data)
        {
            var posts = data != null ? await data.GetPosts() : null;
            MergePosts(posts);
            HasData = true;
        }

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        protected StdEngineOperationWrapper<T> UpdateOperation { get; }

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        public override IOperationViewModel Update => UpdateOperation;

        /// <summary>
        /// Может обновляться.
        /// </summary>
        public override bool CanUpdate => true;

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="o">Данные.</param>
        /// <returns>Операция.</returns>
        protected abstract IEngineOperationsWithProgress<T, EngineProgress> UpdateOperationFactory(object o);

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public virtual Task Start()
        {
            Update.Start();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public virtual Task Stop()
        {
            Update.Cancel();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual Task Resume()
        {
            if (!HasData)
            {
                Update.Start();
            }
            return Task.FromResult(true);
        }
    }
}