using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepeatingWords.DataService.Model
{
    [Table("Words")]
    public class Words
    {     
        [Key]
        public int Id { get; set; }    
        public int IdDictionary { get; set; }
        public string RusWord { get; set; }
        public string EngWord { get; set; }
        public string Transcription { get; set; }
    }
}
