using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Diagnostics;
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
                if (_unitOfWork.DictionaryRepository.Get().Count() == 0)
                {
                    Log.Logger.Info("Init new database");
                    int idLang = CreateDefaultLanguage();
                    int idDefdictionary = CreateDefaultDictionary(idLang);
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

        private int CreateDefaultLanguage()
        {
            var lang = _unitOfWork.LanguageRepository.Create(new Language()
            {
                Id = 0,
                NameLanguage = Resource.DefaultLanguage,
                PercentOfLearned = 0
            }); 
            _unitOfWork.Save();
            return lang.Id;
        }

        
        private int CreateDefaultDictionary(int idLanguage)
        {
            var dic = _unitOfWork.DictionaryRepository.Create(new Dictionary()
            {
                Id = 0,
                Name = Resource.DefaultDictionaryName,
                IdLanguage = idLanguage,
                PercentOfLearned = 0,
                LastUpdated = DateTime.UtcNow,
                IsBeginLearned = false
            });
            _unitOfWork.Save();
            return dic.Id;
        }

        private void CreateDefaultWords(int idDefdictionary)
        {
            if (string.Equals(Resource.IsCurrentLang, "ru"))
            {
                CreateDefaultRussianWords(idDefdictionary);
            }
            else
            {
                CreateDefaultEnglishWords(idDefdictionary);
            }
        }



        private void CreateDefaultRussianWords(int idDefdictionary)
        {
            try
            {
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "определенный артикль", EngWord = "the",
                    Transcription = "[ðə:]", IsLearned = false

                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "и; а, но", EngWord = "and",
                    Transcription = "[ænd]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределенный артикль", EngWord = "a",
                    Transcription = "[ə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "к, в, на, до, для", EngWord = "to",
                    Transcription = "[tu:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было;", EngWord = "was",
                    Transcription = "[wɔz]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "я", EngWord = "I", Transcription = "[ʌi]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "3- е л. ед. ч. наст. врем. гл. to be",
                    EngWord = "is", Transcription = "[iz]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "из, от, о, об", EngWord = "of",
                    Transcription = "[ɔv]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "тот, та, то", EngWord = "that",
                    Transcription = "[ðæt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "ты, вы", EngWord = "you", Transcription = "[ju:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "он", EngWord = "he", Transcription = "[hi:]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "он, она, оно; это", EngWord = "it",
                    Transcription = "[it]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "in", Transcription = "[in]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "his", Transcription = "[hiz]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "имел, получал", EngWord = "had",
                    Transcription = "[hæd]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "делать", EngWord = "do", Transcription = "[du:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "с, вместе с", EngWord = "with",
                    Transcription = "[wið]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "не, нет; ни", EngWord = "not",
                    Transcription = "[nɔt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "её", EngWord = "her", Transcription = "[hз:]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "в течение, на, для", EngWord = "for",
                    Transcription = "[fɔ:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "на", EngWord = "on", Transcription = "[ɔn]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "около, у; в, на", EngWord = "at",
                    Transcription = "[æt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только, лишь, кроме, но, а", EngWord = "but",
                    Transcription = "[bʌt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "она", EngWord = "she", Transcription = "[ʃi:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "him", Transcription = "[him]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "как, когда", EngWord = "as",
                    Transcription = "[æz]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мн. ч. наст. врем. гл. to be", EngWord = "are",
                    Transcription = "[a:(r)]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "говорил, сказал", EngWord = "said",
                    Transcription = "[sed]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "они", EngWord = "they", Transcription = "[ðei]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "мы", EngWord = "we", Transcription = "[wi:]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "все, вся, всё", EngWord = "all",
                    Transcription = "[ɔ:l]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "этот, эта, это", EngWord = "this",
                    Transcription = "[ðis]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "иметь; получать; быть должным", EngWord = "have",
                    Transcription = "[hæv]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "там, туда, здесь", EngWord = "there",
                    Transcription = "[ðɛə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "что", EngWord = "what",
                    Transcription = "[(h)wɔt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вне, снаружи; за", EngWord = "out",
                    Transcription = "[aut]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "наверх(у), выше; вверх по, вдоль по",
                    EngWord = "up", Transcription = "[ʌp]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "один", EngWord = "one", Transcription = "[wʌn]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "от, из, с", EngWord = "from",
                    Transcription = "[frɔm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мне, меня", EngWord = "me",
                    Transcription = "[mi:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "идти, ехать ; уходить", EngWord = "go",
                    Transcription = "[gəu]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "были", EngWord = "were", Transcription = "[wз:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. глагол.; 2) модальный глагол",
                    EngWord = "would", Transcription = "[wud]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "похожий; как, подобно; любить, нравиться",
                    EngWord = "like", Transcription = "[laik]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "когда", EngWord = "when",
                    Transcription = "[(h)wen]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мог, умел", EngWord = "could",
                    Transcription = "[kud]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "тогда; затем", EngWord = "then",
                    Transcription = "[ðen]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "быть, существовать; находиться", EngWord = "be",
                    Transcription = "[bi:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "их , им", EngWord = "them",
                    Transcription = "[ðem]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "делал, выполнял", EngWord = "did",
                    Transcription = "[did]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было", EngWord = "been",
                    Transcription = "[bi:n]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "теперь, сейчас", EngWord = "now",
                    Transcription = "[nau]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "взгляд, смотреть", EngWord = "look",
                    Transcription = "[luk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "спина, задний", EngWord = "back",
                    Transcription = "[bæk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "мой", EngWord = "my", Transcription = "[mai]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "нет, не", EngWord = "no", Transcription = "[nəu]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "твой, ваш", EngWord = "your",
                    Transcription = "[jɔ:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "который; что", EngWord = "which",
                    Transcription = "[(h)witʃ]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "кругом, вокруг; около; о, об, относительно",
                    EngWord = "about", Transcription = "[ə’baut]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "время; раз", EngWord = "time",
                    Transcription = "[taim]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вниз, внизу; вниз по, вдоль по",
                    EngWord = "down", Transcription = "[daun]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "into", Transcription = "[‘intu:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "кто; который", EngWord = "who",
                    Transcription = "[hu:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мочь; уметь", EngWord = "can",
                    Transcription = "[kæn]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "знать", EngWord = "know", Transcription = "[nəu]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "если", EngWord = "if", Transcription = "[if]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только что", EngWord = "just",
                    Transcription = "[dʒʌst]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "их", EngWord = "their", Transcription = "[ðɛə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "получать; брать; приобретать", EngWord = "get",
                    Transcription = "[get]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "над; свыше", EngWord = "over",
                    Transcription = "[‘əuvə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "больше, более", EngWord = "more",
                    Transcription = "[mɔ:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "несколько", EngWord = "some",
                    Transcription = "[sʌm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "человек, мужчина", EngWord = "man",
                    Transcription = "[mæn]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "приходить, приезжать; случаться",
                    EngWord = "come", Transcription = "[kʌm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределённый артикль", EngWord = "an",
                    Transcription = "[æn]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "так; тоже, также", EngWord = "so",
                    Transcription = "[səu]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "другой, иной, еще", EngWord = "other",
                    Transcription = "[‘ʌðə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "маленький", EngWord = "little",
                    Transcription = "[‘litl]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "видеть", EngWord = "see", Transcription = "[si:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "здесь, тут", EngWord = "here",
                    Transcription = "[hiə]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вещь, предмет", EngWord = "thing",
                    Transcription = "[θiŋ]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "рука", EngWord = "hand", Transcription = "[hænd]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "у , около", EngWord = "by",
                    Transcription = "[bai]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. гл. будущ. врем.; 2) модальный глагол",
                    EngWord = "will", Transcription = "[wil]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "путь, дорога", EngWord = "way",
                    Transcription = "[wei]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "опять, снова", EngWord = "again",
                    Transcription = "[ə’gein]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "правый; верно", EngWord = "right",
                    Transcription = "[rait]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только", EngWord = "only",
                    Transcription = "[‘əunli]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1-е л. ед.ч. наст. врем. гл. to be",
                    EngWord = "am", Transcription = "[æm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "как", EngWord = "how", Transcription = "[hau]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "думать; считать, полагать", EngWord = "think",
                    Transcription = "[θiŋk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "или", EngWord = "or", Transcription = "[ɔ:]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "получил", EngWord = "got",
                    Transcription = "[gɔt]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "хороший; добро", EngWord = "good",
                    Transcription = "[gud]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "глаз; взгляд", EngWord = "eye",
                    Transcription = "[ai]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "хорошо", EngWord = "well",
                    Transcription = "[wel]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "думал, мысль", EngWord = "thought",
                    Transcription = "[θɔ:t]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "день; сутки", EngWord = "day",
                    Transcription = "[dei]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "два", EngWord = "two", Transcription = "[tu:]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "чем, нежели", EngWord = "than",
                    Transcription = "[ðæn]",
                    IsLearned = false
                });
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }


        private void CreateDefaultEnglishWords(int idDefdictionary)
        {
            try
            {
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "in, into, to", EngWord = "в",
                    Transcription = "[v]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "not", EngWord = "не", Transcription = "[nyeh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "on, at, to", EngWord = "на",
                    Transcription = "[nah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "I", EngWord = "я", Transcription = "[yah]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "what, that", EngWord = "что",
                    Transcription = "[shtoh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to be, there is, there are", EngWord = "быть",
                    Transcription = "[bit']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "with, from", EngWord = "с", Transcription = "[s]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "he, it", EngWord = "он", Transcription = "[ohn]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "and, but", EngWord = "а", Transcription = "[ah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "how, as, like", EngWord = "как",
                    Transcription = "[kahk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "this is, that is", EngWord = "э́то",
                    Transcription = "[eh-tuh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "along, around, according to", EngWord = "по",
                    Transcription = "[poh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "this", EngWord = "э́тот",
                    Transcription = "[eh-tuht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to, towards", EngWord = "к",
                    Transcription = "[k]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "but", EngWord = "но", Transcription = "[noh]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "they", EngWord = "они́",
                    Transcription = "[uh-nee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "we", EngWord = "мы", Transcription = "[mi]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "all, whole", EngWord = "весь",
                    Transcription = "[vyehs']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "by, at", EngWord = "у", Transcription = "[oo]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "which", EngWord = "кото́рый",
                    Transcription = "[kah-toh-riy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "from, out of", EngWord = "из",
                    Transcription = "[is]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "she", EngWord = "она́",
                    Transcription = "[uh-nah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "for, behind", EngWord = "за",
                    Transcription = "[zah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "from", EngWord = "от", Transcription = "[oht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "about", EngWord = "о", Transcription = "[oh]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "so", EngWord = "так", Transcription = "[tahk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "one's own", EngWord = "свой",
                    Transcription = "[svoy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "for", EngWord = "для", Transcription = "[dlyah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary,
                    RusWord = "and, as for, but, (a particle used for emphasis)", EngWord = "же",
                    Transcription = "[zheh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "year", EngWord = "год", Transcription = "[goht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "you (plural or polite)", EngWord = "вы",
                    Transcription = "[vi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "you (single, familiar)", EngWord = "ты",
                    Transcription = "[ti]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "can, to be able to", EngWord = "мочь",
                    Transcription = "[mohch']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "person, human", EngWord = "челове́к",
                    Transcription = "[chee-luh-vyehk]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "such", EngWord = "тако́й",
                    Transcription = "[tah-koi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "all", EngWord = "все", Transcription = "[fsyeh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "his", EngWord = "его́",
                    Transcription = "[ee-voh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "only", EngWord = "то́лько",
                    Transcription = "[tohl'-kuh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "yet, still, more", EngWord = "ещё",
                    Transcription = "[yi-schoh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "or", EngWord = "и́ли", Transcription = "[ee-lee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "would", EngWord = "бы", Transcription = "[bi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to say, speak", EngWord = "сказа́ть",
                    Transcription = "[skah-zaht']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "oneself", EngWord = "себя́",
                    Transcription = "[see-byah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "her", EngWord = "её", Transcription = "[ee-yoh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "if", EngWord = "е́сли",
                    Transcription = "[yes-lee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "already", EngWord = "уже́",
                    Transcription = "[oo-zhe]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "till, until", EngWord = "до",
                    Transcription = "[doh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "when", EngWord = "когда́",
                    Transcription = "[kuhg-dah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "time", EngWord = "вре́мя",
                    Transcription = "[vryeh-mya]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "here, there", EngWord = "вот",
                    Transcription = "[voht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "that", EngWord = "то", Transcription = "[toh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "other, another, different", EngWord = "друго́й",
                    Transcription = "[droo-goi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to talk", EngWord = "говори́ть",
                    Transcription = "[guh-vuh-reet']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "our", EngWord = "наш", Transcription = "[nahsh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "yes", EngWord = "да", Transcription = "[dah]", IsLearned = false });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "themost", EngWord = "са́мый",
                    Transcription = "[sah-miy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "my, mine", EngWord = "мой",
                    Transcription = "[moi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "at, in, by", EngWord = "при",
                    Transcription = "[pree]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "not, no", EngWord = "нет",
                    Transcription = "[nyeht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "that, than, in order that, so",
                    EngWord = "что́бы", Transcription = "[shtohbi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "who", EngWord = "кто", Transcription = "[ktoh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "even", EngWord = "да́же",
                    Transcription = "[dahzhe]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "business, affair", EngWord = "де́ло",
                    Transcription = "[dyehluh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "no, not, not a", EngWord = "ни",
                    Transcription = "[nee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "life", EngWord = "жизнь",
                    Transcription = "[zhizhn']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to become", EngWord = "стать",
                    Transcription = "[staht']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "first", EngWord = "пе́рвый",
                    Transcription = "[pyerviy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "very", EngWord = "о́чень",
                    Transcription = "[oh-cheen']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "two", EngWord = "два", Transcription = "[dvah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "oneself, myself, himself, herself, alone",
                    EngWord = "сам", Transcription = "[sahm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "there", EngWord = "там", Transcription = "[tahm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "time, occasion, bout, once", EngWord = "раз",
                    Transcription = "[rahs]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "new", EngWord = "но́вый",
                    Transcription = "[noh-viy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "well", EngWord = "ну", Transcription = "[noo]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "one can, one may, it's possible",
                    EngWord = "мо́жно", Transcription = "[mohzh-nah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "day", EngWord = "день",
                    Transcription = "[dyehn']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "hand, arm", EngWord = "рука́",
                    Transcription = "[roo-kah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "with (a different spelling of 'c')",
                    EngWord = "со", Transcription = "[soh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "which", EngWord = "како́й",
                    Transcription = "[kah-koi]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "their, theirs", EngWord = "их",
                    Transcription = "[eekh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "under, below", EngWord = "под",
                    Transcription = "[poht]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "many", EngWord = "мно́го",
                    Transcription = "[mnoh-guh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "if, whether", EngWord = "ли",
                    Transcription = "[lee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "than", EngWord = "чем", Transcription = "[chyem]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "after", EngWord = "по́сле",
                    Transcription = "[pohs-lee]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "work, job, labor", EngWord = "рабо́та",
                    Transcription = "[rah-boh-tah]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "must, it's necessary", EngWord = "на́до",
                    Transcription = "[nah-duh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to want", EngWord = "хоте́ть",
                    Transcription = "[khat-tyeht']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "good, nice, fine", EngWord = "хоро́ший",
                    Transcription = "[khah-roh-shiy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "without", EngWord = "без",
                    Transcription = "[byes]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "now", EngWord = "сейча́с",
                    Transcription = "[see-chahs]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a word", EngWord = "сло́во",
                    Transcription = "[sloh-vuh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "due, owing, proper, just, indebted",
                    EngWord = "до́лжный", Transcription = "[dohlzhniy]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "after, afterwards", EngWord = "пото́м",
                    Transcription = "[puh-tohm]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to have, own", EngWord = "име́ть",
                    Transcription = "[ee-myeht']",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a place", EngWord = "ме́сто",
                    Transcription = "[myehs-tuh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a question", EngWord = "вопро́с",
                    Transcription = "[vah-prohs]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "nothing", EngWord = "ничто́",
                    Transcription = "[neesh-toh]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "also, too", EngWord = "то́же",
                    Transcription = "[toh-zhe]",
                    IsLearned = false
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a face", EngWord = "лицо́",
                    Transcription = "[lee-tsoh]",
                    IsLearned = false
                });
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

    }
}
