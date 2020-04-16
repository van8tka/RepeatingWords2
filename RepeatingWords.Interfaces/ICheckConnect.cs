using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface ICheckConnect
    {
       Task<bool> CheckTheNet();
    }
}
