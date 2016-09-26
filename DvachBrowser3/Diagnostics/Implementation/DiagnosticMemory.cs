using System;
using System.Threading.Tasks;
using Windows.System;

namespace DvachBrowser3.Diagnostics
{
    public sealed class DiagnosticMemory : IDiagnosticMemory
    {
        /// <summary>
        /// Общий объём памяти.
        /// </summary>
        public IDiagnosticValue TotalCommitSize { get; } = new TotalCommitSizeValue();

        /// <summary>
        /// Приватно занятой памяти.
        /// </summary>
        public IDiagnosticValue PrivateWorkingSet { get; } = new PrivateWorkingSetValue();

        /// <summary>
        /// Занято управляемой памятью.
        /// </summary>
        public IDiagnosticValue ManagedMemorySet { get; } = new ManagedMemorySetValue();

        /// <summary>
        /// Собрать мусор.
        /// </summary>
        public void GcCollect()
        {
            AppHelpers.DispatchAction(DoGcCollect);
        }

        private async Task DoGcCollect()
        {
            var task = Task.Factory.StartNew(() =>
            {
                GC.Collect(2, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();
                GC.Collect(2, GCCollectionMode.Forced);
            });
            await task;
            Invalidate();
        }

        private void Invalidate()
        {
            ((DiagnosticValueBase)TotalCommitSize).TriggerChange();
            ((DiagnosticValueBase)PrivateWorkingSet).TriggerChange();
            ((DiagnosticValueBase)ManagedMemorySet).TriggerChange();
        }

        private sealed class TotalCommitSizeValue : DiagnosticValueBase
        {
            public override string Name => "Всего памяти занято";
            public override Task<string> QueryAsync()
            {
                var rep = MemoryManager.GetAppMemoryReport();
                double tc = rep.TotalCommitUsage;
                var tcMb = tc/(1024.0*1024.0);
                return Task.FromResult($"{tcMb:F2} Мб");
            }
        }
        private sealed class PrivateWorkingSetValue : DiagnosticValueBase
        {
            public override string Name => "Частный набор";
            public override Task<string> QueryAsync()
            {
                var rep = MemoryManager.GetAppMemoryReport();
                double tc = rep.PrivateCommitUsage;
                var tcMb = tc / (1024.0 * 1024.0);
                return Task.FromResult($"{tcMb:F2} Мб");
            }
        }
        private sealed class ManagedMemorySetValue : DiagnosticValueBase
        {
            public override string Name => "Управляемая куча";
            public override Task<string> QueryAsync()
            {
                var rep = GC.GetTotalMemory(false);
                double tc = rep;
                var tcMb = tc / (1024.0 * 1024.0);
                return Task.FromResult($"{tcMb:F2} Мб");
            }
        }
    }
}