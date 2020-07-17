using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IExport
    {
        Task<JObject> Export();
    }
}
