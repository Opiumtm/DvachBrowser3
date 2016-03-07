using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Враппер модели представления.
    /// </summary>
    /// <typeparam name="THost">Хост.</typeparam>
    /// <typeparam name="TViewModel">Модель представления.</typeparam>
    public class ViewModelWrapper<THost, TViewModel> : INotifyPropertyChanged where THost : FrameworkElement, IViewModelHost<TViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private WeakReference<THost> hostHandle;

        public THost Host
        {
            get
            {
                if (hostHandle == null)
                {
                    return null;
                }
                THost host;
                if (hostHandle.TryGetTarget(out host))
                {
                    return host;
                }
                return null;
            }
            set
            {
                THost oldHost;
                if (hostHandle == null)
                {
                    oldHost = null;
                }
                else if (!hostHandle.TryGetTarget(out oldHost))
                {
                    oldHost = null;
                }
                if (value != null)
                {
                    value.ViewModelChanged += CreateEventHandler(new WeakReference<ViewModelWrapper<THost, TViewModel>>(this));
                }
                if (oldHost != null)
                {
                    oldHost.ViewModelChanged -= CreateEventHandler(new WeakReference<ViewModelWrapper<THost, TViewModel>>(this));
                }
                if (value == null)
                {
                    hostHandle = null;
                }
                else
                {
                    hostHandle = new WeakReference<THost>(value);
                }
                OnPropertyChanged(nameof(Host));
                OnPropertyChanged(nameof(ViewModel));
            }

        }

        private static EventHandler CreateEventHandler(WeakReference<ViewModelWrapper<THost, TViewModel>> handle)
        {
            return (sender, e) =>
            {
                ViewModelWrapper<THost, TViewModel> obj;
                if (handle.TryGetTarget(out obj))
                {
                    obj.OnPropertyChanged(nameof(ViewModel));
                }
            };
        }

        public TViewModel ViewModel
        {
            get
            {
                var host = Host;
                return host != null ? host.GetHostedViewModel() : default(TViewModel);
            }
        }
    }

    /// <summary>
    /// Хост модели представления.
    /// </summary>
    /// <typeparam name="TViewModel">Модель.</typeparam>
    public interface IViewModelHost<out TViewModel>
    {
        /// <summary>
        /// Получить модель.
        /// </summary>
        /// <returns>Модель.</returns>
        TViewModel GetHostedViewModel();

        /// <summary>
        /// Модель представления изменилась.
        /// </summary>
        event EventHandler ViewModelChanged;
    }
}