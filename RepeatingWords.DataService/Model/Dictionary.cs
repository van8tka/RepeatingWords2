using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RepeatingWords.DataService.Model
{
    [Table("Dictionary")]
   public class Dictionary
    {         
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
