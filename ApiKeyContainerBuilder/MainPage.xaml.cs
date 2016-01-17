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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ApiKeyContainerBuilder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void RandomPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var uuid = Guid.NewGuid();
            PasswordBox.Text = Convert.ToBase64String(uuid.ToByteArray());
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var factory = new KeyContainerFactory(PrivateKeyBox.Text, PublicKeyBox.Text, PasswordBox.Text);
                factory.GenerateContainerString();
                UniqueIdBox.Text = factory.UniqueId;
                ContainerBox.Text = $"{factory.UniqueId}|{PasswordBox.Text}|{factory.ContainerStr}";
            }
            catch (Exception ex)
            {
                var d = new MessageDialog(ex.Message);
                d.Commands.Add(new UICommand("OK"));
                await d.ShowAsync();
            }
        }

        private async void GenerateButton2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var factory = new EncryptedString(StrToEncodeBox.Text, StrPasswordBox.Text);
                StrContainerBox.Text = factory.Encrypt();
            }
            catch (Exception ex)
            {
                var d = new MessageDialog(ex.Message);
                d.Commands.Add(new UICommand("OK"));
                await d.ShowAsync();
            }
        }
    }
}
