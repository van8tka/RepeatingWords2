using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IImport
    {
        Task<bool> import(JObject jobject);
    }
}
