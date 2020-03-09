using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class ImportFileToDb : IImportFile
    {
        private readonly IDictionaryStudyService _studyService;

        public ImportFileToDb(IDictionaryStudyService studyService)
        {
            _studyService = studyService;
        }


        public Task<FileData> PickFile()
        {
            try
            {
                string[] typeFiles = null;
                if (Device.RuntimePlatform == Device.Android)
                    typeFiles = new[] { "text/plain" };
                if (Device.RuntimePlatform == Device.iOS)
                    typeFiles = new[] { "public.text" };
                return CrossFilePicker.Current.PickFile(typeFiles);
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        public Task<bool> StartImport(byte[] data, string file, int dictionaryId)
        {
            try
            {
                var stringData = Encoding.UTF8.GetString(data, 0, data.Length);
                List<string> lines = stringData.Split('\n').ToList();
                //проходим по списку строк считанных из файла
                if (lines != null && lines.Count > 0 && file.EndsWith(".txt"))
                    return ParseLineAndCreateImportedWords(dictionaryId, lines);
                return Task.FromResult<bool>(false);
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        private Task<bool> ParseLineAndCreateImportedWords(int dictionaryId, List<string> lines)
        {
            char[] delim = {'[', ']'};
            for (int i = 0; i < lines.Count; i++)
            {
                if(ValidatorInputLine(lines[i])) 
                     InsertWordFromFileToDb(dictionaryId, lines[i].Split(delim));
                else if(i < lines.Count - 1)
                    return Task.FromResult<bool>(false);
            }
            return Task.FromResult<bool>(true);
        }

        private Task<bool> InsertWordFromFileToDb(int dictionaryId, string[] fileWords)
        {
            var badSymbals = new char[] { ' ', '\n', '\t', '\r' };
            if (fileWords.Count() == 3)
            {
                Words item =new Words
                    {
                        Id = 0,
                        IdDictionary = dictionaryId,
                        RusWord = fileWords[0].Trim(badSymbals),
                        Transcription = "[" + fileWords[1].Trim(badSymbals) + "]",
                        EngWord = fileWords[2].Trim(badSymbals),
                        IsLearned = false
                    };
                _studyService.AddWord(item);
                return Task.FromResult(true);

            }
            return Task.FromResult(false);
        }

        private bool ValidatorInputLine(string inputLine)
        {
            bool isValid = inputLine.Contains("[") && inputLine.Contains("]");
            return isValid;
        }
    }
}
