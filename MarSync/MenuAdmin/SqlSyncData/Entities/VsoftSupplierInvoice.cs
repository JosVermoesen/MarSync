namespace MarSync
{
    class VsoftSupplierInvoice
    {
        // COMMON
        public string V033 { get; set; } // doc number V033
        public string A110 { get; internal set; } // A110 in mdv database
        public string V035 { get; set; } // doc date
        public string V066 { get; set; } // booking date
        public string V036 { get; set; } // expire date
        public string V037 { get; set; } // paid already
        public string V038 { get; set; } // bank sheet nr
        public string V249 { get; set; } // total doc in EUR as string
        public decimal? Decv249 { get; set; } // total doc in EUR as decimal
        public string V039 { get; set; } // payment ref
        public string Vs03 { get; set; } // ISO code default EUR
        public string V040 { get; set; } // daycourse
        public decimal? Decv040 { get; set; }
        public string V041 { get; set; } // flag document
        public string V245 { get; set; } // user number
        public string V246 { get; set; } // systemdate        

        // LOOK UP TO DO
        public string RvDm { get; set; }

        // VAT SUPPLIERS
        public string V042 { get; set; } // BOX 55
        public string V043 { get; set; } // BOX 56
        public string V044 { get; set; } // BOX 57
        public string V045 { get; set; } // BOX 59/63
        public string V046 { get; set; } // BOX 81
        public string V047 { get; set; } // BOX 82
        public string V048 { get; set; } // BOX 83
        public string V049 { get; set; } // depts via thirts other than 81-83
        public string V050 { get; set; } // BOX 84
        public string V051 { get; set; } // BOX 85
        public string V052 { get; set; } // BOX 86
        public string V053 { get; set; } // BOX 87
        public string V054 { get; set; } // BOX 88 ptjer than 86/87
        public decimal? Decv042 { get; set; }
        public decimal? Decv043 { get; set; }
        public decimal? Decv044 { get; set; }
        public decimal? Decv045 { get; set; }
        public decimal? Decv046 { get; set; }
        public decimal? Decv047 { get; set; }
        public decimal? Decv048 { get; set; }
        public decimal? Decv049 { get; set; }
        public decimal? Decv050 { get; set; }
        public decimal? Decv051 { get; set; }
        public decimal? Decv052 { get; set; }
        public decimal? Decv053 { get; set; }
        public decimal? Decv054 { get; set; }
        public virtual VsoftSupplier VsoftSupplier { get; set; } // important for ON DELETE CASCADE when Customer is deleted        
        public int VsoftSupplierId { get; internal set; } // A110 in mdv database
    }
}
