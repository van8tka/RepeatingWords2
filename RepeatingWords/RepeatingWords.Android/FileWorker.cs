using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using System.Linq;
using RepeatingWords.LoggerService;

[assembly: Dependency(typeof(RepeatingWords.Droid.FileWorker))]

namespace RepeatingWords.Droid
{
    public class FileWorker : IFileWorker
    {

        //�������� ����� ��� 
        public string CreateFolder(string folderName, string fileName = null, string filePath = null)
        {
            try
            {//���� �� ���������� ���� � ����� ��� ������� ��������� �����
                if (string.IsNullOrEmpty(filePath))
                {
                    //�� ������� �� ���������
                    filePath = FilePathToBackupFolder(folderName);
                }
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                else if (string.IsNullOrEmpty(fileName))
                        return "exist";
                //���� ���� � �����
                if (!string.IsNullOrEmpty(fileName))
                {
                    string pathToFile = Path.Combine(filePath, fileName);
                    return pathToFile;
                }
                return filePath;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        private static string FilePathToBackupFolder(string folderName)
        {
            string filePath;
            filePath = MainActivity.Instance.ApplicationContext.GetExternalFilesDirs(null).FirstOrDefault()?.Parent;
            int ind = filePath.LastIndexOf('/');
            filePath = filePath.Remove(ind);
            filePath = Path.Combine(filePath, folderName);
            return filePath;
        }

        //��������� ������ ������ ������
        public Task<string> GetBackUpFilesAsync(string folder)
        {
            try
            {
                return Task.Run(() =>
                {
                    string pathToDir = FilePathToBackupFolder(folder);
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
                      throw new ArgumentNullException(nameof(pathToDir));
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