 

namespace RepeatingWords.Helpers.Interfaces
{
  public interface IKeyboardTranscriptionService
    {
        /// <summary>
        /// изменение клавиатуры по умолчанию для поля ввода транскрипции  True-кастомная клавиатура, False - клавиатура устройства 
        /// </summary>
        /// <returns></returns>
        bool ChangeUsingTranscriptionKeyboard();
        /// <summary>
        ///  получение клавиатуры по умолчанию для поля ввода транскрипции, True-кастомная клавиатура, False - клавиатура устройства 
        /// </summary>
        bool GetCurrentTranscriptionKeyboard();
    }
}
