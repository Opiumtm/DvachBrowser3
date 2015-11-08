using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Информация о постинге.
    /// </summary>
    public interface IPostingBoardInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// Возможности.
        /// </summary>
        IList<IBoardInfoPostingCapability> Capabilities { get; }
    }
}