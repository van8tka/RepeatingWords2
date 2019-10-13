using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface IFolderWorker
    {
        string GetRootPath();
        Task<List<string>> GetFoldersAsync(string path = null);
    }
}
