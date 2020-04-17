namespace RepeatingWords.Helpers.Interfaces
{
    public interface IVolumeLanguageService
    {
        /// <summary>
        /// получение имени языка установленного для озвучки
        /// </summary>     
        string GetVolumeLanguage();
      /// <summary>
        /// установка имени языка для озвучки и сохранение в настройках устройства
        /// </summary>  
        bool SetVolumeLanguage(string languageName);
    }
}
