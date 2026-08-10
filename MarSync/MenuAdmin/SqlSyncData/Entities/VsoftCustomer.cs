using System.Collections.Generic;

namespace MarSync
{
    class VsoftCustomer
    {
        // MAIN
        public string A110 { get; set; }      // A110 Unique code number        
                                              // [Required]
                                              // [StringLength(12)]
                                              // public string A110 { get; set; }      // A110 Unique code number    
        public string E072 { get; set; }    // paperless J/N

        public string G101 { get; set; }    // voorkeur documenten J/N
        public string G102 { get; set; }    // relatiegeschenk A J/N  
        public string G103 { get; set; }    // relatiegeschenk B J/N  
        public string G104 { get; set; }    // relatiegeschenk C J/N  
        public string G105 { get; set; }    // relatiegeschenk D J/N  
        public string G106 { get; set; }    // relatiegeschenk E J/N  

        public string A10c { get; set; }    // Language code
        public string A104 { get; set; }    // Street
        public string A105 { get; set; }    // Number
        public string A106 { get; set; }    // Box
        public string A107 { get; set; }    // Postal code
        public string A108 { get; set; }    // City
        public string V149 { get; set; }    // ISO Country number
        public string A109 { get; set; }    // Country code (Belgian bPost)
        public string V150 { get; set; }    // ISO Country code ()
        public string Vs03 { get; set; }    // ISO Currency code (EUR)
        public string V161 { get; set; }    // 40-account connection
        public string A161 { get; set; }    // VAT number (BE format)
        public string V404 { get; set; }    // ondernemingsnummer (BE format)
        public string V151 { get; set; }    // co-contractor (1=yes)
        public string V111 { get; set; }    // default VAT code
        public string V254 { get; set; }    // cloudlink (myDocuments)
        public string V255 { get; set; }    // domic. reference/abonnee ref
        public string V256 { get; set; }    // domic. number
        public string A170 { get; set; }    // banc account number (BE old format)
        public string Vs04 { get; set; }    // payment term
        public string Vs05 { get; set; }    // fixed korting (%)
        public string Vs06 { get; set; }    // sorting code +
        public string Vs07 { get; set; }    // number of sheets (invoice)
        public string V225 { get; set; }    // 60-account connection
        public string V227 { get; set; }    // date last visit
        public string V247 { get; set; }    // (local)server guid (myDocuments)
        public string A10a { get; set; }    // main phone number (fixed)
        public string Vs02 { get; set; }    // main fax number
        public string V224 { get; set; }    // main email adress
        public string A123 { get; set; }    // marital status
        public string A124 { get; set; }    // code sex (3= legal person/rechtspersoon)
        public string A121 { get; set; }    // code nationality 
        public string A122 { get; set; }    // optional description nationality
        public string V259 { get; set; }    // IBAN banc account
        public string V260 { get; set; }    // BIC banc account
        public string E070 { get; set; }    // fee active (1=yes)
        public string E071 { get; set; }    // fee percentage
        public string V252 { get; set; }    // quitance forfait/kwijting forfait (1=yes)

        // INSURANCE MAIN
        public string A191 { get; set; }    // number drivers licence
        public string A192 { get; set; }    // type drivers licence
        public string A193 { get; set; }    // date drivers licence
        public string A194 { get; set; }    // driving since
        public string A197 { get; set; }    // profession risc class
        public string _510z { get; set; }   // main driver (1=yes)
        public string A130 { get; set; }    // physical person (1=yes)

        // FIRST IDENT
        public string V301 { get; set; }    // RR number1        
        public string A102 { get; set; }    // Title code
        public string A100 { get; set; }    // Name
        public string A101 { get; set; }    // Forename
        public string V226 { get; set; }    // mobile phone number (main)
        public string V243 { get; set; }    // Phone work


        // SECOND IDENT (PARTNER)
        public string V302 { get; set; }    // RR number2 (partner)
        public string Vs01 { get; set; }    // Title code (partner)
        public string A125 { get; set; }    // Name (partner)
        public string A127 { get; set; }    // Forename (partner)
        public string V002 { get; set; }    // email2 (partner)
        public string V257 { get; set; }    // IBAN banc account2 (partner)
        public string V258 { get; set; }    // BIC banc account2 (partner)
        public string V244 { get; set; }    // Mobile Phone (partner)
        public string V251 { get; set; }    // old banc account (partner)

        public string A120 { get; set; }    // birth date
        public string V201 { get; set; }    // birth date partner
        public string V202 { get; set; }    // birth date kid1
        public string V203 { get; set; }    // birth date kid2
        public string V204 { get; set; }    // birth date kid3
        public string V205 { get; set; }    // birth date kid4
        public string V206 { get; set; }    // birth date kid5
        public string V207 { get; set; }    // forname kid1
        public string V208 { get; set; }    // forname kid2
        public string V209 { get; set; }    // forname kid3
        public string V210 { get; set; }    // forname kid4
        public string V211 { get; set; }    // forname kid5
        public string V245 { get; set; }    // user number
        public string V246 { get; set; }    // system date         
        public string V253 { get; set; }    // SQL sell instruction
        public string Uxxx { get; set; }    // ??
        public string V262 { get; set; }    // ?
        public string V263 { get; set; }    // ?                                            
        public string V407 { get; set; }    // ?

        public virtual ICollection<VsoftContract> VsoftContracts { get; set; }
        public virtual ICollection<VsoftCustomerInvoice> VsoftCustomerInvoices { get; set; }

        public string SelectedInfo
        {
            get
            {
                return $"{A100} {A101} ({A107} - {A108})";
            }
        }
    }
}
