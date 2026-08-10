using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    class VsoftLedgerAccount
    {
        [Required]
        [StringLength(7)]
        public string V019 { get; set; }
        [Required]
        [StringLength(40)]
        public string V020 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Dece022 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece023 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece024 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece025 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece026 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece027 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece028 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece029 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece030 { get; set; }
        [Column(TypeName = "money")]
        public decimal? Dece031 { get; set; }
        [StringLength(50)]
        public string V021 { get; set; }
        [StringLength(1)]
        public string V032 { get; set; }
        [StringLength(2)]
        public string V216 { get; set; }        
        public virtual ICollection<VsoftLedger> VsoftLedgers { get; set; }        
    }
}
