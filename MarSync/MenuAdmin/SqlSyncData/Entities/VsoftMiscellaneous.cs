using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    [Table("vsoft_Miscellaneouses")]
    class VsoftMiscellaneous
    {
        public int Id { get; set; }
        [Column("v004")]
        [StringLength(13)]
        public string V004 { get; set; }
        [Column("v005")]
        [StringLength(20)]
        public string V005 { get; set; }
        [Column("MEMO")]
        public string Memo { get; set; }
        [StringLength(12)]
        public string A000 { get; set; }
        public int? RvID { get; set; }
    }
}
