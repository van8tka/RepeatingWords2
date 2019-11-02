using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using RepeatingWords.Model;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class VolumeLanguageService : IVolumeLanguageService
    {
     
        public VolumeLanguageModel GetVolumeLanguage()
        {
            object volumeLanguage;
            if (App.Current.Properties.TryGetValue(Constants.VOLUME_LANGUAGE, out volumeLanguage))
            {
                if(volumeLanguage is VolumeLanguageModel languageSpeaker)
                    return languageSpeaker;
            }
            return Constants.VOLUME_LANGUAGE_DEFAULT;
        }

        public bool SetVolumeLanguage(VolumeLanguageModel languageSpeaker)
        {
            try
            {
                if (App.Current.Properties.ContainsKey(Constants.VOLUME_LANGUAGE))
                {
                    App.Current.Properties.Remove(Constants.VOLUME_LANGUAGE);
                }
                App.Current.Properties.Add(Constants.VOLUME_LANGUAGE, languageSpeaker);
                return true;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }


        //public string GetSysAbbreviationVolumeLanguage()
        //{
        //    try
        //    {
        //        var languageName = GetVolumeLanguage();
        //        string languageAbbreviation = string.Empty;
        //        switch (languageName)
        //        {
        //            case "English":
        //                {

        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "en_GB";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "en-GB";
        //                    }

        //                    break;
        //                }
        //            case "French":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "fr_FR";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "fr-FR";
        //                    }
        //                    break;
        //                }
        //            case "German":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "de_DE";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "de-DE";
        //                    }

        //                    break;
        //                }
        //            case "Polish":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "pl_PL";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "pl-PL";
        //                    }

        //                    break;
        //                }
        //            case "Ukrainian":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "uk_UK";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "uk-UK";
        //                    }

        //                    break;
        //                }
        //            case "Italian":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "it_IT";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "it-IT";
        //                    }

        //                    break;
        //                }
        //            case "Русский":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "ru_RU";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "ru-RU";
        //                    }

        //                    break;
        //                }
        //            case "Chinese":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "zh_CN";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "zh-CN";
        //                    }

        //                    break;
        //                }
        //            case "Japanese":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "ja_JP";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "ja-JP";
        //                    }

        //                    break;
        //                }
        //            case "Portuguese(Brazil)":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "pt_BR";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "pt-BR";
        //                    }

        //                    break;
        //                }
        //            case "Spanish":
        //                {
        //                    if (Device.RuntimePlatform == Device.Android)
        //                    {
        //                        languageAbbreviation = "es_ES";
        //                    }
        //                    else if (Device.RuntimePlatform == Device.UWP)
        //                    {
        //                        languageAbbreviation = "es-ES";
        //                    }
        //                    break;
        //                }
        //            case "Turkish":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    languageAbbreviation = "tr_TR";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    languageAbbreviation = "tr-TR";
        //                }
        //                break;
        //            }
        //            default:
        //               throw new ArgumentException(nameof(languageName));
        //        }
        //        return languageAbbreviation;
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error(e);
        //        throw;
        //    }
        //}

    }
}
