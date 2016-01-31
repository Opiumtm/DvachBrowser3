using DvachBrowser3.Links;

namespace DvachBrowser3
{
    /// <summary>
    /// Расширенный параметр страницы.
    /// </summary>
    public class ExtendedPageParam
    {
        /// <summary>
        /// Параметр ссылки.
        /// </summary>
        public string LinkParam { get; set; }         

        /// <summary>
        /// Идентификатор дополнительных данных.
        /// </summary>
        public string CustomDataId { get; set; }
    }

    /// <summary>
    /// Расширенный параметр страницы.
    /// </summary>
    public class ExtendedPageParam2
    {
        /// <summary>
        /// Параметр ссылки.
        /// </summary>
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Идентификатор дополнительных данных.
        /// </summary>
        public string CustomDataId { get; set; }
    }
}