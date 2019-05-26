using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;

[assembly:Dependency(typeof(RepeatingWords.Droid.FolderWorker))]

namespace RepeatingWords.Droid
{
    public class FolderWorker : IFolderWorker
    {

        public Task<List<string>> GetFoldersAsync(string path = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!MainActivity.HasPermissionToReadWriteExternalStorage)
                        throw new UnauthorizedAccessException();
                    var dir = new List<string>();
                    string rootpath;
                    if (string.IsNullOrEmpty(path))
                    {
                        rootpath = GetRootPath();
                    }
                    else
                    {
                        rootpath = path;
                    }

                    dir = Directory.EnumerateDirectories(rootpath).ToList();
                 
                    var tempList = new List<string>();
                    foreach (var i in dir)
                    {
                        var s = i.Split(new string[] { rootpath+"/" }, StringSplitOptions.None);
                        if (s.Any() && s.Count() > 1)
                            tempList.Add(s[1]);
                    }
                    return tempList;
                }
                catch(UnauthorizedAccessException er)
                {
                    ErrorHandlerCustom.getErrorMessage(er);
                    throw new UnauthorizedAccessException();
                }
                 catch(Exception er)
                {
                    ErrorHandlerCustom.getErrorMessage(er);
                    return null;
                }
              
            });
          
        }

        public string GetRootPath()
        {
            try
            {
                if (!MainActivity.HasPermissionToReadWriteExternalStorage)
                    throw new UnauthorizedAccessException();
                var state = Android.OS.Environment.ExternalStorageState;
                return Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            catch (UnauthorizedAccessException er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
                throw new UnauthorizedAccessException();
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
                return null;
            }          
        }
    }
}