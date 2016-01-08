using System;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Динамический источник строки команд.
    /// </summary>
    public interface IDynamicShellAppBarProvider : IShellAppBarProvider
    {
        /// <summary>
        /// Изменить строку команд.
        /// </summary>
        event EventHandler AppBarChange;
    }
}