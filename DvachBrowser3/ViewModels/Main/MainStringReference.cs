namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Ссылка на движок.
    /// </summary>
    public class MainStringReference
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="name">Имя.</param>
        public MainStringReference(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; private set; }
    }
}