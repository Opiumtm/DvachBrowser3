using System.ComponentModel;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    public interface IPostQuoteViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// ������������ ������.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// ���.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ������.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// �������� ������.
        /// </summary>
        IStyleManager StyleManager { get; }
    }
}