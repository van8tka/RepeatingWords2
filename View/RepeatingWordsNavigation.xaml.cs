using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepeatingWordsNavigation : NavigationPage
    {
        public RepeatingWordsNavigation()
        {
            InitializeComponent();
        }
        public RepeatingWordsNavigation(Page root):base(root)
        {
            InitializeComponent();
        }
    }
}