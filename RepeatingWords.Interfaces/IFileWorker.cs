using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface IFileWorker
    {
        string CreateFolder(string folderName, string fileName = null, string filePath = null);// создание папки 

        bool WriteFile(string filePathsource, string filepathDest);//запись файла

        Task<string> GetBackUpFilesAsync(string folder);//получение файла бэкапа из опредго каталога
    }
}
