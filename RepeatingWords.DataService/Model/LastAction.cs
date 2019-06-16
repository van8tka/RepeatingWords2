using System;
using System.Collections.Generic;
using System.Text;

namespace RepeatingWords.DataService.Model
{
    [Table("LastAction")]
    public class LastAction
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int IdDictionary { get; set; }
        public int IdWord { get; set; }
        public bool FromRus { get; set; }
    }
}
