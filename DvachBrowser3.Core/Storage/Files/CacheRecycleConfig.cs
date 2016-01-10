using System;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// ������������ ������� ����.
    /// </summary>
    public struct CacheRecycleConfig
    {
        /// <summary>
        /// ������������ ��������.
        /// </summary>
        public static readonly CacheRecycleConfig MaxValue = new CacheRecycleConfig()
        {
            MaxSize = ulong.MaxValue,
            NormalSize = ulong.MaxValue,
            MaxFiles = int.MaxValue,
            NormalFiles = int.MaxValue
        };

        /// <summary>
        /// ������������ ������.
        /// </summary>
        public ulong MaxSize;

        /// <summary>
        /// ���������� ������.
        /// </summary>
        public ulong NormalSize;

        /// <summary>
        /// ������������ ���������� ������.
        /// </summary>
        public int MaxFiles;

        /// <summary>
        /// ���������� ���������� ������.
        /// </summary>
        public int NormalFiles;

    }
}