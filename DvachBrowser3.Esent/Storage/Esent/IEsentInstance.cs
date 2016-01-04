using System;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    internal interface IEsentInstance : IDisposable
    {
        /// <summary>
        /// Экземпляр.
        /// </summary>
        Instance Instance { get; }
    }
}