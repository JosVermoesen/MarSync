namespace MarSync
{
    class VsoftTelebibContract
    {
        public int Id { get; set; }
        public string Mij { get; set; }        
        public string MemoTb2 { get; set; }
        public string DocType { get; set; }

        public string VsoftContractId { get; internal set; }
        public virtual VsoftContract VsoftContract { get; set; } // important for ON DELETE CASCADE when Contract is deleted        
    }
}
