using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    class VsoftProduct
    {
        [Required]
        [StringLength(13)]
        public string V102 { get; set; } // product number
        
        [StringLength(40)]
        public string V105 { get; set; } // product name
        
        [StringLength(40)]
        public string V221 { get; set; }
        
        [StringLength(1)]
        public string V106 { get; set; }
        
        [StringLength(10)]
        public string V107 { get; set; }
        
        [StringLength(1)]
        public string V108 { get; set; }
        
        [StringLength(3)]
        public string V110 { get; set; }
        
        [StringLength(1)]
        public string V111 { get; set; }
        
        [StringLength(1)]
        public string V168 { get; set; }
        
        [StringLength(3)]
        public string V169 { get; set; }
        
        [StringLength(15)]
        public string V112 { get; set; }
        
        [StringLength(15)]
        public string V113 { get; set; }
        
        [StringLength(10)]
        public string V115 { get; set; }
        
        [StringLength(7)]
        public string V116 { get; set; }
        
        [StringLength(7)]
        public string V117 { get; set; }
        
        [StringLength(7)]
        public string V118 { get; set; }
        
        [StringLength(10)]
        public string V114 { get; set; }
        
        [StringLength(10)]
        public string V119 { get; set; }
        
        [StringLength(10)]
        public string V120 { get; set; }
        
        [StringLength(15)]
        public string V121 { get; set; }
        
        [StringLength(15)]
        public string V122 { get; set; }
        
        [StringLength(15)]
        public string V123 { get; set; }
        
        [StringLength(10)]
        public string V109 { get; set; }
        
        [StringLength(8)]
        public string V103 { get; set; }
        
        [StringLength(20)]
        public string V104 { get; set; }
        
        [StringLength(12)]
        public string V124 { get; set; }
        
        [StringLength(1)]
        public string V125 { get; set; }
        
        [StringLength(30)]
        public string V001 { get; set; }
        
        [StringLength(255)]
        public string V002 { get; set; }
        
        [StringLength(50)]
        public string V249 { get; set; }
        
        [StringLength(50)]
        public string E112 { get; set; }
        
        [StringLength(50)]
        public string E113 { get; set; }
        
        [StringLength(50)]
        public string E121 { get; set; }
        
        [StringLength(50)]
        public string E122 { get; set; }
        
        [StringLength(50)]
        public string E123 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Dece112 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Dece113 { get; set; }
        
        [Column(TypeName = "money")]
        public decimal? Dece121 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Dece122 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Dece123 { get; set; }

        [StringLength(50)]
        public string V261 { get; set; }
        public int? RvID { get; set; }
    }
}
