using System.Threading.Tasks;
using RepeatingWords.Model;
 

namespace RepeatingWords.Helpers.Interfaces
{
    public interface ICustomContentViewModel
    {
          RepeatingWordsModel Model { get; set; }
          Xamarin.Forms.View WordContainer { get; set; }
          Task ShowNextWord(bool isFirstShowAfterLoad = false);
          Task SetViewWords(WordsModel currentWord, bool isFromNative);
    }
}
