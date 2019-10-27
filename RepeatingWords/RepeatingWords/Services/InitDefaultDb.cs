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
                if (_unitOfWork.DictionaryRepository.Get().Count() == 0)
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
                    Transcription = "[ðə:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "и; а, но", EngWord = "and",
                    Transcription = "[ænd]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределенный артикль", EngWord = "a",
                    Transcription = "[ə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "к, в, на, до, для", EngWord = "to",
                    Transcription = "[tu:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было;", EngWord = "was",
                    Transcription = "[wɔz]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "я", EngWord = "I", Transcription = "[ʌi]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "3- е л. ед. ч. наст. врем. гл. to be",
                    EngWord = "is", Transcription = "[iz]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "из, от, о, об", EngWord = "of",
                    Transcription = "[ɔv]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "тот, та, то", EngWord = "that",
                    Transcription = "[ðæt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "ты, вы", EngWord = "you", Transcription = "[ju:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "он", EngWord = "he", Transcription = "[hi:]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "он, она, оно; это", EngWord = "it",
                    Transcription = "[it]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "in", Transcription = "[in]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "his", Transcription = "[hiz]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "имел, получал", EngWord = "had",
                    Transcription = "[hæd]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "делать", EngWord = "do", Transcription = "[du:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "с, вместе с", EngWord = "with",
                    Transcription = "[wið]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "не, нет; ни", EngWord = "not",
                    Transcription = "[nɔt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "её", EngWord = "her", Transcription = "[hз:]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "в течение, на, для", EngWord = "for",
                    Transcription = "[fɔ:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "на", EngWord = "on", Transcription = "[ɔn]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "около, у; в, на", EngWord = "at",
                    Transcription = "[æt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только, лишь, кроме, но, а", EngWord = "but",
                    Transcription = "[bʌt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "она", EngWord = "she", Transcription = "[ʃi:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "его", EngWord = "him", Transcription = "[him]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "как, когда", EngWord = "as",
                    Transcription = "[æz]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мн. ч. наст. врем. гл. to be", EngWord = "are",
                    Transcription = "[a:(r)]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "говорил, сказал", EngWord = "said",
                    Transcription = "[sed]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "они", EngWord = "they", Transcription = "[ðei]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "мы", EngWord = "we", Transcription = "[wi:]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "все, вся, всё", EngWord = "all",
                    Transcription = "[ɔ:l]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "этот, эта, это", EngWord = "this",
                    Transcription = "[ðis]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "иметь; получать; быть должным", EngWord = "have",
                    Transcription = "[hæv]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "там, туда, здесь", EngWord = "there",
                    Transcription = "[ðɛə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "что", EngWord = "what",
                    Transcription = "[(h)wɔt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вне, снаружи; за", EngWord = "out",
                    Transcription = "[aut]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "наверх(у), выше; вверх по, вдоль по",
                    EngWord = "up", Transcription = "[ʌp]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "один", EngWord = "one", Transcription = "[wʌn]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "от, из, с", EngWord = "from",
                    Transcription = "[frɔm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мне, меня", EngWord = "me",
                    Transcription = "[mi:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "идти, ехать ; уходить", EngWord = "go",
                    Transcription = "[gəu]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "были", EngWord = "were", Transcription = "[wз:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. глагол.; 2) модальный глагол",
                    EngWord = "would", Transcription = "[wud]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "похожий; как, подобно; любить, нравиться",
                    EngWord = "like", Transcription = "[laik]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "когда", EngWord = "when",
                    Transcription = "[(h)wen]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мог, умел", EngWord = "could",
                    Transcription = "[kud]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "тогда; затем", EngWord = "then",
                    Transcription = "[ðen]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "быть, существовать; находиться", EngWord = "be",
                    Transcription = "[bi:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "их , им", EngWord = "them",
                    Transcription = "[ðem]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "делал, выполнял", EngWord = "did",
                    Transcription = "[did]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "был, была, было", EngWord = "been",
                    Transcription = "[bi:n]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "теперь, сейчас", EngWord = "now",
                    Transcription = "[nau]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "взгляд, смотреть", EngWord = "look",
                    Transcription = "[luk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "спина, задний", EngWord = "back",
                    Transcription = "[bæk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "мой", EngWord = "my", Transcription = "[mai]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "нет, не", EngWord = "no", Transcription = "[nəu]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "твой, ваш", EngWord = "your",
                    Transcription = "[jɔ:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "который; что", EngWord = "which",
                    Transcription = "[(h)witʃ]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "кругом, вокруг; около; о, об, относительно",
                    EngWord = "about", Transcription = "[ə’baut]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "время; раз", EngWord = "time",
                    Transcription = "[taim]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вниз, внизу; вниз по, вдоль по",
                    EngWord = "down", Transcription = "[daun]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "в", EngWord = "into", Transcription = "[‘intu:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "кто; который", EngWord = "who",
                    Transcription = "[hu:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "мочь; уметь", EngWord = "can",
                    Transcription = "[kæn]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "знать", EngWord = "know", Transcription = "[nəu]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "если", EngWord = "if", Transcription = "[if]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только что", EngWord = "just",
                    Transcription = "[dʒʌst]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "их", EngWord = "their", Transcription = "[ðɛə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "получать; брать; приобретать", EngWord = "get",
                    Transcription = "[get]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "над; свыше", EngWord = "over",
                    Transcription = "[‘əuvə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "больше, более", EngWord = "more",
                    Transcription = "[mɔ:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "несколько", EngWord = "some",
                    Transcription = "[sʌm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "человек, мужчина", EngWord = "man",
                    Transcription = "[mæn]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "приходить, приезжать; случаться",
                    EngWord = "come", Transcription = "[kʌm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "неопределённый артикль", EngWord = "an",
                    Transcription = "[æn]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "так; тоже, также", EngWord = "so",
                    Transcription = "[səu]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "другой, иной, еще", EngWord = "other",
                    Transcription = "[‘ʌðə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "маленький", EngWord = "little",
                    Transcription = "[‘litl]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "видеть", EngWord = "see", Transcription = "[si:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "здесь, тут", EngWord = "here",
                    Transcription = "[hiə]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "вещь, предмет", EngWord = "thing",
                    Transcription = "[θiŋ]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "рука", EngWord = "hand", Transcription = "[hænd]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "у , около", EngWord = "by",
                    Transcription = "[bai]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1) вспом. гл. будущ. врем.; 2) модальный глагол",
                    EngWord = "will", Transcription = "[wil]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "путь, дорога", EngWord = "way",
                    Transcription = "[wei]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "опять, снова", EngWord = "again",
                    Transcription = "[ə’gein]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "правый; верно", EngWord = "right",
                    Transcription = "[rait]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "только", EngWord = "only",
                    Transcription = "[‘əunli]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "1-е л. ед.ч. наст. врем. гл. to be",
                    EngWord = "am", Transcription = "[æm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "как", EngWord = "how", Transcription = "[hau]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "думать; считать, полагать", EngWord = "think",
                    Transcription = "[θiŋk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "или", EngWord = "or", Transcription = "[ɔ:]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "получил", EngWord = "got",
                    Transcription = "[gɔt]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "хороший; добро", EngWord = "good",
                    Transcription = "[gud]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "глаз; взгляд", EngWord = "eye",
                    Transcription = "[ai]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "хорошо", EngWord = "well",
                    Transcription = "[wel]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "думал, мысль", EngWord = "thought",
                    Transcription = "[θɔ:t]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "день; сутки", EngWord = "day",
                    Transcription = "[dei]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "два", EngWord = "two", Transcription = "[tu:]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "чем, нежели", EngWord = "than",
                    Transcription = "[ðæn]"
                });
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
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
                    Transcription = "[v]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "not", EngWord = "не", Transcription = "[nyeh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "on, at, to", EngWord = "на",
                    Transcription = "[nah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "I", EngWord = "я", Transcription = "[yah]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "what, that", EngWord = "что",
                    Transcription = "[shtoh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to be, there is, there are", EngWord = "быть",
                    Transcription = "[bit']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "with, from", EngWord = "с", Transcription = "[s]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "he, it", EngWord = "он", Transcription = "[ohn]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "and, but", EngWord = "а", Transcription = "[ah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "how, as, like", EngWord = "как",
                    Transcription = "[kahk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "this is, that is", EngWord = "э́то",
                    Transcription = "[eh-tuh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "along, around, according to", EngWord = "по",
                    Transcription = "[poh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "this", EngWord = "э́тот",
                    Transcription = "[eh-tuht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to, towards", EngWord = "к",
                    Transcription = "[k]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "but", EngWord = "но", Transcription = "[noh]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "they", EngWord = "они́",
                    Transcription = "[uh-nee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "we", EngWord = "мы", Transcription = "[mi]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "all, whole", EngWord = "весь",
                    Transcription = "[vyehs']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "by, at", EngWord = "у", Transcription = "[oo]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "which", EngWord = "кото́рый",
                    Transcription = "[kah-toh-riy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "from, out of", EngWord = "из",
                    Transcription = "[is]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "she", EngWord = "она́",
                    Transcription = "[uh-nah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "for, behind", EngWord = "за",
                    Transcription = "[zah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "from", EngWord = "от", Transcription = "[oht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "about", EngWord = "о", Transcription = "[oh]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "so", EngWord = "так", Transcription = "[tahk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "one's own", EngWord = "свой",
                    Transcription = "[svoy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "for", EngWord = "для", Transcription = "[dlyah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary,
                    RusWord = "and, as for, but, (a particle used for emphasis)", EngWord = "же",
                    Transcription = "[zheh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "year", EngWord = "год", Transcription = "[goht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "you (plural or polite)", EngWord = "вы",
                    Transcription = "[vi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "you (single, familiar)", EngWord = "ты",
                    Transcription = "[ti]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "can, to be able to", EngWord = "мочь",
                    Transcription = "[mohch']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "person, human", EngWord = "челове́к",
                    Transcription = "[chee-luh-vyehk]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "such", EngWord = "тако́й",
                    Transcription = "[tah-koi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "all", EngWord = "все", Transcription = "[fsyeh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "his", EngWord = "его́",
                    Transcription = "[ee-voh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "only", EngWord = "то́лько",
                    Transcription = "[tohl'-kuh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "yet, still, more", EngWord = "ещё",
                    Transcription = "[yi-schoh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "or", EngWord = "и́ли", Transcription = "[ee-lee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "would", EngWord = "бы", Transcription = "[bi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to say, speak", EngWord = "сказа́ть",
                    Transcription = "[skah-zaht']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "oneself", EngWord = "себя́",
                    Transcription = "[see-byah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "her", EngWord = "её", Transcription = "[ee-yoh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "if", EngWord = "е́сли",
                    Transcription = "[yes-lee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "already", EngWord = "уже́",
                    Transcription = "[oo-zhe]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "till, until", EngWord = "до",
                    Transcription = "[doh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "when", EngWord = "когда́",
                    Transcription = "[kuhg-dah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "time", EngWord = "вре́мя",
                    Transcription = "[vryeh-mya]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "here, there", EngWord = "вот",
                    Transcription = "[voht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "that", EngWord = "то", Transcription = "[toh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "other, another, different", EngWord = "друго́й",
                    Transcription = "[droo-goi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to talk", EngWord = "говори́ть",
                    Transcription = "[guh-vuh-reet']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "our", EngWord = "наш", Transcription = "[nahsh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                    {Id = 0, IdDictionary = idDefdictionary, RusWord = "yes", EngWord = "да", Transcription = "[dah]"});
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "themost", EngWord = "са́мый",
                    Transcription = "[sah-miy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "my, mine", EngWord = "мой",
                    Transcription = "[moi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "at, in, by", EngWord = "при",
                    Transcription = "[pree]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "not, no", EngWord = "нет",
                    Transcription = "[nyeht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "that, than, in order that, so",
                    EngWord = "что́бы", Transcription = "[shtohbi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "who", EngWord = "кто", Transcription = "[ktoh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "even", EngWord = "да́же",
                    Transcription = "[dahzhe]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "business, affair", EngWord = "де́ло",
                    Transcription = "[dyehluh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "no, not, not a", EngWord = "ни",
                    Transcription = "[nee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "life", EngWord = "жизнь",
                    Transcription = "[zhizhn']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to become", EngWord = "стать",
                    Transcription = "[staht']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "first", EngWord = "пе́рвый",
                    Transcription = "[pyerviy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "very", EngWord = "о́чень",
                    Transcription = "[oh-cheen']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "two", EngWord = "два", Transcription = "[dvah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "oneself, myself, himself, herself, alone",
                    EngWord = "сам", Transcription = "[sahm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "there", EngWord = "там", Transcription = "[tahm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "time, occasion, bout, once", EngWord = "раз",
                    Transcription = "[rahs]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "new", EngWord = "но́вый",
                    Transcription = "[noh-viy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "well", EngWord = "ну", Transcription = "[noo]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "one can, one may, it's possible",
                    EngWord = "мо́жно", Transcription = "[mohzh-nah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "day", EngWord = "день",
                    Transcription = "[dyehn']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "hand, arm", EngWord = "рука́",
                    Transcription = "[roo-kah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "with (a different spelling of 'c')",
                    EngWord = "со", Transcription = "[soh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "which", EngWord = "како́й",
                    Transcription = "[kah-koi]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "their, theirs", EngWord = "их",
                    Transcription = "[eekh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "under, below", EngWord = "под",
                    Transcription = "[poht]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "many", EngWord = "мно́го",
                    Transcription = "[mnoh-guh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "if, whether", EngWord = "ли",
                    Transcription = "[lee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "than", EngWord = "чем", Transcription = "[chyem]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "after", EngWord = "по́сле",
                    Transcription = "[pohs-lee]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "work, job, labor", EngWord = "рабо́та",
                    Transcription = "[rah-boh-tah]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "must, it's necessary", EngWord = "на́до",
                    Transcription = "[nah-duh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to want", EngWord = "хоте́ть",
                    Transcription = "[khat-tyeht']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "good, nice, fine", EngWord = "хоро́ший",
                    Transcription = "[khah-roh-shiy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "without", EngWord = "без",
                    Transcription = "[byes]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "now", EngWord = "сейча́с",
                    Transcription = "[see-chahs]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a word", EngWord = "сло́во",
                    Transcription = "[sloh-vuh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "due, owing, proper, just, indebted",
                    EngWord = "до́лжный", Transcription = "[dohlzhniy]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "after, afterwards", EngWord = "пото́м",
                    Transcription = "[puh-tohm]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "to have, own", EngWord = "име́ть",
                    Transcription = "[ee-myeht']"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a place", EngWord = "ме́сто",
                    Transcription = "[myehs-tuh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a question", EngWord = "вопро́с",
                    Transcription = "[vah-prohs]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "nothing", EngWord = "ничто́",
                    Transcription = "[neesh-toh]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "also, too", EngWord = "то́же",
                    Transcription = "[toh-zhe]"
                });
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0, IdDictionary = idDefdictionary, RusWord = "a face", EngWord = "лицо́",
                    Transcription = "[lee-tsoh]"
                });
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
