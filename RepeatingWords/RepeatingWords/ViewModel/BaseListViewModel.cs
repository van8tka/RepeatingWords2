using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    /// <summary>
    /// abstract class using only for DictionaryListViewModel and WordsListViewModel
    /// it contains properties and method for manage floating data
    /// </summary>
    public abstract class BaseListViewModel : ViewModelBase
    {
        //ctor
        protected BaseListViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IImportFile importFile) : base(navigationServcie, dialogService)
        {
            _unitOfWork = unitOfWork;
            _importFile = importFile ?? throw new ArgumentNullException(nameof(_importFile));
            MenuCommand = new Command(async () => { await ChangeVisibleMenuButtons(); });
            ImportWordsCommand = new Command(async () => { await ImportFile(); SetUnVisibleFloatingMenu(); });
        }

        public ICommand ImportWordsCommand { get; set; }
        public ICommand MenuCommand { get; set; }

        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IImportFile _importFile;
        private readonly string menyActive = "floating_btn_menu.png";
        private readonly string menuUnActive = "floating_btn_menuGray.png";

        protected abstract Task ImportFile();

        private string _sourceMenuBtn;
        public string SourceMenuBtn { get => _sourceMenuBtn; set { _sourceMenuBtn = value; OnPropertyChanged(nameof(SourceMenuBtn)); } }
        private bool _learnVisible;
        public bool LearnVisible { get => _learnVisible; set { _learnVisible = value; OnPropertyChanged(nameof(LearnVisible)); } }
        private bool _importVisible;
        public bool ImportVisible { get => _importVisible; set { _importVisible = value; OnPropertyChanged(nameof(ImportVisible)); } }
        private bool _addVisible;
        public bool AddVisible { get => _addVisible; set { _addVisible = value; OnPropertyChanged(nameof(AddVisible)); } }

        protected void SetUnVisibleFloatingMenu()
        {
            LearnVisible = false;
            ImportVisible = false;
            AddVisible = false;
            SourceMenuBtn = menyActive;
        }

        protected async Task ChangeVisibleMenuButtons()
        {
            await Task.Delay(350);
            LearnVisible = !LearnVisible;
            ImportVisible = !ImportVisible;
            AddVisible = !AddVisible;
            SourceMenuBtn = AddVisible ? menuUnActive : menyActive;
        }
    }
}
