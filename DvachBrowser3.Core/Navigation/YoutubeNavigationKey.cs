using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class YoutubeNavigationKey : BoardLinkNavigaionKey<YoutubeLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-Youtube";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public YoutubeNavigationKey(YoutubeLink link)
            : base(link)
        {
        }

        /// <summary>
        /// Имя типа ключа.
        /// </summary>
        public override string TypeName
        {
            get { return TypeNameConst; }
        }

        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <returns>Строка.</returns>
        public override string Serialize()
        {
            return string.Format("{0}|{1}", Link.Engine, Link.YoutubeId);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static YoutubeNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new YoutubeNavigationKey(new YoutubeLink() { Engine = parts[0], YoutubeId = parts[1] });
        }
    }
}