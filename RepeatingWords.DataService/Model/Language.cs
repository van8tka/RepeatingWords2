using System.ComponentModel.DataAnnotations.Schema;

namespace RepeatingWords.DataService.Model
{
    [Table("Language")]
    public class Language
    {
        public int Id { get; set; }
        public string NameLanguage { get; set; }
        public int PercentOfLearned { get; set; }
    }
}
