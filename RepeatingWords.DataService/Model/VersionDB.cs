using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RepeatingWords.DataService.Model
{
    [Table("VersionDB")]
    public class VersionDB
    {
        [Key]
        public int Id { get; set; }
        public int VersionNumber { get; set; }
    }
}