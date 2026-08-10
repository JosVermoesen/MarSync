using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarSync
{
    class VsoftCorrespondence
    {
        public int Id { get; set; }
        [Column("v126")]
        [StringLength(1)]
        public string V126 { get; set; }
        [Column("v127")]
        [StringLength(1)]
        public string V127 { get; set; }
        [StringLength(12)]
        public string A110 { get; set; }
        [Column("v128", TypeName = "datetime")]
        public DateTime? V128 { get; set; }
        [Column("v129")]
        [StringLength(40)]
        public string V129 { get; set; }
        [Column("v130", TypeName = "datetime")]
        public DateTime? V130 { get; set; }
        [Column("v131")]
        [StringLength(1)]
        public string V131 { get; set; }
        [Column("v133")]
        [StringLength(13)]
        public string V133 { get; set; }
        [Column("v134")]
        [StringLength(13)]
        public string V134 { get; set; }
        [Column("v132")]
        public string V132 { get; set; }
        [Column("v172")]
        [StringLength(2)]
        public string V172 { get; set; }
        [Column("bstndBLOB", TypeName = "image")]
        public byte[] BstndBlob { get; set; }
        [Column("bstndNaam")]
        [StringLength(255)]
        public string BstndNaam { get; set; }
        [Column("typeZending")]
        [StringLength(5)]
        public string TypeZending { get; set; }
    }
}
