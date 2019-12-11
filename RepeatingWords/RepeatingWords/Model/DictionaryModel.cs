using RepeatingWords.DataService.Model;

namespace RepeatingWords.Model
{
   public class DictionaryModel
    {
        public DictionaryModel(Dictionary dictionary)
        {
            Id = dictionary.Id;
            Name = dictionary.Name;
            PercentOfLearned = dictionary.PercentOfLearned.ToString();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        private string _percent;
        public string PercentOfLearned
        {
            get => _percent;
            set
            {
                if (int.Parse(value) == 0)
                    _percent = string.Empty;
                else
                    _percent = value + "%";
            }
        }
    }
}
