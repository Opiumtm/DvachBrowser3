using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;
using DvachBrowser3.Views.Partial.Captcha;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class CaptchaQueryView : UserControl, ICatpchaQueryView
    {
        public CaptchaQueryView()
        {
            this.InitializeComponent();
            UpdateCaptchaData();
        }

        /// <summary>
        /// Параметр запроса на капчу.
        /// </summary>
        public CaptchaQueryViewParam QueryParam
        {
            get { return (CaptchaQueryViewParam) GetValue(QueryParamProperty); }
            set { SetValue(QueryParamProperty, value); }
        }

        /// <summary>
        /// Параметр запроса на капчу.
        /// </summary>
        public static readonly DependencyProperty QueryParamProperty = DependencyProperty.Register("QueryParam", typeof (CaptchaQueryViewParam), typeof (CaptchaQueryView), new PropertyMetadata(null, QueryParamPropertyChangedCallback));

        private static void QueryParamPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CaptchaQueryView;
            obj?.UpdateCaptchaData();
        }

        /// <summary>
        /// Результат ввода капчи.
        /// </summary>
        public event CaptchaQueryResultEventHandler CaptchaQueryResult;

        /// <summary>
        /// Загрузить капчу.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public void Load(BoardLinkBase link)
        {
            queryView?.Load(link);
        }

        private ICatpchaQueryView queryView;

        private void UpdateCaptchaData()
        {
            CaptchaContainer.Children.Clear();
            if (QueryParam != null)
            {
                if (QueryParam.CaptchaType == CaptchaType.DvachCaptcha && CoreConstants.Engine.Makaba.Equals(QueryParam.Engine, StringComparison.OrdinalIgnoreCase))
                {
                    var qv = new DvachCaptchaQueryView();
                    queryView = qv;
                    CaptchaContainer.Children.Add(qv);
                    qv.CaptchaQueryResult += (sender, e) =>
                    {
                        CaptchaQueryResult?.Invoke(this, e);
                    };
                    UnknownCaptcha.Visibility = Visibility.Collapsed;
                    CaptchaContainer.Visibility = Visibility.Visible;
                    return;
                }
            }
            UnknownCaptcha.Visibility = Visibility.Visible;
            CaptchaContainer.Visibility = Visibility.Collapsed;
        }
    }
}
