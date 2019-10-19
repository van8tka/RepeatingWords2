using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class ChooseFileViewModel : ViewModelBase
    {
        private readonly IImportFile _importFile;

        public ChooseFileViewModel(INavigationService navigationServcie, IDialogService dialogService, IImportFile importFile ) : base(navigationServcie, dialogService)
        {
            _importFile = importFile;
        }

        private Dictionary _dictionary;
        private string _rootPath;
        private string _defaultRootPath;

        private string _currentPath;
        public string CurrentPath { get => _currentPath; set { _currentPath = value; OnPropertyChanged(CurrentPath); } }

        private string _selectedItem;
        public string SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); if (_selectedItem != null) SelectedAction(_selectedItem); } }

        private ObservableCollection<string> _fileList;
        public ObservableCollection<string> FileList { get => _fileList; set { _fileList = value; OnPropertyChanged(nameof(FileList)); } }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Dictionary dictionary)
            {
                _dictionary = dictionary;
                await GetFolderList();
            }               
            await base.InitializeAsync(navigationData);
        }

        private async Task GetFolderList()
        {
            try
            {
                _defaultRootPath = DependencyService.Get<IFolderWorker>().GetRootPath();
                _rootPath = _defaultRootPath; 
                await UpdateFileList(_rootPath);
                CurrentPath = Resource.LabelPathToRoot + " " + _rootPath;
                OnPropertyChanged(nameof(CurrentPath));
            }      
            catch (Exception er)
            {
                Log.Logger.Error(er);
            }
        }

       

        private async void SelectedAction(string selectedItem)
        {
            try
            {
                SelectedItem = null;
                if (selectedItem.Equals("..", StringComparison.OrdinalIgnoreCase))
                    await OnBackPath(_rootPath);
                else
                   await GotoThePath(selectedItem);
               
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);              
            }
        }

        private async Task GotoThePath(string selectedItem)
        {
            try
            {
                var varPath = Path.Combine(_rootPath, selectedItem);
                //если выбрана директория
                if (!DependencyService.Get<IFileWorker>().IsFile(varPath))
                {
                    _rootPath = Path.Combine(_rootPath, selectedItem);
                    CurrentPath = Path.Combine(CurrentPath, selectedItem);
                    OnPropertyChanged(nameof(CurrentPath));
                    await UpdateFileList(_rootPath);
                }
                else//если файл 
                {
         //           bool success = await _importFile.StartImport(varPath, _dictionary.Id);
                    //if (success)
                    //    await GoToWordsList();
                    //else
                    //    await DialogService.ShowAlertDialog(Resource.ModalException, Resource.Continue, Resource.ModalAddWords);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);              
            }
        }

        private async Task GoToWordsList()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
            await NavigationService.NavigateToAsync<WordsListViewModel>(_dictionary);
            await NavigationService.RemoveLastFromBackStackAsync();
        }


        public async Task<bool> UpdateFileList(string folderPath = null)
        {
            try
            {
                var _collectionFile = new ObservableCollection<string>();
                string backItem = "..";
                //получим список файлов или папок                                
                var folderList = await DependencyService.Get<IFolderWorker>().GetFoldersAsync(folderPath);
                    var fileList = await DependencyService.Get<IFileWorker>().GetFilesAsync(folderPath);
                    FileList = await Task.Run(() =>
                    {
                        if(fileList.Any())
                            folderList.AddRange(fileList);
                        if(!folderPath.Equals(_defaultRootPath, StringComparison.OrdinalIgnoreCase))
                            folderList.Insert(0, backItem);
                        for (int i = 0; i < folderList.Count(); i++)
                            _collectionFile.Add(folderList.ElementAt(i));
                        return _collectionFile;
                    });
                return true;         
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                return false;
            }
        }


       
        private async Task OnBackPath(string path)
        {                                             
            _rootPath = path.Remove(path.LastIndexOf('/'));
            CurrentPath = CurrentPath.Remove(CurrentPath.LastIndexOf('/'));
            OnPropertyChanged(nameof(CurrentPath));
            await UpdateFileList(_rootPath);     
        }
    }
}
