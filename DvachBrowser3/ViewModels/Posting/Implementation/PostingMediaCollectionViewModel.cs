using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция медиа для постинга.
    /// </summary>
    public sealed class PostingMediaCollectionViewModel : PostingFieldViewModelBase<Empty>, IPostingMediaCollectionViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="isSupported">Поддерживается.</param>
        /// <param name="role">Роль.</param>
        /// <param name="maxFiles">Максимальное количество файлов.</param>
        public PostingMediaCollectionViewModel(IPostingFieldsViewModel parent, bool isSupported, PostingFieldSemanticRole role, int? maxFiles) : base(parent, isSupported, role)
        {
            this.maxFiles = maxFiles;
            ((ObservableCollection<IPostingMediaViewModel>)Media).CollectionChanged += OnMediaCollectionChanged;
        }

        private void OnMediaCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(CanAdd));
        }

        /// <summary>
        /// Изображения.
        /// </summary>
        public IList<IPostingMediaViewModel> Media { get; } = new ObservableCollection<IPostingMediaViewModel>();

        private readonly int? maxFiles;

        /// <summary>
        /// Можно добавлять медиа файл.
        /// </summary>
        public bool CanAdd => maxFiles != null && Media.Count <= maxFiles;

        /// <summary>
        /// Добавить медиа файл.
        /// </summary>
        public async Task AddMediaFile(StorageFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var item = await storage.PostData.MediaStorage.AddMediaFile(file);
            Media.Add(new PostingMediaViewModel(this, new PostingMediaFile()
            {
                OriginalName = file.Name,
                Resize = false,
                AddUniqueId = false,
                MediaFileId = item.Id
            }));
            OnValueChange();
        }

        /// <summary>
        /// Выбрать медиа файл.
        /// </summary>
        public async Task ChooseMediaFile()
        {
            var fp = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif", ".webm" }
            };
            var f = await fp.PickSingleFileAsync();
            if (f != null)
            {
                await AddMediaFile(f);
            }
        }

        /// <summary>
        /// Очистить файлы.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override async Task Clear(bool flush = true)
        {
            var toDelete = Media.ToArray();
            foreach (var item in toDelete)
            {
                await item.Delete();
            }
            if (flush)
            {
                OnValueChange();
            }
        }

        /// <summary>
        /// Заполнить значение.
        /// </summary>
        /// <param name="data">Значение.</param>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override void SetValueData(object data, bool flush = true)
        {
            Media.Clear();
            var newData = data as PostingMediaFiles;
            if (newData?.Files != null)
            {
                foreach (var file in newData.Files)
                {
                    Media.Add(new PostingMediaViewModel(this, file));
                }
            }
            if (flush)
            {
                OnValueChange();
            }
        }

        /// <summary>
        /// Заполнить значение по умолчанию.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override void SetDefaultValueData(bool flush = true)
        {
            SetValueData(null, flush);
        }

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        public override KeyValuePair<PostingFieldSemanticRole, object>? GetValueData()
        {
            return new KeyValuePair<PostingFieldSemanticRole, object>(Role, new PostingMediaFiles()
            {
                Files = Media.Select(i => i.CreateData()).ToList()
            });
        }
    }
}