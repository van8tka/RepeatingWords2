using RepeatingWords.Model;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Linq;
using RepeatingWords.DataService.Model;
using System.Diagnostics;
using RepeatingWords.Services;
using RepeatingWords.LoggerService;

namespace RepeatingWords.Pages
{
    public partial class ChooseFile : ContentPage
    {
        Dictionary dictionary;
        string RootPath;
        bool getFolder;
       


        //для добавления слов из выбранного файла и выбора файла
        public ChooseFile(Dictionary dictionary=null)
        {
            InitializeComponent();
            getFolder = false;
            this.dictionary = dictionary;                
        }
        protected async override void OnAppearing()
        {
            progressBar.IsVisible = true;
            await progressBar.ProgressTo(0.9, 1000, Easing.CubicInOut);
            await GetFolderList();
            progressBar.IsVisible = false;
            progressBar.Progress = 0.1;
            base.OnAppearing();
        }




      
        private async Task GetFolderList()
        {           
                try
                {
                        RootPath = DependencyService.Get<IFolderWorker>().GetRootPath();
                        var Permission = await UpdateFileList(getFolder);
                        TextPath.Text = Resource.LabelPathToRoot + " " + RootPath;
                }
                catch (Exception er)
                {
                Log.Logger.Error(er);
            }
        }



        //при коротком нажатии на папку или на файл
        private async Task FileSelected(string nameItemSelected)
        {
            try
            {
                if (nameItemSelected == null) return;
                string filename = nameItemSelected;
                var varPath = RootPath + "/" + filename;
                //проверим тапнут file or folder
                //if folder
                if (!DependencyService.Get<IFileWorker>().IsFile(varPath))
                {
                    RootPath = RootPath + "/" + filename;
                    TextPath.Text = TextPath.Text + "/" + filename;
                    await UpdateFileList(getFolder, RootPath);
                }
                else//если файл 
                {
                    if(dictionary!=null)//если dictionary не нуль
                    {
                       progressBar.IsVisible = true;
                       await progressBar.ProgressTo(0.9, 1000, Easing.CubicInOut);
                       await GetWordsFromFile(varPath);//то получаем список слов  
                       progressBar.IsVisible = false;
                       progressBar.Progress = 0.1;
                    }
                
                }
              
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }









        private async Task GetWordsFromFile(string filePath)
        {
           
                try
                {
                    string ModalAddWords = Resource.ModalAddWords;
                    string ModalException = Resource.ModalException;
                    string ModalIncorrectFile = Resource.ModalIncorrectFile;
                 
                    List<string> lines = await DependencyService.Get<IFileWorker>().LoadTextAsync(filePath);
                //проходим по списку строк считанных из файла
                if (lines != null && lines.Count>0 && filePath.EndsWith(".txt"))
                    {
                        char[] delim = { '[', ']' };
                        //переменная для проверки добавления слов
                        bool CreateWordsFromFile = false;
                        //проход по списку слов
                        foreach (var i in lines)
                        {//проверка на наличие разделителей, т.е. транскрипции в строке(символы транскрипции и есть разделители)
                            if (i.Contains("[") && i.Contains("]"))
                            {
                                CreateWordsFromFile = true;
                                string[] fileWords = i.Split(delim);
                                Words item = new Words
                                {
                                    Id = 0,
                                    IdDictionary = dictionary.Id,
                                    RusWord = fileWords[0].Trim(),
                                    Transcription = "[" + fileWords[1] + "]",
                                    EngWord = fileWords[2].Trim()
                                };//добавим слово в БД
                            Debugger.Break();
                            //  await Task.Run(()=> App.Wr.CreateWord(item));
                        }
                        }          
                        if (CreateWordsFromFile)
                        {
                            await DisplayAlert("", ModalAddWords, "Ok");
                            AddWord adw = new AddWord(dictionary);
                            await Navigation.PushAsync(adw);
                        }
                        else
                        {
                            await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                    }
                }
                catch (Exception er)
                {
                Log.Logger.Error(er);
            }                 
        }


       private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem!=null)
            {
                string itemTap = e.SelectedItem.ToString();
                //если нажали элемент списка ..  то выходим на верх
                if (itemTap == "..")
                    OnBackButtonPressed();
                else
                    await FileSelected(itemTap);
            }           
        }
            
        public async Task<bool> UpdateFileList(bool getFolderList, string folderPath = null)
        {
            try
            {
                string backItem = "..";
                //получим список файлов или папок
                if (!getFolderList)
                {
                    //получаем все файлы
                    var folderL = await DependencyService.Get<IFolderWorker>().GetFoldersAsync(folderPath);
                    
                    var fileL = await DependencyService.Get<IFileWorker>().GetFilesAsync(RootPath);
                    if(fileL.Any())
                         folderL.AddRange(fileL);
                    folderL.Insert(0, backItem);
                    fileList.ItemsSource = folderL;
                }
                else //или папок
                {
                    var items = await DependencyService.Get<IFolderWorker>().GetFoldersAsync(folderPath);
                    //добавим элемент назад
                    items.Insert(0, backItem);                
                    fileList.ItemsSource = items;
                }
                if (fileList != null)
                {
                    fileList.SelectedItem = null;
                    return true;                  
                }                 
                else           
                {
                    fileList.SelectedItem = null;
                    return false;
                }
                                            
            }
            catch(Exception er)
            {
                Log.Logger.Error(er);
                return false;
            }
        }



        //вызов главной страницы и чистка стека страниц
        private void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }




       

        //переопределение метода обработки события при нажатии кнопки НАЗАД, 
        //для его срабатывания надо в MainActivity тоже переопределить метод OnBackPressed
        protected override bool OnBackButtonPressed()
        {
            string rootPath = DependencyService.Get<IFolderWorker>().GetRootPath();
            if(!string.IsNullOrEmpty(RootPath))
            {
                if (RootPath == rootPath)
                {
                    Navigation.PopAsync();
                }
                else
                {
                    string textpath = TextPath.Text;
                    RootPath = RootPath.Remove(RootPath.LastIndexOf('/'));
                    TextPath.Text = textpath.Remove(textpath.LastIndexOf('/'));
#pragma warning disable CS4014
                    UpdateFileList(getFolder, RootPath);
#pragma warning restore CS4014
                }
            }            
            else
                 Navigation.PopAsync();
            return true;
        }
    }
}