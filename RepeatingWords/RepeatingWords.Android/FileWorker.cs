using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using RepeatingWords.LoggerService;

[assembly: Dependency(typeof(RepeatingWords.Droid.FileWorker))]

namespace RepeatingWords.Droid
{
    public class FileWorker : IFileWorker
    {

        //создание папки для 
        public string CreateFolder(string folderName, string fileName = null, string filePath = null)
        {
            try
            {//если не передается путь к папке где создать резервную копию
                if (string.IsNullOrEmpty(filePath))
                {//то создаем по умолчанию
                    filePath = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
                //созд путь к папке
                string pathToDir = Path.Combine(filePath, folderName);
                if (!Directory.Exists(pathToDir))
                    Directory.CreateDirectory(pathToDir);
                else if (string.IsNullOrEmpty(fileName))
                        return "exist";
                //созд путь к файлу
                if (!string.IsNullOrEmpty(fileName))
                {
                    string pathToFile = Path.Combine(pathToDir, fileName);
                    return pathToFile;
                }
                else
                    return pathToDir;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        //запись файла резервной копии
        public bool WriteFile(string filePathSource, string filePathDestin)
        {
            try
            {
                File.Copy(filePathSource, filePathDestin, true);
                return true;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        //получение списка файлов бэкапа
        public Task<string> GetBackUpFilesAsync(string folder)
        {
            try
            {
                return Task.Run(() =>
                  {
                      string path = Android.OS.Environment.ExternalStorageDirectory.ToString();
                      string pathToDir = Path.Combine(path, folder);
                      if (Directory.Exists(pathToDir))
                      {
                          var list = Directory.GetFiles(pathToDir);
                          string lastFile = string.Empty;
                          DateTime tempDateTime = DateTime.MinValue;
                          foreach (var i in list)
                          {
                              var fI = new FileInfo(i);
                              var DateTimeCreate = fI.LastWriteTime;
                              if (DateTimeCreate > tempDateTime)
                              {
                                  tempDateTime = DateTimeCreate;
                                  lastFile = i;
                              }
                          }
                          return lastFile;
                      }
                      else
                          return null;
                  });
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }


    }

}