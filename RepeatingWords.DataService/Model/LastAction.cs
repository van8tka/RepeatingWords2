using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepeatingWords.DataService.Model
{
    [Table("LastAction")]
    public class LastAction
    {
        [Key]
        public int Id { get; set; }          
        public int IdDictionary { get; set; }   
        public int IdWord { get; set; }
        public bool FromRus { get; set; }
    }
}
