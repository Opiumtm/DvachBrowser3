using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace DvachBrowser3
{
    /// <summary>
    /// ������������ ������������� ���������� ������������ ����������.
    /// </summary>
    /// <typeparam name="T">��� ����������.</typeparam>
    public sealed class MaxConcurrencyAccessManager<T> : IConcurrenctyDispatcher<T>
    {
        private readonly AsyncConcurrencySemaphore asyncLock;

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="maxConcurrency">������������ ���������� ������������ �����.</param>
        public MaxConcurrencyAccessManager(int maxConcurrency)
        {
            asyncLock = new AsyncConcurrencySemaphore(maxConcurrency);
        }

        /// <summary>
        /// ������������� ��������.
        /// </summary>
        /// <param name="action">��������.</param>
        /// <returns>����.</returns>
        public async Task<T> QueueAction(Func<Task<T>> action)
        {
            using (await asyncLock.Lock())
            {
                return await action();
            }
        }
    }
}