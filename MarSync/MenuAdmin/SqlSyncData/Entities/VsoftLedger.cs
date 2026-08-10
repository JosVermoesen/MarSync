using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    class VsoftLedger
    {
        public int Id { get; set; }
        [StringLength(7)]
        public string V019 { get; set; }
        [StringLength(15)]
        public string V070 { get; set; }
        [StringLength(13)]
        public string V034 { get; set; }
        [StringLength(8)]
        public string V066 { get; set; }
        [StringLength(11)]
        public string V033 { get; set; }
        [StringLength(8)]
        public string V038 { get; set; }
        [StringLength(8)]
        public string V035 { get; set; }
        [StringLength(35)]
        public string V067 { get; set; }
        [StringLength(12)]
        public string V068 { get; set; }
        [StringLength(7)]
        public string V069 { get; set; }
        [StringLength(1)]
        public string V041 { get; set; }
        [StringLength(50)]
        public string V249 { get; set; }
        [StringLength(12)]
        public string V248 { get; set; }
        [StringLength(50)]
        public string V245 { get; set; }
        [StringLength(50)]
        public string V246 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece068 { get; set; }
        public string V102 { get; set; }
        public virtual VsoftLedgerAccount VsoftLedgerAccount { get; set; }  // important for ON DELETE CASCADE when Customer is deleted 
        public int VsoftLedgerAccountId { get; internal set; } // V019 in mdv database
    }
}
