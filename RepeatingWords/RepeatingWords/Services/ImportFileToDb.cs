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
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class ImportFileToDb : IImportFile
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImportFileToDb(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> PickFile(int dictionaryId)
        {
            try
            {
                string[] typeFiles = null;
                if (Device.RuntimePlatform == Device.Android)
                    typeFiles = new[] { "text/plain" };
                if (Device.RuntimePlatform == Device.iOS)
                    typeFiles = new[] { "public.text" };
                bool success = false;
                using (var filePiker = await CrossFilePicker.Current.PickFile(typeFiles))
                {
                    if (!string.IsNullOrEmpty(filePiker.FileName))
                        success = await StartImport(filePiker.DataArray, filePiker.FileName, dictionaryId);
                    else
                        success = true;
                }
                return success;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        public async Task<bool> StartImport(byte[] data, string file, int dictionaryId)
        {
            try
            {
                var stringData = Encoding.UTF8.GetString(data, 0, data.Length);
                List<string> lines = stringData.Split('\n').ToList();
                //проходим по списку строк считанных из файла
                if (lines != null && lines.Count > 0 && file.EndsWith(".txt"))
                    return await ParseLineAndCreateImportedWords(dictionaryId, lines);
                return false;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        private Task<bool> ParseLineAndCreateImportedWords(int dictionaryId, List<string> lines)
        {
            bool isImportSuccessed = false;
            char[] delim = {'[', ']'};
            for (int i = 0; i < lines.Count; i++)
            {
                if(ValidatorInputLine(lines[i]))
                    isImportSuccessed = InsertWordFromFileToDb(dictionaryId, lines[i].Split(delim));
                else if(i < lines.Count - 1)
                {
                    _unitOfWork.Save();
                    return Task.FromResult(false);
                }
            }
            _unitOfWork.Save();
            return Task.FromResult(isImportSuccessed);
        }

        private bool InsertWordFromFileToDb(int dictionaryId, string[] fileWords)
        {
            var badSymbals = new char[] { ' ', '\n', '\t', '\r' };
            if (fileWords.Count() == 3)
            {
                Words item = new Words
                {
                    Id = 0,
                    IdDictionary = dictionaryId,
                    RusWord = fileWords[0].Trim(badSymbals),
                    Transcription = "[" + fileWords[1].Trim(badSymbals) + "]",
                    EngWord = fileWords[2].Trim(badSymbals),
                    IsLearned = false
                };
                _unitOfWork.WordsRepository.Create(item);
                return true;
            }
            return false;
        }

        private bool ValidatorInputLine(string inputLine)
        {
            bool isValid = inputLine.Contains("[") && inputLine.Contains("]");
            return isValid;
        }
    }
}
