namespace MarSync
{
    class VsoftSupplier
    {
        public string A110 { get; set; } // codenumber (A110)     
        public string A102 { get; set; } // Title code
        public string A100 { get; set; } // Name 1
        public string Vs01 { get; set; } // Title code 2
        public string A125 { get; set; } // Name 2
        public string A10c { get; set; } // Language code
        public string A104 { get; set; } // Street
        public string A105 { get; set; } // Number
        public string A106 { get; set; } // Box
        public string A107 { get; set; } // Postal code
        public string A108 { get; set; } // Place
        public string V149 { get; set; } // ISO Country NUMBER
        public string A109 { get; set; } // Country code Postoffice
        public string V150 { get; set; } // ISO Country CODE
        public string Vs03 { get; set; } // ISO Currency code
        public string A10a { get; set; } // Phone number
        public string Vs02 { get; set; } // Fax
        public string V224 { get; set; } // Email 

        // e072 paperless
        public string V163 { get; set; } // connection for stock       
        public string V016 { get; set; } // connection for 6-costaccount
        public string V161 { get; set; } // connection for 44-account        
        public string A161 { get; set; } // VAT number (BE format)
        public string V404 { get; set; } // ondernemingsnummer (BE format)
        public string A170 { get; set; } // belgian fin. account old format
        public string V259 { get; set; } // fin. account IBAN format
        public string V260 { get; set; } // fin. account BIC
        public string A400 { get; set; } // fixed ref/agent number
        public string V015 { get; set; } // registration number
        public string V151 { get; set; } // medecontractant
        public string V111 { get; set; } // default VAT cod
        public string Vs04 { get; set; } // payment delay days
        public string V017 { get; set; } // aardcode gestruct. mededeling
        public string V018 { get; set; } // streefomzet
        public string V001 { get; set; } // Picture
        public string V002 { get; set; } // Video file
        public string V226 { get; set; } // Mobile Phone number
        public string V227 { get; set; } // Date last visit
        public string V247 { get; set; } // Opmerkingen
        public string V254 { get; set; } // SQL aankoopverrichting
        public decimal? Decv018 { get; set; } // streefomzet
        public string V255 { get; set; } // ?
        public string V256 { get; set; } // ?
        public string V262 { get; set; } // ?                
    }
}
