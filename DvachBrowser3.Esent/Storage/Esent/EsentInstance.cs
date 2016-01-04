using System;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// ���������.
    /// </summary>
    internal sealed class EsentInstance : IEsentInstance
    {
        /// <summary>
        /// ��������� ������������ ����������� ������, ��������� � ���������, �������������� ��� ������� ������������� ��������.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Object"/>.
        /// </summary>
        public EsentInstance(Action onDispose, Instance instance)
        {
            OnDispose = onDispose;
            Instance = instance;
        }

        /// <summary>
        /// ���������.
        /// </summary>
        public Instance Instance { get; }

        /// <summary>
        /// �������� �� ����������.
        /// </summary>
        private Action OnDispose;
    }
}