using System;
using System.IO;
using System.Threading.Tasks;

namespace RepeatingWords.Interfaces
{
    public interface IImportFile
    {
        Task<bool> PickFile(int dictionaryId);
        Task<bool> StartImport(byte[] data,string file, int dictionaryId);
    }
}
