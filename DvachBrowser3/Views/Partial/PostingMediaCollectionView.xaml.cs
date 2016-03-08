using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostingMediaCollectionView : UserControl
    {
        public PostingMediaCollectionView()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = DataContext;
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostingMediaCollectionViewModel ViewModel
        {
            get { return (IPostingMediaCollectionViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostingMediaCollectionViewModel), typeof (PostingMediaCollectionView), new PropertyMetadata(null));

        private readonly Lazy<IStyleManager> styleManager = new Lazy<IStyleManager>(() => StyleManagerFactory.Current.GetManager());

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => styleManager.Value;

        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag = (sender as FrameworkElement)?.Tag as IPostingMediaViewModel;
                if (tag != null)
                {
                    bool isDelete = false;
                    var dialog = new MessageDialog("Удалить медиафайл?", "Внимание!")
                    {
                        Commands =
                    {
                        new UICommand("Да", command =>
                        {
                            isDelete = true;
                        }),
                        new UICommand("Нет")
                    },
                        CancelCommandIndex = 1,
                        DefaultCommandIndex = 1
                    };
                    await dialog.ShowAsync();
                    if (isDelete)
                    {
                        await tag.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel?.CanAdd ?? false)
                {
                    await ViewModel.ChooseMediaFile();
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }
    }
}
