namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Стиль браузера.
    /// </summary>
    public interface IBrowserStyle
    {
        /// <summary>
        /// Стиль поста.
        /// </summary>
        IPostTextStyle PostTextStyle { get; } 
    }
}