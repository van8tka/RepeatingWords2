﻿using RepeatingWords.Model;

namespace RepeatingWords.Heleprs
{
    internal class Constants
    {
        internal const string DATABASE_NAME = "repeatwords.db";
        internal const string LOCAL_FOLDER_BACKUP = "CardsOfWordsBackup";

        internal const string THEME = "theme";
        internal const string THEME_WHITE = "white";
        internal const string THEME_DARK = "black";

        internal const string KEYBOARD_TRANSCRIPTION = "TrKeyboard";
        internal const string KEYBOARD_TRANSCRIPTION_SHOWED = "showed";
        internal const string KEYBOARD_TRANSCRIPTION_HIDE = "hide";
        internal const string VOLUME_LANGUAGE = "volume";
        internal static VolumeLanguageModel VOLUME_LANGUAGE_DEFAULT = new VolumeLanguageModel() { Name = "English(GB)", CountryCode = "GB", LanguageCode = "en", IsChecked = true };
        
        internal const string NAME_DB_FOR_CONTINUE = "ContinueDictionary";
       
        internal const int CHECK_AVAILABLE_COUNT = 3;

        internal const string LANGUAGE_FIRST = "language_first";
        internal const string LANGUAGE_NATIVE = "native";
        internal const string LANGUAGE_FOREIGN = "foreign";
    }
}
