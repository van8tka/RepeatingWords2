using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Linq;

namespace RepeatingWords.Services
{
    public class InitDefaultDb : IInitDefaultDb
    {
        private readonly IUnitOfWork _unitOfWork;    

        public InitDefaultDb(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));          
        }

        public bool LoadDefaultData()
        {
            try
            {              
                if (_unitOfWork.DictionaryRepository.Get().Count()==0)
                {
                    Log.Logger.Info("Init new database");
                    int idDefdictionary = CreateDefaultDictionary();
                    CreateDefaultWords(idDefdictionary);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;              
            }
        }


        private void CreateDefaultWords(int idDefdictionary)
        {
            if(string.Equals( Resource.IsCurrentLang, "ru"))
            {
                CreateDefaultRussianWords( idDefdictionary );
            }
            else
            {
                CreateDefaultEnglishWords( idDefdictionary );
            }
        }



        private void CreateDefaultRussianWords(int idDefdictionary)
        {
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "определенный артикль", EngWord = "the", Transcription = "[ðə:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "и; а, но", EngWord = "and", Transcription = "[ænd]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределенный артикль", EngWord = "a", Transcription = "[ə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "к, в, на, до, для", EngWord = "to", Transcription = "[tu:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было;", EngWord = "was", Transcription = "[wɔz]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "я", EngWord = "I", Transcription = "[ʌi]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "3- е л. ед. ч. наст. врем. гл. to be", EngWord = "is", Transcription = "[iz]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "из, от, о, об", EngWord = "of", Transcription = "[ɔv]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "тот, та, то", EngWord = "that", Transcription = "[ðæt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "ты, вы", EngWord = "you", Transcription = "[ju:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "он", EngWord = "he", Transcription = "[hi:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "он, она, оно; это", EngWord = "it", Transcription = "[it]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "in", Transcription = "[in]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "his", Transcription = "[hiz]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "имел, получал", EngWord = "had", Transcription = "[hæd]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "делать", EngWord = "do", Transcription = "[du:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "с, вместе с", EngWord = "with", Transcription = "[wið]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "не, нет; ни", EngWord = "not", Transcription = "[nɔt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "её", EngWord = "her", Transcription = "[hз:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "в течение, на, для", EngWord = "for", Transcription = "[fɔ:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "на", EngWord = "on", Transcription = "[ɔn]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "около, у; в, на", EngWord = "at", Transcription = "[æt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "только, лишь, кроме, но, а", EngWord = "but", Transcription = "[bʌt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "она", EngWord = "she", Transcription = "[ʃi:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "him", Transcription = "[him]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "как, когда", EngWord = "as", Transcription = "[æz]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мн. ч. наст. врем. гл. to be", EngWord = "are", Transcription = "[a:(r)]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "говорил, сказал", EngWord = "said", Transcription = "[sed]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "они", EngWord = "they", Transcription = "[ðei]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мы", EngWord = "we", Transcription = "[wi:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "все, вся, всё", EngWord = "all", Transcription = "[ɔ:l]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "этот, эта, это", EngWord = "this", Transcription = "[ðis]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "иметь; получать; быть должным", EngWord = "have", Transcription = "[hæv]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "там, туда, здесь", EngWord = "there", Transcription = "[ðɛə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "что", EngWord = "what", Transcription = "[(h)wɔt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "вне, снаружи; за", EngWord = "out", Transcription = "[aut]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "наверх(у), выше; вверх по, вдоль по", EngWord = "up", Transcription = "[ʌp]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "один", EngWord = "one", Transcription = "[wʌn]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "от, из, с", EngWord = "from", Transcription = "[frɔm]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мне, меня", EngWord = "me", Transcription = "[mi:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "идти, ехать ; уходить", EngWord = "go", Transcription = "[gəu]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "были", EngWord = "were", Transcription = "[wз:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. глагол.; 2) модальный глагол", EngWord = "would", Transcription = "[wud]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "похожий; как, подобно; любить, нравиться", EngWord = "like", Transcription = "[laik]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "когда", EngWord = "when", Transcription = "[(h)wen]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мог, умел", EngWord = "could", Transcription = "[kud]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "тогда; затем", EngWord = "then", Transcription = "[ðen]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "быть, существовать; находиться", EngWord = "be", Transcription = "[bi:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "их , им", EngWord = "them", Transcription = "[ðem]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "делал, выполнял", EngWord = "did", Transcription = "[did]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было", EngWord = "been", Transcription = "[bi:n]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "теперь, сейчас", EngWord = "now", Transcription = "[nau]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "взгляд, смотреть", EngWord = "look", Transcription = "[luk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "спина, задний", EngWord = "back", Transcription = "[bæk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мой", EngWord = "my", Transcription = "[mai]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "нет, не", EngWord = "no", Transcription = "[nəu]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "твой, ваш", EngWord = "your", Transcription = "[jɔ:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "который; что", EngWord = "which", Transcription = "[(h)witʃ]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "кругом, вокруг; около; о, об, относительно", EngWord = "about", Transcription = "[ə’baut]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "время; раз", EngWord = "time", Transcription = "[taim]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "вниз, внизу; вниз по, вдоль по", EngWord = "down", Transcription = "[daun]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "into", Transcription = "[‘intu:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "кто; который", EngWord = "who", Transcription = "[hu:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "мочь; уметь", EngWord = "can", Transcription = "[kæn]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "знать", EngWord = "know", Transcription = "[nəu]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "если", EngWord = "if", Transcription = "[if]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "только что", EngWord = "just", Transcription = "[dʒʌst]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "их", EngWord = "their", Transcription = "[ðɛə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "получать; брать; приобретать", EngWord = "get", Transcription = "[get]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "над; свыше", EngWord = "over", Transcription = "[‘əuvə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "больше, более", EngWord = "more", Transcription = "[mɔ:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "несколько", EngWord = "some", Transcription = "[sʌm]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "человек, мужчина", EngWord = "man", Transcription = "[mæn]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "приходить, приезжать; случаться", EngWord = "come", Transcription = "[kʌm]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределённый артикль", EngWord = "an", Transcription = "[æn]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "так; тоже, также", EngWord = "so", Transcription = "[səu]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "другой, иной, еще", EngWord = "other", Transcription = "[‘ʌðə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "маленький", EngWord = "little", Transcription = "[‘litl]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "видеть", EngWord = "see", Transcription = "[si:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "здесь, тут", EngWord = "here", Transcription = "[hiə]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "вещь, предмет", EngWord = "thing", Transcription = "[θiŋ]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "рука", EngWord = "hand", Transcription = "[hænd]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "у , около", EngWord = "by", Transcription = "[bai]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. гл. будущ. врем.; 2) модальный глагол", EngWord = "will", Transcription = "[wil]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "путь, дорога", EngWord = "way", Transcription = "[wei]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "опять, снова", EngWord = "again", Transcription = "[ə’gein]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "правый; верно", EngWord = "right", Transcription = "[rait]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "только", EngWord = "only", Transcription = "[‘əunli]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "1-е л. ед.ч. наст. врем. гл. to be", EngWord = "am", Transcription = "[æm]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "как", EngWord = "how", Transcription = "[hau]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "думать; считать, полагать", EngWord = "think", Transcription = "[θiŋk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "или", EngWord = "or", Transcription = "[ɔ:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "получил", EngWord = "got", Transcription = "[gɔt]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "хороший; добро", EngWord = "good", Transcription = "[gud]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "глаз; взгляд", EngWord = "eye", Transcription = "[ai]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "хорошо", EngWord = "well", Transcription = "[wel]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "думал, мысль", EngWord = "thought", Transcription = "[θɔ:t]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "день; сутки", EngWord = "day", Transcription = "[dei]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "два", EngWord = "two", Transcription = "[tu:]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "чем, нежели", EngWord = "than", Transcription = "[ðæn]" });
            _unitOfWork.Save();
        }


        private void CreateDefaultEnglishWords(int idDefdictionary)
        {
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "dictionary", EngWord = "ручка", Transcription = "[ˈdɪkʃəneri]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "book", EngWord = "примечание", Transcription = "[bʊk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "table", EngWord = "человек", Transcription = "[teɪb(ə)l]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "pen", EngWord = "table", Transcription = "[pen]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "note", EngWord = "человек", Transcription = "[nəut]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "people", EngWord = "people", Transcription = "[ˈpi:pl]" });
            _unitOfWork.Save();
        }

        private int CreateDefaultDictionary()
        {             
            var dic = _unitOfWork.DictionaryRepository.Create(new Dictionary()
            {
                Id = 0,
                Name = Resource.DefaultDictionaryName
            });
            _unitOfWork.Save();
            return dic.Id;
        }
    }
}
