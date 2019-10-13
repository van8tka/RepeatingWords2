using System.Threading.Tasks;

namespace RepeatingWords.Interfaces
{
    public interface IImportFile
    {
        Task<bool> StartImport(string filePath, int dictionaryId);
    }
}
