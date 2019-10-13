using System.Windows.Input;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IMainPage
    {
        ICommand BegianLearningCommand { get; set; }
        ICommand ChooseDictionaryCommand { get; set; }
        ICommand ContinueLearningCommand { get; set; }
        ICommand ShowToolsCommand { get; set; }
        ICommand LikeCommand { get; set; }
        ICommand HelperCommand { get; set; }
    }
}
