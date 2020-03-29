using System.Threading.Tasks;
using RepeatingWords.DataService.Model;
using RepeatingWords.Model;
using Xamarin.Forms;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface ICustomContentViewModel
    {
          RepeatingWordsModel Model { get; set; }
          Xamarin.Forms.View WordContainer { get; set; }
          Task ShowNextWord(bool isFirstShowAfterLoad = false);
          Task SetViewWords(Words currentWord, bool isFromNative);
    }
}
