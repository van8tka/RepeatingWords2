using RepeatingWords.Model;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IVolumeLanguageService
    {
        /// <summary>
        /// получение имени языка установленного для озвучки
        /// </summary>     
        VolumeLanguageModel GetVolumeLanguage();
        /// <summary>
        /// получение аббревиатуры языка установленного для озвучки
        /// </summary>     
      //  string GetSysAbbreviationVolumeLanguage();
        /// <summary>
        /// установка имени языка для озвучки и сохранение в настройках устройства
        /// </summary>  
        bool SetVolumeLanguage(VolumeLanguageModel languageName);
    }
}
