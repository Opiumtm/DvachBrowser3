using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DvachBrowser3.Behaviors;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.TextRender;
using DvachBrowser3.ViewModels;
using Microsoft.Xaml.Interactivity;

namespace DvachBrowser3.Views.Partial
{
    public static class RenderLinkClickHelper
    {
        public static void SetupLinkActions(FrameworkElement result, ITextRenderLinkAttribute linkAttribute, ILinkClickCallback linkClickCallback)
        {
            result.Tapped += (sender, e) =>
            {
                e.Handled = true;
                linkClickCallback?.OnLinkClick(linkAttribute);
            };
            var bc = new BehaviorCollection();
            var mf = new MenuFlyout();
            var mfi = new MenuFlyoutItem()
            {
                Text = "Копировать ссылку",
            };
            mfi.Click += MfiOnClick(linkAttribute);
            mf.Items?.Add(mfi);
            bc.Add(new PopupMenuBehavior()
            {
                MenuFlyout = mf
            });
            Interaction.SetBehaviors(result, bc);
            var youtubeLink = linkAttribute.CustomData as YoutubeLink;
            if (youtubeLink != null)
            {
                var youtubeVm = new YoutubeThumbnailImageSourceViewModel(youtubeLink.Engine, youtubeLink.YoutubeId);
                var youtubeImage = new PreviewImage()
                {
                    ViewModel = youtubeVm,
                    Height = youtubeVm.Height / 2.0,
                    Width = youtubeVm.Width / 2.0
                };
                ToolTipService.SetToolTip(result, youtubeImage);
                var yl = new MenuFlyoutItem()
                {
                    Text = "Открыть в браузере"
                };
                yl.Click += YlOnClick(youtubeLink);
                mf.Items?.Add(yl);
            }
        }

        private static RoutedEventHandler YlOnClick(YoutubeLink link)
        {
            return async (sender, e) =>
            {
                try
                {
                    var youtubeUri = ServiceLocator.Current.GetServiceOrThrow<IYoutubeUriService>();
                    var uri = youtubeUri.GetViewUri(link.YoutubeId);
                    await Launcher.LaunchUriAsync(uri);
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            };
        }

        private static RoutedEventHandler MfiOnClick(ITextRenderLinkAttribute linkAttribute)
        {
            return async (sender, e) =>
            {
                try
                {
                    if (linkAttribute.Uri != null && linkAttribute.Uri != "[data]")
                    {
                        var dp = new DataPackage();
                        dp.SetText(linkAttribute.Uri);
                        dp.SetWebLink(new Uri(linkAttribute.Uri, UriKind.Absolute));
                        Clipboard.SetContent(dp);
                        Clipboard.Flush();
                    }
                    else if (linkAttribute.CustomData != null)
                    {
                        var link = linkAttribute.CustomData as BoardLinkBase;
                        if (link != null)
                        {
                            var uri = link.GetWebLink();
                            if (uri != null)
                            {
                                var dp = new DataPackage();
                                dp.SetText(uri.ToString());
                                dp.SetWebLink(uri);
                                Clipboard.SetContent(dp);
                                Clipboard.Flush();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            };
        }
    }
}