using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<bool> StartImport(string filePath, int dictionaryId)
        {       
            try
            {              
                bool isImportSuccessed = false;
                List<string> lines = await DependencyService.Get<IFileWorker>().LoadTextAsync(filePath);
                //проходим по списку строк считанных из файла
                if (lines != null && lines.Count > 0 && filePath.EndsWith(".txt"))
                {
                    char[] delim = { '[', ']' };
                    //переменная для проверки добавления слов
                   
                    //проход по списку слов
                    for(int i=0;i<lines.Count;i++)
                    {//проверка на наличие разделителей, т.е. транскрипции в строке(символы транскрипции и есть разделители)
                        if (lines[i].Contains("[") && lines[i].Contains("]"))
                        {
                            var badSymbals = new char[] { ' ', '\n', '\t', '\r' };
                            isImportSuccessed = true;
                            string[] fileWords = lines[i].Split(delim);
                            Words item = new Words
                            {
                                Id = 0,
                                IdDictionary = dictionaryId,
                                RusWord = fileWords[0].Trim(badSymbals),
                                Transcription = "[" + fileWords[1].Trim(badSymbals) + "]",
                                EngWord = fileWords[2].Trim(badSymbals)
                            };
                            _unitOfWork.WordsRepository.Create(item);
                        }
                    }
                    _unitOfWork.Save();                    
                }
                return isImportSuccessed;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

    }
}
