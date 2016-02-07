using System;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовая модель тайла треда.
    /// </summary>
    /// <typeparam name="T">Тип информации о треде.</typeparam>
    public abstract class ThreadTileViewModelBase<T> : ViewModelBase, IThreadTileViewModel, IThreadTileUpdater where T : ShortThreadInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        protected ThreadTileViewModelBase(BoardLinkBase link)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            Link = link;
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            Board = $"/{linkTransform.GetBoardShortName(Link)}/";
            ThreadNumber = linkTransform.GetPostNum(Link) ?? 0;
        }

        /// <summary>
        /// Обновить данные.
        /// </summary>
        /// <param name="collection">Коллекция.</param>
        /// <returns>Короткая информация.</returns>
        public ShortThreadInfo UpdateData(ThreadLinkCollection collection)
        {
            if (collection?.ThreadInfo != null)
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var hash = linkHash.GetLinkHash(Link);
                if (collection.ThreadInfo.ContainsKey(hash))
                {
                    var data = collection.ThreadInfo[hash] as T;
                    if (data != null)
                    {
                        SetData(data);
                        return data;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Установить данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        protected virtual void SetData(T data)
        {
            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            Name = data.Title;
            if (data.SmallImage?.Link == null)
            {
                Image = null;
            }
            else
            {
                var needUpdate = false;
                if (lastImage == null)
                {
                    needUpdate = true;
                }
                else
                {
                    var hash1 = linkHash.GetLinkHash(lastImage);
                    var hash2 = linkHash.GetLinkHash(data.SmallImage.Link);
                    if (hash1 != hash2)
                    {
                        needUpdate = true;
                    }
                }
                if (needUpdate)
                {
                    Image = new ImageSourceViewModel(data.SmallImage.Link);
                }
            }
            lastImage = data.SmallImage?.Link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        private string board;

        /// <summary>
        /// Борда.
        /// </summary>
        public string Board
        {
            get { return board; }
            protected set
            {
                board = value;
                RaisePropertyChanged();
            }
        }

        private int threadNumber;

        /// <summary>
        /// Номер треда.
        /// </summary>
        public int ThreadNumber
        {
            get { return threadNumber; }
            protected set
            {
                threadNumber = value;
                RaisePropertyChanged();
            }
        }

        private string name;

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get { return name; }
            protected set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private BoardLinkBase lastImage;

        private IImageSourceViewModel image;

        /// <summary>
        /// Изображение.
        /// </summary>
        public IImageSourceViewModel Image
        {
            get { return image; }
            protected set
            {
                image = value;
                RaisePropertyChanged();
            }
        }

        private bool hasNewPosts;

        /// <summary>
        /// Есть новые посты.
        /// </summary>
        public bool HasNewPosts
        {
            get { return hasNewPosts; }
            protected set
            {
                hasNewPosts = value;
                RaisePropertyChanged();
            }
        }

        private int newPosts;

        /// <summary>
        /// Новые посты.
        /// </summary>
        public int NewPosts
        {
            get { return newPosts; }
            protected set
            {
                newPosts = value;
                RaisePropertyChanged();
            }
        }
    }
}