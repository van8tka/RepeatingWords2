using System.ComponentModel.DataAnnotations.Schema;

namespace RepeatingWords.DataService.Model
{
    [Table("Dictionary")]
   public class Dictionary
    {      
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
