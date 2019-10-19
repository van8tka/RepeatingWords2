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

        //�������� ����� ��� 
        public string CreateFolder(string folderName, string fileName = null, string filePath = null)
        {
            try
            {//���� �� ���������� ���� � ����� ��� ������� ��������� �����
                if (string.IsNullOrEmpty(filePath))
                {//�� ������� �� ���������
                    filePath = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
                //���� ���� � �����
                string pathToDir = Path.Combine(filePath, folderName);
                if (!Directory.Exists(pathToDir))
                    Directory.CreateDirectory(pathToDir);
                else if (string.IsNullOrEmpty(fileName))
                        return "exist";
                //���� ���� � �����
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

        //������ ����� ��������� �����
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

        //��������� ������ ������ ������
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