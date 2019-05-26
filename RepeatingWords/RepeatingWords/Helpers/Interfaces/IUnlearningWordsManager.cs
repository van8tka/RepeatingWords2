using RepeatingWords.DataService.Model;


namespace RepeatingWords.Helpers.Interfaces
{
    public interface IUnlearningWordsService
    {
        /// <summary>
        /// добавление слова в словарь не выученных слов 
        /// 1.в начале изучения 
        /// 2. при продолжении изучения 
        /// 3. в случае нового изучения уже изучавшегося словаря.
        /// Создание словаря не выученных слов при его отсутствии
        /// </summary> 
        /// <param name="nameDictionary">имя словаря загруженного для изучения</param>
        /// <param name="word">не выученное слово которое необходимо добавить в словарь</param>
        /// <returns></returns>
        void SaveUnlearningDictionary(string nameDictionary, Words word);
        /// <summary>
        /// удаление слов из словаря невыученных слов в двух случаях:
        /// 1. изучение невыученных слов 
        /// 2. продолжение изучения невыученных слов
        /// </summary>
        /// <param name="nameDictionary">имя словаря загруженного для изучения</param>
        /// <param name="word">слово для удаления</param>
        /// <returns></returns>
        void RemoveUnlearningDictionary(string nameDictionary, Words word);
        void CheckSaveOrRemoveWord(Words word, bool isSaveWord, string nameDictionary);
    }
}
