using System;

namespace DvachBrowser3.Lifecycle
{
    /// <summary>
    /// Ошибка менеджера сохранения состояния.
    /// </summary>
    public class SuspensionManagerException : Exception
    {
        public SuspensionManagerException()
        {
        }

        public SuspensionManagerException(Exception e)
            : base("SuspensionManager failed", e)
        {

        }
    }
}