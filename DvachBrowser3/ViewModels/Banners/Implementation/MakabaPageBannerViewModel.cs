using System;
using System.Linq;
using DvachBrowser3.Links;
using DvachBrowser3.Makaba;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Баннек движка Макаба.
    /// </summary>
    public sealed class MakabaPageBannerViewModel : PageBannerViewModelBase
    {
        private readonly MakabaEntityTree data;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="engine">Движок.</param>
        public MakabaPageBannerViewModel(MakabaEntityTree data, string engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            this.data = data;
            if (data?.BoardBannerImage != null)
            {
                BannerImageLink = new MediaLink()
                {
                    Engine = engine,
                    IsAbsolute = false,
                    RelativeUri = data.BoardBannerImage
                };
            }
            if (data?.BoardBannerLink != null)
            {
                BannerLink = new BoardLink()
                {
                    Engine = engine,
                    Board = data.BoardBannerLink
                };
            }
            AppHelpers.DispatchAction(LoadBannerName);
        }

        private async void LoadBannerName()
        {
            try
            {
                BannerLinkTitle = await BoardTitleHelper.GetBoardTitle(BannerLink);                
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                BannerLinkTitle = "";
            }
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public override BoardLinkBase BannerLink { get; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public override BoardLinkBase BannerImageLink { get; }

        /// <summary>
        /// Тип медиа.
        /// </summary>
        public override PageBannerMediaType MediaType
        {
            get
            {
                var afterDot = data.BoardBannerImage?.Split('.').LastOrDefault();
                if (afterDot == null)
                {
                    return PageBannerMediaType.Other;
                }
                switch (afterDot.ToLowerInvariant())
                {
                    case "gif":
                        return PageBannerMediaType.Gif;
                    case "jpg":
                    case "jpeg":
                        return PageBannerMediaType.Jpeg;
                    case "png":
                        return PageBannerMediaType.Png;
                    default:
                        return PageBannerMediaType.Other;
                }
            }
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public override int Height => 100;

        /// <summary>
        /// Ширина.
        /// </summary>
        public override int Width => 300;
    }
}