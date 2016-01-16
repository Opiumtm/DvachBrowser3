using System.Threading.Tasks;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления больших постов.
    /// </summary>
    public interface IBigMediaSourceViewModel : IImageSourceViewModel
    {
        /// <summary>
        /// Сохранить в файл.
        /// </summary>
        Task SaveToFile();

        /// <summary>
        /// Открыть в браузере.
        /// </summary>
        Task OpenInBrowser();
    }
}