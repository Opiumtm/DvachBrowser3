using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;
using DvachBrowser3.StdReplace;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial.Captcha
{
    public sealed partial class DvachCaptchaQueryView : UserControl, ICatpchaQueryView
    {
        public DvachCaptchaQueryView()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
        }

        public event CaptchaQueryResultEventHandler CaptchaQueryResult;

        /// <summary>
        /// Загрузить капчу.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public async void Load(BoardLinkBase link)
        {
            try
            {
                await DoLoad(link);
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        public async void Refresh()
        {
            try
            {
                await DoLoad(lastLink);
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Принять.
        /// </summary>
        public async void Accept()
        {
            try
            {
                var v = (EntryBox.Text ?? "").Trim();
                if (v == string.Empty)
                {
                    var dlg = new MessageDialog("Нужно ввести капчу", "Внимание!");
                    dlg.Commands.Add(new UICommand("Ок"));
                    await dlg.ShowAsync();
                    return;
                }
                CaptchaQueryResult?.Invoke(this, new CaptchaQueryResultEventArgs(new DvachCaptchaPostingData()
                {
                    Key = key,
                    CaptchaEntry = v
                }));
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private BoardLinkBase lastLink;

        private string key;

        private async Task DoLoad(BoardLinkBase link)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            lastLink = link;
            if (!CanLoad)
            {
                return;
            }
            IsLoading = true;
            HasImage = false;
            NotNeeded = false;
            CanLoad = false;
            EntryBox.Text = "";
            try
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var engine = engines.GetEngineById(CoreConstants.Engine.Makaba);
                var captchaKeyOperation = engine.GetCaptchaKeys(link, CaptchaType.DvachCaptcha);
                var keysData = await captchaKeyOperation.Complete();
                if (!keysData.NeedCaptcha)
                {
                    NotNeeded = true;
                    return;
                }
                var keys = keysData.Keys as DvachCaptchaKeys;
                if (keys == null)
                {
                    throw new InvalidOperationException("Неправильный тип ключей капчи");
                }
                var imgLink = new MediaLink() {Engine = CoreConstants.Engine.Makaba, IsAbsolute = false, RelativeUri = $"makaba/captcha.fcgi?type=2chaptcha&action=image&id={WebUtility.HtmlDecode(keys.Key)}"};
                this.key = keys.Key;
                var imgOperation = engine.GetMediaFile(imgLink);
                var img = await imgOperation.Complete();
                using (var str = await img.TempFile.OpenReadAsync())
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(str);
                    Image = bitmap;
                    HasImage = true;
                }
                try
                {
                    await img.TempFile.DeleteAsync();
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
            finally
            {
                IsLoading = false;
                CanLoad = true;
            }
        }

        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            private set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool hasImage;

        public bool HasImage
        {
            get { return hasImage; }
            private set
            {
                hasImage = value;
                OnPropertyChanged();
            }
        }

        private ImageSource image;

        public ImageSource Image
        {
            get { return image; }
            private set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public bool notNeeded;

        public bool NotNeeded
        {
            get { return notNeeded; }
            private set
            {
                notNeeded = value;
                OnPropertyChanged();
            }
        }

        private bool canLoad = true;

        public bool CanLoad
        {
            get { return canLoad; }
            private set
            {
                canLoad = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Accept();
        }
    }
}
