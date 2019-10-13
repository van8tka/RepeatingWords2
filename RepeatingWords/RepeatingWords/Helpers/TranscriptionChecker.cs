namespace RepeatingWords.Helpers
{
    internal class TranscriptionChecker
    {
        internal static bool CheckIsNotEmptyTranscription(string transcription)
        {
            if (string.IsNullOrEmpty(transcription))
                return false;
            string temp = transcription.Replace("[", string.Empty).Replace("]", string.Empty);

            if (string.IsNullOrEmpty(temp) || string.IsNullOrWhiteSpace(temp))
                return false;
            return true;
        }
    }
}
