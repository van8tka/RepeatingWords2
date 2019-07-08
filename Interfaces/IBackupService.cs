using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IBackupService
    {
        Task<bool> CreateBackup(string file);
        Task<bool> RestoreBackup(string file);
    }
}
