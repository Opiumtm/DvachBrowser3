using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поля постинга.
    /// </summary>
    public sealed class PostingFieldsViewModel : ViewModelBase, IPostingFieldsViewModel
    {
        private readonly BoardLinkBase boardLink;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public PostingFieldsViewModel(IPostingViewModel parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (parent?.PostingLink == null)
            {
                throw new ArgumentException("Должна быть указана ссылка постинга");
            }
            boardLink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().BoardLinkFromAnyLink(parent.PostingLink);
            if (boardLink == null)
            {
                throw new ArgumentException("Ссылка для постинга должна содержать информацию о доске");
            }
            Parent = parent;
            flushDelay = new EventDelayHelper(TimeSpan.FromSeconds(0.5));
            flushDelay.EventFired += FlushDelayOnEventFired;
            AppHelpers.DispatchAction(InitializeData, true);
        }

        private async Task InitializeData()
        {
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var engine = engines.FindEngine(Parent.PostingLink.Engine);
            if (engine == null)
            {
                throw new InvalidOperationException("Сетевой движок не найден");
            }
            var rootLink = engine.RootLink;
            var boardInfo = await storage.ThreadData.LoadBoardReferences(rootLink);
            BoardReferencePostingExtension extension = null;
            BoardReference boardInfo2 = null;
            if (boardInfo?.References != null)
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var eqComparer = linkHash.GetComparer();
                boardInfo2 = boardInfo.References.FirstOrDefault(d => eqComparer.Equals(d.Link, boardLink));
                if (boardInfo2 != null)
                {
                    extension = boardInfo2.Extensions?.OfType<BoardReferencePostingExtension>()?.FirstOrDefault();
                }
            }
            if (extension == null)
            {
                var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                boardInfo2 = engine.GetDefaultBoardData("", linkTransform.GetBoardShortName(boardLink));
                if (boardInfo2 != null)
                {
                    extension = boardInfo2.Extensions?.OfType<BoardReferencePostingExtension>()?.FirstOrDefault();
                }
            }
            if (extension?.Capabilities == null)
            {
                throw new InvalidOperationException("Невозможно получить информацию о полях для постинга для данной доски");
            }
            var caps = extension.Capabilities.Deduplicate(c => c.Role).ToDictionary(c => c.Role);
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.Title), PostingFieldSemanticRole.Title) { DefaultValue = "" }, nameof(Title));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.Comment), PostingFieldSemanticRole.Comment) { DefaultValue = "" }, nameof(Comment));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.Email), PostingFieldSemanticRole.Email) { DefaultValue = "" }, nameof(Email));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.PosterName), PostingFieldSemanticRole.PosterName) { DefaultValue = "" }, nameof(PosterName));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.PosterTrip), PostingFieldSemanticRole.PosterTrip) { DefaultValue = "" }, nameof(PosterTrip1));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.PosterTrip2), PostingFieldSemanticRole.PosterTrip2) { DefaultValue = "" }, nameof(PosterTrip2));


            MakabaBoardReferenceExtension makaba = boardInfo2?.Extensions?.OfType<MakabaBoardReferenceExtension>()?.FirstOrDefault();

            if (makaba != null)
            {
                SetField(new PostingIconViewModel(this, caps.ContainsKey(PostingFieldSemanticRole.Icon), PostingFieldSemanticRole.Icon, makaba, boardLink.Engine), nameof(Icon));
            }
            else
            {
                SetField(new PostingIconViewModel(this, false, PostingFieldSemanticRole.Icon, null, boardLink.Engine), nameof(Icon));
            }

            SetField(new PostingFieldViewModel<bool>(this, caps.ContainsKey(PostingFieldSemanticRole.SageFlag), PostingFieldSemanticRole.SageFlag) { DefaultValue = false }, nameof(SageFlag));
            SetField(new PostingFieldViewModel<bool>(this, caps.ContainsKey(PostingFieldSemanticRole.WatermarkFlag), PostingFieldSemanticRole.WatermarkFlag) { DefaultValue = false }, nameof(WatermarkFlag));
            SetField(new PostingFieldViewModel<bool>(this, caps.ContainsKey(PostingFieldSemanticRole.OpFlag), PostingFieldSemanticRole.OpFlag) { DefaultValue = false }, nameof(OpFlag));
            SetField(new PostingFieldViewModel<string>(this, caps.ContainsKey(PostingFieldSemanticRole.ThreadTag), PostingFieldSemanticRole.ThreadTag) { DefaultValue = "" }, nameof(ThreadTag));

            var media = caps.ContainsKey(PostingFieldSemanticRole.MediaFile) ? caps[PostingFieldSemanticRole.MediaFile] as PostingMediaFileCapability : null;

            SetField(new PostingMediaCollectionViewModel(this, caps.ContainsKey(PostingFieldSemanticRole.MediaFile), PostingFieldSemanticRole.MediaFile, media?.MaxFileCount ?? 1), nameof(Media));
        }

        private readonly Dictionary<PostingFieldSemanticRole, IPostingFieldDataProvider> data = new Dictionary<PostingFieldSemanticRole, IPostingFieldDataProvider>();

        private T GetField<T>(PostingFieldSemanticRole role) where T : class, IPostingFieldDataProvider 
        {
            if (data.ContainsKey(role))
            {
                return data[role] as T;
            }
            return null;
        }

        private void SetField(IPostingFieldDataProvider v, string propertyName)
        {
            if (v != null)
            {
                data[v.Role] = v;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostingViewModel Parent { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public IPostingFieldViewModel<string> Title => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.Title);

        /// <summary>
        /// Комментарий.
        /// </summary>
        public IPostingFieldViewModel<string> Comment => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.Comment);

        /// <summary>
        /// Адрес почты.
        /// </summary>
        public IPostingFieldViewModel<string> Email => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.Email);

        /// <summary>
        /// Имя постера.
        /// </summary>
        public IPostingFieldViewModel<string> PosterName => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.PosterName);

        /// <summary>
        /// Трипкод (первая часть).
        /// </summary>
        public IPostingFieldViewModel<string> PosterTrip1 => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.PosterTrip);

        /// <summary>
        /// Трипкод (вторая часть).
        /// </summary>
        public IPostingFieldViewModel<string> PosterTrip2 => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.PosterTrip2);

        /// <summary>
        /// Иконка.
        /// </summary>
        public IPostingIconViewModel Icon => GetField<IPostingIconViewModel>(PostingFieldSemanticRole.Icon);

        /// <summary>
        /// Имя постера.
        /// </summary>
        public IPostingFieldViewModel<bool> SageFlag => GetField<IPostingFieldViewModel<bool>>(PostingFieldSemanticRole.SageFlag);

        /// <summary>
        /// Вотермарка.
        /// </summary>
        public IPostingFieldViewModel<bool> WatermarkFlag => GetField<IPostingFieldViewModel<bool>>(PostingFieldSemanticRole.WatermarkFlag);

        /// <summary>
        /// Вотермарка.
        /// </summary>
        public IPostingFieldViewModel<bool> OpFlag => GetField<IPostingFieldViewModel<bool>>(PostingFieldSemanticRole.OpFlag);

        /// <summary>
        /// Медиа файлы.
        /// </summary>
        public IPostingMediaCollectionViewModel Media => GetField<IPostingMediaCollectionViewModel>(PostingFieldSemanticRole.MediaFile);

        /// <summary>
        /// Тэг треда.
        /// </summary>
        public IPostingFieldViewModel<string> ThreadTag => GetField<IPostingFieldViewModel<string>>(PostingFieldSemanticRole.ThreadTag);

        /// <summary>
        /// Сохранить хранилище полей.
        /// </summary>
        /// <param name="data1">Данные.</param>
        public void Save(IDictionary<PostingFieldSemanticRole, object> data1)
        {
            if (data1 != null)
            {
                data1.Clear();
                foreach (var kv in data)
                {
                    if (kv.Value != null)
                    {
                        var kv2 = kv.Value.GetValueData();
                        if (kv2 != null)
                        {
                            data1[kv2.Value.Key] = kv2.Value.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить хранилище полей.
        /// </summary>
        /// <param name="data1">Данные.</param>
        public void Load(IDictionary<PostingFieldSemanticRole, object> data1)
        {
            foreach (var kv in data)
            {
                kv.Value?.SetDefaultValueData(false);
            }
            if (data1 != null)
            {
                foreach (var kv in data1.Where(kv => data.ContainsKey(kv.Key)))
                {
                    data[kv.Key].SetValueData(kv.Value, false);
                }
            }
        }

        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="immediate">Сохранить немедленно.</param>
        public async Task Flush(bool immediate = false)
        {
            if (immediate)
            {
                try
                {
                    await DoFlush();
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
            else
            {
                flushDelay.Trigger();
            }
        }

        private async void FlushDelayOnEventFired(object sender, EventArgs e)
        {
            try
            {
                await DoFlush();
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task DoFlush()
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var postData = new PostingData()
                {
                    FieldData = new Dictionary<PostingFieldSemanticRole, object>(),
                    Link = Parent?.PostingLink,
                    SaveTime = DateTime.Now
                };
                foreach (var v in data.Values.Where(v => v != null))
                {
                    var kv = v.GetValueData();
                    if (kv != null)
                    {
                        postData.FieldData[kv.Value.Key] = kv.Value.Value;
                    }
                }
                await storage.PostData.SavePostData(postData);
                Flushed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private readonly EventDelayHelper flushDelay;

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            foreach (var kv in data)
            {
                if (kv.Value != null)
                {
                    await kv.Value.Clear(false);
                }
            }
            await Flush(true);
        }

        /// <summary>
        /// Установить значения по умолчанию.
        /// </summary>
        public void SetDefault()
        {
            foreach (var kv in data)
            {
                kv.Value?.SetDefaultValueData(false);
            }
        }

        /// <summary>
        /// Сохранено.
        /// </summary>
        public event EventHandler Flushed;
    }
}