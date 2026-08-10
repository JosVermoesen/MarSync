using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    class VsoftYearSetting
    {
        public int Id { get; set; }
        [Required]
        [Column("v071")]
        [StringLength(5)]
        public string V071 { get; set; }
        [Required]
        [Column("v217")]
        [StringLength(50)]
        public string V217 { get; set; }
        [Required]
        [Column("bkjr")]
        [StringLength(4)]
        public string Bkjr { get; set; }
        public int? RvID { get; set; }
    }
}
