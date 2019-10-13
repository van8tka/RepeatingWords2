
namespace RepeatingWords.Helpers.Interfaces
{
    public interface IThemeService
    {
        /// <summary>
        /// return true is dark them
        /// </summary>
        /// <returns></returns>
        bool GetCurrentTheme();
        /// <summary>
        /// return true is dark them
        /// </summary>
        /// <returns></returns>
        bool ChangeTheme();
    }
}
