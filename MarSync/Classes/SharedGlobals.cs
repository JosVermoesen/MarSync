namespace MarSync
{
    public static class SharedGlobals
    {
        static SharedGlobals()
        {
            DbJetProvider = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=";            
            UserTestGuid = "";
            UserProdGuid = "";

            MimDataLocation = "";
            MarntMdvLocation = "";
            IsAdmin = false; // default to false
            UserCanToggleTabs = false; // default to false
            LastSelectedCompany = "";

            ActiveCompany = "";
            CompanyName = ""; // value s046 in marnt.mdv file is Company Name
            CompanyAddress = ""; // value s047 in marnt.mdv file is Company Address
            CompanyPostalCodeAndCity = ""; // value s048 in marnt.mdv file is Company Postal Code and City
            CompanyPhoneNumber = ""; // value s049 in marnt.mdv file is Company Phone Number
            CompanyKBONumber = ""; // value s292 in marnt.mdv file is Company KBO Number
            CompanyVATNumber = ""; // value s051 in marnt.mdv file is Company VAT Number
            CompanyIBANNumber = ""; // value s293 in marnt.mdv file is Company IBAN Number
            CompanyBICNumber = ""; // value s294 in marnt.mdv file is Company BIC Number
            CompanyEmailAddress = ""; // value s295 in marnt.mdv file is Company Email Address
            CompanyContactPerson = ""; // value s052 in marnt.mdv file is Company Contact Person
            CompanyContactEmailAddress = ""; // value s050 in marnt.mdv file is Company Contact Email Address
            AccountingContactPerson = ""; // value s060 in marnt.mdv file is Accounting Contact Person
            AccountingContactEmailAddress = ""; // value s061 in marnt.mdv file is Accounting Contact Email Address
            AccountingReference = ""; // value s065 in marnt.mdv file is Accounting Reference

            MarntCloudLocation = ""; // default values
            MarntCLoudArchiveLocation = ""; // default values
            MarntCloudMarioLocation = ""; // default values
            CloudCompanyPeppolArchiveLocation = ""; // default values

            PeppolOutFiles = 0;

            ApiModus = "TESTMODE";
            AdemicoApiUrl = "";
            AdemicoAccessToken = "";
            AdemicoUsername = "";
            AdemicoPassword = "";

            MdvSettings2025 = "";
            LocalSQLSetting2025 = "";
            HostedSQLSetting2025 = "";

            RequestCounter = 0;

            NotificationsSenderALL = "";
            NotificationsReceiverALL = "";

            NotificationsServerALL = "";
        } // default values
        public static string AccountingContactPerson { get; set; } // default values
        public static string AccountingContactEmailAddress { get; set; } // default values
        public static string AccountingReference { get; set; } // default values
        public static string NotificationsServerALL { get; set; } // default values
        public static string NotificationsSenderALL { get; set; } // default values
        public static string NotificationsReceiverALL { get; set; } // default values
        public static string LastSelectedCompany { get; set; } // default values
        public static bool UserCanToggleTabs { get; set; } // default values
        public static int RequestCounter { get; set; } // default values
        public static string UserTestGuid { get; set; } // default values
        public static string UserProdGuid { get; set; } // default values
        public static string MdvSettings2025 { get; set; } = ""; // default values
        public static string LocalSQLSetting2025 { get; set; } = ""; // default values
        public static string HostedSQLSetting2025 { get; set; } = ""; // default values
        public static string ApiModus { get; set; } = "";
        public static string AdemicoApiUrl { get; set; } = "";
        public static string AdemicoAccessToken { get; set; } = "";
        public static string AdemicoUsername { get; set; } = "";
        public static string AdemicoPassword { get; set; } = "";

        public static string CloudCompanyPeppolArchiveLocation { get; set; } = ""; // default values
        public static string MarntCloudMarioLocation { get; set; } = ""; // default values
        public static string MarntCLoudArchiveLocation { get; set; } = ""; // default values
        public static string MarntCloudLocation { get; set; } = ""; // default values
        public static int PeppolOutFiles { get; set; } = 0; // default values
        public static string CompanyName { get; set; } = ""; // default values
        public static string CompanyAddress { get; set; } = ""; // default values
        public static string CompanyPostalCodeAndCity { get; set; } = ""; // default values
        public static string CompanyPhoneNumber { get; set; } = ""; // default values
        public static string CompanyKBONumber { get; set; } = ""; // default values
        public static string CompanyVATNumber { get; set; } = ""; // default values
        public static string CompanyIBANNumber { get; set; } = ""; // default values
        public static string CompanyBICNumber { get; set; } = ""; // default values
        public static string CompanyEmailAddress { get; set; } = ""; // default values
        public static string CompanyContactPerson { get; set; } = ""; // default values
        public static string CompanyContactEmailAddress { get; set; } = ""; // default values

        public static bool IsAdmin { get; set; } = true; // default to true                        
        public static string DbJetProvider { get; }
        public static string MimDataLocation { get; private set; }
        public static string ActiveCompany { get; set; } = ""; // default values
        public static void SetMimDataLocation(string newString)
        {
            MimDataLocation = newString;
        }
        public static string MarntMdvLocation { get; private set; }
        public static void SetMarntMdvLocation(string newString)
        {
            MarntMdvLocation = newString;
        }
    }
}
