using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Configuration
{
    public sealed partial class MakabaBaseConfigPart : UserControl, IUiConfigPart
    {
        public MakabaBaseConfigPart()
        {
            this.InitializeComponent();
            Load();
        }

        private async void Load()
        {
            try
            {
                var engine = MakabaEngine;
                var config = (IMakabaEngineConfig)engine.Configuration;
                EngineBaseUri = config.BaseUri.ToString();
                var isf = true;
                foreach (var d in config.Domains)
                {
                    var db = new MakabaDomainName(d);
                    DomainNames.Add(db);
                    if (isf)
                    {
                        DomainCombo.SelectedItem = db;
                        isf = false;
                    }
                }
                HttpsProtocol = true;
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        public async Task Save()
        {
            var engine = MakabaEngine;
            var config = (IMakabaEngineConfig)engine.Configuration;
            var uri = EngineBaseUri.Trim();
            if (!uri.EndsWith("/"))
            {
                uri = uri + "/";
            }
            config.BaseUri = new Uri(uri, UriKind.Absolute);
            await config.Save();
        }

        private INetworkEngine MakabaEngine => ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>().FindEngine(CoreConstants.Engine.Makaba);

        public static readonly DependencyProperty EngineBaseUriProperty = DependencyProperty.Register(
            "EngineBaseUri", typeof (string), typeof (MakabaBaseConfigPart), new PropertyMetadata(default(string)));

        public string EngineBaseUri
        {
            get { return (string) GetValue(EngineBaseUriProperty); }
            set { SetValue(EngineBaseUriProperty, value); }
        }

        public ObservableCollection<MakabaDomainName> DomainNames { get; } = new ObservableCollection<MakabaDomainName>();

        public static readonly DependencyProperty HttpsProtocolProperty = DependencyProperty.Register(
            "HttpsProtocol", typeof (bool), typeof (MakabaBaseConfigPart), new PropertyMetadata(default(bool)));

        public bool HttpsProtocol
        {
            get { return (bool) GetValue(HttpsProtocolProperty); }
            set { SetValue(HttpsProtocolProperty, value); }
        }

        private void SetUri_OnClick(object sender, RoutedEventArgs e)
        {
            var d = (DomainCombo.SelectedItem as MakabaDomainName)?.Name;
            if (!string.IsNullOrWhiteSpace(d))
            {
                if (HttpsProtocol)
                {
                    EngineBaseUri = $"https://{d.Trim()}/";
                }
                else
                {
                    EngineBaseUri = $"http://{d.Trim()}/";
                }
            }
        }
    }

    public class MakabaDomainName
    {
        public MakabaDomainName(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
