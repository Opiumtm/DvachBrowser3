using System.Collections.Generic;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис изменения ссылок.
    /// </summary>
    public interface ILinkTransformService
    {
        /// <summary>
        /// Получить ссылку на тред из ссылки на часть треда.
        /// </summary>
        /// <param name="link">Ссылка на часть треда.</param>
        /// <returns>Ссылка на тред.</returns>
        BoardLinkBase ThreadLinkFromThreadPartLink(BoardLinkBase link);

        /// <summary>
        /// Получить ссылку на часть треда из ссылки на тред.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="lastPostLink">Ссылка на последний пост.</param>
        /// <returns>Ссылка на часть треда.</returns>
        BoardLinkBase ThreadPartLinkFromThreadLink(BoardLinkBase threadLink, BoardLinkBase lastPostLink);

        /// <summary>
        /// Получить ссылку на страницу борды.
        /// </summary>
        /// <param name="link">Ссылка на борду или страницу борды.</param>
        /// <returns>Ссылка на страницу борды.</returns>
        BoardLinkBase BoardPageLinkFromBoardLink(BoardLinkBase link);

        /// <summary>
        /// Получить средство сравнения ссылок.
        /// </summary>
        /// <returns>Средство сравнения ссылок.</returns>
        IComparer<BoardLinkBase> GetLinkComparer();

        /// <summary>
        /// Строка для отображения ссылки.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Строка.</returns>
        string GetLinkDisplayString(BoardLinkBase link);

        /// <summary>
        /// Строка для отображения обратной ссылки.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Строка.</returns>
        string GetBackLinkDisplayString(BoardLinkBase link);

        /// <summary>
        /// Строка номера поста.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Строка.</returns>
        string GetPostNumberDisplayString(BoardLinkBase link);

        /// <summary>
        /// Ссылка из этого треда.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        /// <returns>Результат проверки.</returns>
        bool IsThisTread(BoardLinkBase threadLink, BoardLinkBase postLink);

        /// <summary>
        /// Получить ссылку на пост по номеру поста.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postNum">Номер поста.</param>
        /// <returns>Ссылка на пост (null, если это невозможно).</returns>
        BoardLinkBase GetPostLinkByNum(BoardLinkBase threadLink, int postNum);

        /// <summary>
        /// Получить ссылку на пост по ссылке на тред.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <returns>Ссылка на пост (null, если это невозможно).</returns>
        BoardLinkBase GetRootPostLink(BoardLinkBase threadLink);

        /// <summary>
        /// Короткое имя борды.
        /// </summary>
        /// <param name="boardLink">Ссылка на борду.</param>
        /// <returns>Короткое имя.</returns>
        string GetBoardShortName(BoardLinkBase boardLink);
    }
}