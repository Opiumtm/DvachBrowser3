using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Имя в посте.
    /// </summary>
    public sealed class PostNameViewModel : ViewModelBase, IPostNameViewModel
    {
        private readonly PostTree postData;

        /// <summary>
        /// Родительcкая модель.
        /// </summary>
        public IPostViewModel Parent { get; }

        private string name;

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private string tripcode;

        /// <summary>
        /// Трипкод.
        /// </summary>
        public string TripCode
        {
            get { return tripcode; }
            private set
            {
                tripcode = value;
                RaisePropertyChanged();
            }
        }

        private IImageSourceViewModel icon;

        /// <summary>
        /// Иконка.
        /// </summary>
        public IImageSourceViewModel Icon
        {
            get { return icon; }
            private set
            {
                icon = value;
                RaisePropertyChanged();
            }
        }

        private string iconName;

        /// <summary>
        /// Имя иконки.
        /// </summary>
        public string IconName
        {
            get { return iconName ?? ""; }
            private set
            {
                iconName = value;
                RaisePropertyChanged();
            }
        }

        private IImageSourceViewModel flag;

        /// <summary>
        /// Флаг.
        /// </summary>
        public IImageSourceViewModel Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postData">Данные.</param>
        /// <param name="parent">Родитель.</param>
        public PostNameViewModel(PostTree postData, IPostViewModel parent)
        {
            this.postData = postData;
            Parent = parent;
            TripCode = "";
            Name = "";
            AppHelpers.Dispatcher.DispatchAsync(() => SetData(), 5);
        }

        private async void SetData()
        {
            try
            {
                var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                string newName = null;
                if (postData != null)
                {
                    var boardLink = linkTransform.BoardLinkFromAnyLink(postData.Link);
                    if (postData.Extensions != null)
                    {
                        var nameExtension = postData.Extensions.OfType<PostTreePosterExtension>().FirstOrDefault();
                        if (nameExtension != null)
                        {
                            newName = nameExtension.Name ?? "";
                            TripCode = nameExtension.Tripcode ?? "";
                        }
                    }
                    if (newName == null)
                    {
                        if (boardLink?.Engine != null)
                        {
                            newName = await GetDefaultName(boardLink, hashService);
                        }
                    }
                }
                if (newName == null)
                {
                    newName = "Аноним";
                }
                Name = newName;
                if (postData?.Extensions != null)
                {
                    var flagExtension = postData.Extensions.OfType<PostTreeCountryExtension>().FirstOrDefault();
                    if (flagExtension != null)
                    {
                        Flag = new ImageSourceViewModel(new MediaLink() { Engine = postData?.Link?.Engine ?? "", IsAbsolute = false, RelativeUri = flagExtension.Uri });
                        if (!Flag.SetImageCache(StaticImageCache.IconsAndFlags))
                        {
                            Flag.Load.Start();
                        }
                    }
                    var iconExtension = postData.Extensions.OfType<PostTreeIconExtension>().FirstOrDefault();
                    if (iconExtension != null)
                    {
                        Icon = new ImageSourceViewModel(new MediaLink() { Engine = postData?.Link?.Engine ?? "", IsAbsolute = false, RelativeUri = iconExtension.Uri });
                        if (!Icon.SetImageCache(StaticImageCache.IconsAndFlags))
                        {
                            Icon.Load.Start();
                        }
                        IconName = iconExtension.Description ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private static readonly Dictionary<string, string> DefaultNameCache = new Dictionary<string, string>();

        private async Task<string> GetDefaultName(BoardLinkBase boardLink, ILinkHashService hashService)
        {
            var hash = hashService.GetLinkHash(boardLink);
            if (!DefaultNameCache.ContainsKey(hash))
            {
                string defName = null;
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var engine = engines.FindEngine(boardLink.Engine);
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var refs = await storage.ThreadData.LoadBoardReferences(engine.RootLink);
                if (refs?.References != null)
                {
                    var eq = hashService.GetComparer();
                    var r1 = refs.References.FirstOrDefault(r => eq.Equals(r.Link, boardLink));
                    if (r1.Extensions != null)
                    {
                        var ext = r1.Extensions.OfType<MakabaBoardReferenceExtension>().FirstOrDefault();
                        if (ext != null)
                        {
                            defName = ext.DefaultName;
                        }
                    }
                }
                DefaultNameCache[hash] = defName;
            }
            return DefaultNameCache[hash];
        }
    }
}