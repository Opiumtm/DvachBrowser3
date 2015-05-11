using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Ключ навигации борды.
    /// </summary>
    public sealed class PostNavigationKey : BoardLinkNavigaionKey<PostLink>
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public const string TypeNameConst = "Link-Post";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public PostNavigationKey(PostLink link)
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
            return string.Format("{0}|{1}|{2}|{3}", Link.Engine, Link.Board, Link.Thread, Link.Post);
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ключ навигации.</returns>
        public static PostNavigationKey Deserialize(string data)
        {
            var parts = data.Split('|');
            return new PostNavigationKey(new PostLink() { Engine = parts[0], Board = parts[1], Thread = int.Parse(parts[2]), Post = int.Parse(parts[3])});
        }
    }
}