using System.ComponentModel.DataAnnotations;

namespace MarSync
{
    class VsoftProductGroup
    {
        public int Id { get; set; }
        [StringLength(60)]
        public string GroepsNaam { get; set; }
        public string GroepItems { get; set; }
        public int? RvID { get; set; }
    }
}
