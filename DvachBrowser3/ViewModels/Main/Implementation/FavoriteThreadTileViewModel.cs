using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл избранного треда.
    /// </summary>
    public sealed class FavoriteThreadTileViewModel : ThreadTileViewModelBase<FavoriteThreadInfo>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public FavoriteThreadTileViewModel(BoardLinkBase link) : base(link)
        {
        }

        /// <summary>
        /// Установить данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        protected override void SetData(FavoriteThreadInfo data)
        {
            base.SetData(data);
            if (data.CountInfo != null)
            {
                var count = Math.Max(data.CountInfo.LoadedPostCount, data.CountInfo.ViewedPostCount);
                HasNewPosts = data.CountInfo.PostCount > count;
                NewPosts = data.CountInfo.PostCount - count;
            }
            else
            {
                HasNewPosts = false;
            }
        }
    }
}