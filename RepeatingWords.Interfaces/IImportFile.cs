using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.FilePicker.Abstractions;

namespace RepeatingWords.Interfaces
{
    public interface IImportFile
    {
        Task<FileData> PickFile();
        Task<bool> StartImport(byte[] data,string file, int dictionaryId);
    }
}
