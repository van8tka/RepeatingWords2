using System;
using System.Collections.Generic;
using System.Text;

namespace RepeatingWords.DataService.Model
{
    [Table("Words")]
    public class Words
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdDictionary { get; set; }
        public string RusWord { get; set; }
        public string EngWord { get; set; }
        public string Transcription { get; set; }
    }
}
