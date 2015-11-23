﻿using System;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;
using Template10.Common;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Предварительный просмотр треда.
    /// </summary>
    public sealed class ThreadPreviewViewModel : StaticPostCollectionViewModelBase<ThreadPreviewTree>, IThreadPreviewViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="data">Данные.</param>
        public ThreadPreviewViewModel(IBoardPageViewModel parent, ThreadPreviewTree data) : base(data)
        {
            Parent = parent;
#pragma warning disable 4014
            BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() => UpdatePostCount());
#pragma warning restore 4014
        }

        private async void UpdatePostCount()
        {
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var data = await store.ThreadData.LoadPostCountInfo(CollectionData.Link);
                if (data == null)
                {
                    NotViewedPosts = 0;
                    IsHidden = false;
                    return;
                }
                var loadedPosts = data.LoadedPostCount;
                NotViewedPosts = Math.Max(0, PostCount - loadedPosts);
                IsHidden = data.IsHidden;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                NotViewedPosts = 0;
                IsHidden = false;
            }
        }

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected override IPostViewModel CreatePostViewModel(PostTree post)
        {
            return new PostViewModel(post, this);
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IBoardPageViewModel Parent { get; }

        /// <summary>
        /// Количство изображений.
        /// </summary>
        public int ImageCount => CollectionData.ImageCount;

        /// <summary>
        /// Пропущено изображений.
        /// </summary>
        public int OmitImages => CollectionData.OmitImages;

        /// <summary>
        /// Количество постов.
        /// </summary>
        public int PostCount => CollectionData.ReplyCount;

        /// <summary>
        /// Пропущено постов.
        /// </summary>
        public int OmitPosts => CollectionData.Omit;

        private int notViewedPosts;

        /// <summary>
        /// Не просмотренных постов.
        /// </summary>
        public int NotViewedPosts
        {
            get { return notViewedPosts; }
            private set
            {
                notViewedPosts = value;
                RaisePropertyChanged();
            }
        }

        private bool isHidden;

        /// <summary>
        /// Тред скрыт.
        /// </summary>
        public bool IsHidden
        {
            get { return isHidden; }
            set
            {
                isHidden = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Скрыть тред.
        /// </summary>
        public async void Hide()
        {
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var data = await store.ThreadData.LoadPostCountInfo(CollectionData.Link) ?? new PostCountInfo()
                {
                    Link = CollectionData.Link,
                    LastView = DateTime.MinValue,
                };
                data.IsHidden = true;
                await store.ThreadData.SavePostCountInfo(data);
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }
    }
}