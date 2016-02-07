namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// ������ �� ������.
    /// </summary>
    public class MainStringReference
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="id">�������������.</param>
        /// <param name="name">���.</param>
        public MainStringReference(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// �������������.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// ���.
        /// </summary>
        public string Name { get; private set; }
    }
}