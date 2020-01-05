using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface ITextToSpeech
    {
        Task Speak(string text);
        string Language { get; }
    }
}
