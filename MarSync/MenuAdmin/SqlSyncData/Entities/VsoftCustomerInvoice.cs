using System.ComponentModel.DataAnnotations;

namespace MarSync
{
    class VsoftCustomerInvoice
    {
        // COMMON                
        public string V033 { get; set; } // doc number V033        
        public string A110 { get; set; } // 'K'+ A110 in mdv database        
        public string V035 { get; set; } // doc date        
        public string V066 { get; set; } // booking date                                         
        public string V036 { get; set; } // expire date        
        public string V037 { get; set; } // paid already        
        public string V038 { get; set; } // bank sheet nr        
        public string V249 { get; set; } // total doc in EUR
        public decimal Decv249 { get; set; }
        public string V039 { get; set; } // payment ref
        public string Vs03 { get; set; } // ISO code default EUR
        public string V040 { get; set; } // daycourse
        public decimal Decv040 { get; set; }
        public string V041 { get; set; } // flag document        
        public string V245 { get; set; } // user number        
        public string V246 { get; set; } // systemdate        

        // INSURANCE EXTRAS        
        public string A000 { get; set; } // contract nr        
        public string B010 { get; set; } // premium to pay        
        public decimal DecB010 { get; set; }
        public string B014 { get; set; } // commission        
        public decimal DecB014 { get; set; }
        public string B090 { get; set; } // premium to return        
        public decimal DecB090 { get; set; }
        public string B094 { get; set; } // commission -        
        public decimal DecB094 { get; set; }
        public string V065 { get; set; } // amount damage paid         
        public decimal Decv065 { get; set; }
        public string E069 { get; set; } // Admin cost fixed amount

        // TODO!!!!!!!!!!!
        public string Dece069 { get; set; }
        public string E071 { get; set; } // Fee %        
        public string E072 { get; set; } // Fee amount in EUR

        // VAT EXTRAS        
        public string V055 { get; set; } // BOX 00        
        public decimal Decv055 { get; set; }
        public string V056 { get; set; } // BOX 01        
        public decimal Decv056 { get; set; }
        public string V057 { get; set; } // BOX 02        
        public decimal Decv057 { get; set; }
        public string V058 { get; set; } // BOX 03        
        public decimal Decv058 { get; set; }
        public string V059 { get; set; } // BOX 45        
        public decimal Decv059 { get; set; }
        public string V060 { get; set; } // BOX 46        
        public decimal Decv060 { get; set; }
        public string V061 { get; set; } // BOX 47        
        public decimal Decv061 { get; set; }
        public string V062 { get; set; } // BOX 48        
        public decimal Decv062 { get; set; }
        public string V063 { get; set; } // BOX 49        
        public decimal Decv063 { get; set; }
        public string V064 { get; set; } // BOX 54/64 VAT AMOUNT        
        public decimal Decv064 { get; set; }
        public string RvDm { get; set; }
        public string RvXmltb2 { get; set; }
        public string V405 { get; set; } // sent certificate        
        public string V406 { get; set; } // sent invoice        

        public int VsoftCustomerId { get; internal set; } // A110 in mdv database
        public virtual VsoftCustomer VsoftCustomer { get; set; } // important for ON DELETE CASCADE when Customer is deleted                
    }
}
