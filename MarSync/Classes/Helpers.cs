using JRO; // Add this namespace to resolve the 'JRO' reference
using MarSync.Classes.Ademico;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MarSync
{
    public class PeppolDataExtrator
    {
        public class PeppolNlInvoiceData
        {
            public string Id { get; set; }
            public string IssueDate { get; set; }
            public string DueDate { get; set; }
            public string SupplierName { get; set; }
            public string SupplierVatEU { get; set; }
            public string SupplierCompanyID { get; set; }
            public string SupplierEmail { get; set; }
            public string CustomerName { get; set; }
            public string CustomerVatEU { get; set; }
            public string CustomerEmail { get; set; }
            public string PayableAmount { get; set; }
            public string Currency { get; set; }
            public List<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
        }

        public class InvoiceLine
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string LineExtensionAmount { get; set; }
            public string TaxAmount { get; set; } // Added TaxAmount property
        }

        public static class InvoiceExtractor
        {
            public static PeppolNlInvoiceData ExtractInvoiceData(string xml)
            {
                // XNamespace ns = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
                XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
                XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

                var doc = XDocument.Parse(xml);

                var invoice = new PeppolNlInvoiceData
                {
                    Id = doc.Root.Element(cbc + "ID")?.Value,
                    IssueDate = doc.Root.Element(cbc + "IssueDate")?.Value,
                    DueDate = doc.Root.Element(cbc + "DueDate")?.Value,
                    Currency = doc.Root.Element(cbc + "DocumentCurrencyCode")?.Value,
                    PayableAmount = doc.Root.Element(cac + "LegalMonetaryTotal")?.Element(cbc + "PayableAmount")?.Value,
                    SupplierName = doc.Root
                        .Element(cac + "AccountingSupplierParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "PartyName")?
                        .Element(cbc + "Name")?.Value,
                    SupplierVatEU = doc.Root
                        .Element(cac + "AccountingSupplierParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "PartyTaxScheme")?
                        .Element(cbc + "CompanyID")?.Value,
                    SupplierCompanyID = doc.Root
                        .Element(cac + "AccountingSupplierParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "PartyLegalEntity")?
                        .Element(cbc + "CompanyID")?.Value,
                    SupplierEmail = doc.Root
                        .Element(cac + "AccountingSupplierParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "Contact")?
                        .Element(cbc + "ElectronicMail")?.Value,
                    CustomerName = doc.Root
                        .Element(cac + "AccountingCustomerParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "PartyName")?
                        .Element(cbc + "Name")?.Value,
                    CustomerVatEU = doc.Root
                        .Element(cac + "AccountingCustomerParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "PartyTaxScheme")?
                        .Element(cbc + "CompanyID")?.Value,
                    CustomerEmail = doc.Root
                        .Element(cac + "AccountingCustomerParty")?
                        .Element(cac + "Party")?
                        .Element(cac + "Contact")?
                        .Element(cbc + "ElectronicMail")?.Value,
                };

                foreach (var line in doc.Root.Elements(cac + "InvoiceLine"))
                {
                    invoice.Lines.Add(new InvoiceLine
                    {
                        Id = line.Element(cbc + "ID")?.Value,
                        Description = line.Element(cac + "Item")?.Element(cbc + "Description")?.Value,
                        Quantity = line.Element(cbc + "InvoicedQuantity")?.Value,
                        LineExtensionAmount = line.Element(cbc + "LineExtensionAmount")?.Value,
                        TaxAmount = line.Element(cac + "TaxTotal")?.Element(cbc + "TaxAmount")?.Value // Extract TaxAmount
                    });
                }

                return invoice;
            }
        }

    }

    public static class XmlComparer
    {        
        public static bool AreXmlStringsEqual(string xmlA, string xmlB)
        {
            if (xmlA == null || xmlB == null)
                return xmlA == xmlB; // both null = equal

            try
            {
                // Parse both strings into XDocument
                var docA = XDocument.Parse(xmlA, LoadOptions.PreserveWhitespace);
                var docB = XDocument.Parse(xmlB, LoadOptions.PreserveWhitespace);
                                
                // Normalize formatting
                string normA = NormalizeXml(docA);
                string normB = NormalizeXml(docB);

                return string.Equals(normA, normB, StringComparison.Ordinal);
            }
            catch (XmlException)
            {
                // One or both strings are not valid XML
                return false;
            }
        }

        private static string NormalizeXml(XDocument doc)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineChars = "\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
    }
    
    public class MarHelpers
    {

        public static string PartRight(string theString, int theLength)
        {
            try
            {
                if (string.IsNullOrEmpty(theString) || theLength <= 0)
                    return string.Empty;

                int stringLength = theString.Length;
                int start = stringLength - theLength + 1; // VB6 Mid is 1-based
                if (start < 1)
                    return string.Empty;

                int index = start - 1; // C# Substring is 0-based
                if (index >= theString.Length)
                    return string.Empty;

                int available = theString.Length - index;
                int take = theLength < available ? theLength : available;
                return take <= 0 ? string.Empty : theString.Substring(index, take);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string PartLeft(string theString, int theLength)
        {
            try
            {
                if (string.IsNullOrEmpty(theString) || theLength <= 0)
                    return string.Empty;

                int take = theLength < theString.Length ? theLength : theString.Length;
                return take <= 0 ? string.Empty : theString.Substring(0, take);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string PartMid(string theString, int thePosition, int theLength)
        {
            try
            {
                if (string.IsNullOrEmpty(theString) || thePosition < 1 || theLength <= 0)
                    return string.Empty;

                int index = thePosition - 1; // VB6 Mid is 1-based
                if (index >= theString.Length)
                    return string.Empty;

                int available = theString.Length - index;
                int take = theLength < available ? theLength : available;
                return take <= 0 ? string.Empty : theString.Substring(index, take);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ReadUBLInvoice(
            string filePath,
            bool messageBox,
            bool justDocumentId,
            bool justPayRef,
            bool companyIdDocumentIdAndPayRef,
            bool quickInfo)
        {            
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                if (messageBox)
                    MessageBox.Show($"Error loading XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ex.Message;
            }

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            var sb = new StringBuilder();

            // UBL Version
            var ublVersionNode = xmlDoc.SelectSingleNode("//cbc:UBLVersionID", ns);
            sb.AppendLine("UBL VersionID: " + (ublVersionNode?.InnerText ?? "not found"));

            // Document ID
            var invoiceIdNode = xmlDoc.SelectSingleNode("//cbc:ID", ns);
            sb.AppendLine("Document ID: " + (invoiceIdNode?.InnerText ?? "not found"));
            string documentId = invoiceIdNode?.InnerText ?? "";
            if (justDocumentId)
                return documentId;

            // IssueDate
            var issueDateNode = xmlDoc.SelectSingleNode("//cbc:IssueDate", ns);
            sb.AppendLine("IssueDate: " + (issueDateNode?.InnerText ?? "not found"));

            // DueDate
            var dueDateNode = xmlDoc.SelectSingleNode("//cbc:DueDate", ns);
            sb.AppendLine("DueDate: " + (dueDateNode?.InnerText ?? "not found"));

            // InvoiceTypeCode
            var invTypeNode = xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", ns);
            if (invTypeNode != null)
            {
                var invoiceTypeCode = invTypeNode.InnerText;
                var listID = invTypeNode.Attributes?["listID"]?.Value ?? "";
                sb.AppendLine("invoiceTypeCode: " + invoiceTypeCode);
                sb.AppendLine("invoice listID: " + listID);
            }
            else
            {
                if (messageBox)
                    MessageBox.Show("InvoiceTypeCode element not found.");
            }

            // OrderReference
            var orderList = xmlDoc.SelectNodes("//cac:OrderReference", ns);
            if (orderList != null)
            {
                foreach (XmlNode ordNode in orderList)
                {
                    var orderId = ordNode.SelectSingleNode("cbc:ID", ns)?.InnerText ?? "Order ID: not available";
                    sb.AppendLine("Order ID: " + orderId);
                }
            }
            if (messageBox)
                MessageBox.Show(sb.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Supplier info
            var supplierNode = xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party", ns);
            string companyId = "";            
            string partyName = "";
            string streetName = "";
            string placeName = "";
                        
            string customerId = "";
            string customerPartyName = "";
            string customerStreetName = "";
            string customerPlaceName = "";

            if (supplierNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Supplier info");
                msg.AppendLine("-------------");

                string valueToUse = NodeText(supplierNode, "cbc:EndpointID", ns);                

                if (valueToUse.Length == 12 && valueToUse.Contains("BE"))
                {
                    // Get only the VAT part of number
                    valueToUse = PartMid(valueToUse, 3, valueToUse.Length - 2);
                }
                else if (valueToUse.Length == 13 && PartLeft(valueToUse, 2) == "54")
                {
                    // Global Location Number company in Belgium or Luxembourg
                    valueToUse = NodeText(supplierNode, "cac:PartyIdentification/cbc:ID", ns);
                    if (valueToUse.Length == 10)
                    {
                        // ok
                    }
                    else
                    {
                        MessageBox.Show("VAT number? " + valueToUse, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (valueToUse.Length == 13)
                {
                    // Global Location Number of company outside Belgium
                    // MessageBox.Show("Global Company Number Outside Belgium" + Environment.NewLine + Environment.NewLine + valueToUse,
                    //    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valueToUse = NodeText(supplierNode, "cac:PartyIdentification/cbc:ID", ns);
                }

                companyId = valueToUse;
                // companyId = NodeText(supplierNode, "cbc:EndpointID", ns);
                string supplierId = NodeText(supplierNode, "cac:PartyIdentification/cbc:ID", ns);
                partyName = NodeText(supplierNode, "cac:PartyName/cbc:Name", ns);
                streetName = NodeText(supplierNode, "cac:PostalAddress/cbc:StreetName", ns);
                placeName = NodeText(supplierNode, "cac:PostalAddress/cbc:CityName", ns);

                msg.AppendLine("endpointOndernemingsnummer " + companyId);                
                msg.AppendLine("supplierID: " + supplierId);
                msg.AppendLine("tradingName: " + partyName);
                msg.AppendLine("street: " + streetName);
                msg.AppendLine("city: " + placeName);
                msg.AppendLine("postalZone: " + NodeText(supplierNode, "cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("countryCode: " + NodeText(supplierNode, "cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("vatNumber: " + NodeText(supplierNode, "cac:PartyTaxScheme/cbc:CompanyID", ns));
                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (messageBox)
                    MessageBox.Show("No AccountingSupplierParty element found.");
            }

            // Customer info
            var custNode = xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty", ns);
            if (custNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Customer info");
                msg.AppendLine("-------------");

                string customerCompanyId = NodeText(custNode, "cbc:CustomerAssignedAccountID", ns);
                customerId = NodeText(custNode, "cac:Party/cbc:EndpointID", ns);
                customerPartyName = NodeText(custNode, "cac:Party/cac:PartyName/cbc:Name", ns);
                customerStreetName = NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:StreetName", ns);
                customerPlaceName = NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:CityName", ns);

                msg.AppendLine("custAssignedAccountID: " + customerCompanyId);
                msg.AppendLine("custEndpointID: " + customerId);
                msg.AppendLine("custName: " + customerPartyName);
                msg.AppendLine("custStreet: " + customerStreetName);
                msg.AppendLine("custCity: " + customerPlaceName);
                msg.AppendLine("custPostalZone: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("custCountryCode: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("custTaxID: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cbc:CompanyID", ns));
                msg.AppendLine("custTaxScheme: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cac:TaxScheme/cbc:ID", ns));

                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // MessageBox.Show("No AccountingCustomerParty element found.");
            }

            // PaymentMeans
            var pmNodes = xmlDoc.SelectNodes("//cac:PaymentMeans", ns);
            if (pmNodes != null && pmNodes.Count > 0)
            {
                var msg = new StringBuilder();
                msg.AppendLine("PaymentMeans");
                msg.AppendLine("------------");
                foreach (XmlNode pmNode in pmNodes)
                {
                    msg.AppendLine("PaymentMeansCode: " + NodeText(pmNode, "cbc:PaymentMeansCode", ns));
                    string paymentId = NodeText(pmNode, "cbc:PaymentID", ns);

                    // Remove '+' and '/'
                    paymentId = paymentId.Replace("+", "").Replace("/", "");

                    msg.AppendLine("PaymentID: " + paymentId);
                    if (justPayRef)
                        return paymentId;

                    if (companyIdDocumentIdAndPayRef)
                    {
                        return companyId + "|" + documentId + "|" + paymentId;
                    }

                    msg.AppendLine("Payee IBAN: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:ID", ns));
                    msg.AppendLine("Account Name: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:Name", ns));
                    msg.AppendLine("BIC/Branch ID: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cac:FinancialInstitutionBranch/cbc:ID", ns));
                    msg.AppendLine();
                    msg.AppendLine("Card account (if present)");
                    msg.AppendLine("Card Account ID: " + NodeText(pmNode, "cac:CardAccount/cbc:ID", ns));
                    msg.AppendLine("Card Account Name: " + NodeText(pmNode, "cac:CardAccount/cbc:Name", ns));
                    msg.AppendLine();
                    msg.AppendLine("Direct debit mandate (if present)");
                    msg.AppendLine("Mandate ID: " + NodeText(pmNode, "cac:PaymentMandate/cbc:ID", ns));
                    msg.AppendLine("Mandate Date: " + NodeText(pmNode, "cac:PaymentMandate/cbc:PaymentMandateDate", ns));
                }

                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (messageBox)
                    MessageBox.Show("No PaymentMeans element found.");
            }

            // TaxTotal
            var msgTax = new StringBuilder();
            msgTax.AppendLine("TaxTotal");
            msgTax.AppendLine("--------");
            var taxAmountEl = xmlDoc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", ns);
            var currencyID = taxAmountEl?.Attributes?["currencyID"]?.Value ?? "";
            if (taxAmountEl != null && string.IsNullOrEmpty(currencyID))
            {
                if (messageBox)
                    MessageBox.Show("Attribute currencyID is missing on <cbc:TaxAmount>");
            }

            var taxTotals = xmlDoc.SelectNodes("//cac:TaxTotal", ns);
            if (taxTotals != null)
            {
                foreach (XmlNode taxTotalElem in taxTotals)
                {
                    var ttAmount = taxTotalElem.SelectSingleNode("cbc:TaxAmount", ns)?.InnerText ?? "";
                    msgTax.AppendLine($"TaxTotal: {ttAmount} {currencyID}");

                    var subtotals = taxTotalElem.SelectNodes("cac:TaxSubtotal", ns);
                    foreach (XmlNode subElem in subtotals)
                    {
                        msgTax.AppendLine();
                        msgTax.AppendLine("SubDetail");
                        msgTax.AppendLine("TaxableAmount: " + NodeText(subElem, "cbc:TaxableAmount", ns));
                        msgTax.AppendLine("TaxAmount: " + NodeText(subElem, "cbc:TaxAmount", ns));
                        msgTax.AppendLine("Percent: " + NodeText(subElem, "cac:TaxCategory/cbc:Percent", ns) + "%");
                        msgTax.AppendLine();
                    }
                }

                if (messageBox)
                    MessageBox.Show(msgTax.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // LegalMonetaryTotal
            string taxExclusive = "";
            string taxInclusive = "";

            var msgMoney = new StringBuilder();
            msgMoney.AppendLine("LegalMonetaryTotal");
            msgMoney.AppendLine("------------------");
            var moneyTotalEl = xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal", ns);
            if (moneyTotalEl != null)
            {
                taxExclusive = NodeText(moneyTotalEl, "cbc:TaxExclusiveAmount", ns);
                taxInclusive = NodeText(moneyTotalEl, "cbc:TaxInclusiveAmount", ns);

                msgMoney.AppendLine("LineExtensionAmount: " + NodeText(moneyTotalEl, "cbc:LineExtensionAmount", ns));
                msgMoney.AppendLine("TaxExclusiveAmount: " + taxExclusive);
                msgMoney.AppendLine("TaxInclusiveAmount: " + taxInclusive);
                msgMoney.AppendLine("PayableAmount: " + NodeText(moneyTotalEl, "cbc:PayableAmount", ns) + $" ({currencyID})");

                if (messageBox)
                    MessageBox.Show(msgMoney.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // InvoiceLines
            var msgLines = new StringBuilder();
            var invoiceLines = xmlDoc.SelectNodes("//cac:InvoiceLine", ns);
            if (invoiceLines != null)
            {
                foreach (XmlNode lineNode in invoiceLines)
                {
                    var desc = NodeText(lineNode, ".//cbc:Description", ns);
                    var qty = NodeText(lineNode, ".//cbc:InvoicedQuantity", ns);
                    var price = NodeText(lineNode, ".//cbc:PriceAmount", ns);
                    msgLines.AppendLine($"Item: {desc}, Quantity: {qty}, Price: {price}");
                }

                if (messageBox)
                    MessageBox.Show(msgLines.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (quickInfo)
            {
                string quickInfoText =
                "SnelHelp:\n" +
                " - Document ID: " + documentId + "\n" +
                " - Document Datum: " + issueDateNode.InnerText + "\n" +
                " - InvoiceTypeCode: " + invTypeNode.InnerText + "\n\n" +
                " - Info Leverancier:\n " + 
                "   - " + companyId + " - " + partyName + "\n" +
                "   - " + streetName + ", " + placeName + "\n\n" +
                " - Info Klant:\n" +
                "   - " + customerId + " - " + customerPartyName + "\n" +
                "   - " + customerStreetName + ", " + customerPlaceName + "\n\n" +
                " - Exclusief BTW: " + taxExclusive + " " + currencyID + "\n" +
                " - Inclusief BTW: " + taxInclusive + " " + currencyID + "\n";

                return quickInfoText;
            }
            return documentId;
        }

        public static string NodeText(XmlNode parentNode, string xpath, XmlNamespaceManager ns)
        {
            var node = parentNode.SelectSingleNode(xpath, ns);
            return node?.InnerText.Trim() ?? "";
        }

        public static string ReadUBLDate(
            string filePath,
            bool asIssueDate)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            if (asIssueDate)
            {
                // IssueDate
                var issueDateNode = xmlDoc.SelectSingleNode("//cbc:IssueDate", ns);
                string issueDate = issueDateNode?.InnerText ?? "";
                return issueDate;
            }
            else
            {
                // DueDate
                var dueDateNode = xmlDoc.SelectSingleNode("//cbc:DueDate", ns);
                string dueDate = dueDateNode?.InnerText ?? "";
                return dueDate;
            }
        }

        private static int GetIso8601WeekOfYear(DateTime date)
        {
            // ISO 8601 week 1 is the week with the first Thursday of the year.
            // DayOfWeek is 0 for Sunday, 1 for Monday, etc.
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the ISO 8601 week number.
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

        public static string CopyPeppolFileToWeekFolderOnCloudDrive(
            bool isSeller,
            string sourceFolderPathAndFile,
            string cloudFolderPath)
        {
            string prefix = isSeller ? "{V}-" : "{A}-"; // V for Verkoop/Vente, A for Aankoop/Achat
            string documentDate = MarHelpers.ReadUBLDate(sourceFolderPathAndFile, asIssueDate: true);

            DateTime dt = DateTime.ParseExact(documentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int weekNr = GetIso8601WeekOfYear(dt);
            string weekNrStr = weekNr.ToString("D2"); // "50"
            string yearStr = dt.Year.ToString(); // "2025"             

            string cloudFolderPath2 = cloudFolderPath + "\\"
                + yearStr
                + "\\Week" + weekNrStr;


            if (!Directory.Exists(cloudFolderPath2))
                // then create it
                Directory.CreateDirectory(cloudFolderPath2);

            try
            {
                // Copy file to cloud peppol week folder
                string fileName = prefix + Path.GetFileName(sourceFolderPathAndFile);
                string file = Path.Combine(cloudFolderPath2, fileName);
                File.Copy(sourceFolderPathAndFile, file, true);
                return file;
            }
            catch (Exception)
            {
                return "File in Use";
            }
        }

        public static void ExtractPdfFromUblDocument(string ublFilePath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(ublFilePath);

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            var node = xmlDoc.SelectSingleNode("//cbc:EmbeddedDocumentBinaryObject", ns);
            if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
            {
                string base64Data = node.InnerText.Trim();
                byte[] pdfBytes = Convert.FromBase64String(base64Data);

                string pdfFilePath = ublFilePath.Substring(0, ublFilePath.Length - 4) + ".pdf";
                File.WriteAllBytes(pdfFilePath, pdfBytes);

                // Optionally open the PDF
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfFilePath,
                    UseShellExecute = true
                });
            }
        }

        public static async Task<string> GetPublicPeppolRegistrationAsync(
            string query, bool asJson,
            CancellationToken cancellationToken = default)
        {
            var baseUrlJson = "https://directory.peppol.eu/search/1.0/json";
            var requestUri = baseUrlJson + "?q=" + Uri.EscapeDataString("iso6523-actorid-upis:" + query);

            var baseUrlXml = "https://directory.peppol.eu/search/1.0/xml";
            var requestUriXml = baseUrlXml + "?q=" + Uri.EscapeDataString("iso6523-actorid-upis:" + query);

            if (!asJson)
            {
                // Using XML version
                using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUriXml))
                {
                    // Set Accept header
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }

            // Using JSON version
            using (var httpClient = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                // Set Accept header
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }
        public static void SetApiMode(string apiMode)
        {
            if (apiMode == "TESTMODE")
            {
                SharedGlobals.AdemicoApiUrl = MyApiSecrets.testBaseUrl;
                SharedGlobals.AdemicoAccessToken = MyApiSecrets.testAccessToken;
                SharedGlobals.AdemicoUsername = MyApiSecrets.testUsername;
                SharedGlobals.AdemicoPassword = MyApiSecrets.testPassword;
                SharedGlobals.ApiModus = "TESTMODE";
            }            
            else if (apiMode == "PRODUCTIE")
            {
                SharedGlobals.AdemicoApiUrl = MyApiSecrets.prodBaseUrl;
                SharedGlobals.AdemicoAccessToken = MyApiSecrets.prodAccessToken;
                SharedGlobals.AdemicoUsername = MyApiSecrets.prodUsername;
                SharedGlobals.AdemicoPassword = MyApiSecrets.prodPassword;
                SharedGlobals.ApiModus = "PRODUCTIE";
            }
            else
            {
                throw new ArgumentException("Invalid API mode. Use 'TESTMODE' or 'PRODUCTIE'.", nameof(apiMode));
            }
        }

        public static void ResetCompanyGlobals()
        {
            SharedGlobals.ActiveCompany = "";
            SharedGlobals.SetMarntMdvLocation("");
            SharedGlobals.CompanyName = ""; // default values
            SharedGlobals.CompanyAddress = ""; // default values
            SharedGlobals.CompanyPostalCodeAndCity = ""; // default values
            SharedGlobals.CompanyPhoneNumber = ""; // default values
            SharedGlobals.CompanyKBONumber = ""; // default values
            SharedGlobals.CompanyVATNumber = ""; // default values
            SharedGlobals.CompanyIBANNumber = ""; // default values
            SharedGlobals.CompanyBICNumber = ""; // default values
            SharedGlobals.CompanyEmailAddress = ""; // default values
            SharedGlobals.CompanyContactPerson = ""; // default values
            SharedGlobals.CompanyContactEmailAddress = ""; // default values
        }

        public static void SetCompanyGlobals(string selectedCompany)
        {
            SharedGlobals.ActiveCompany = selectedCompany;            

            // Use the provided method to set the value instead of direct assignment
            SharedGlobals.SetMarntMdvLocation("\\" + selectedCompany + "\\marnt.mdv");
            string jrFile = GetHighestCounterTable(SharedGlobals.MarntMdvLocation, "jr");

            string connStr = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
            OleDbConnection conn = new OleDbConnection
            {
                ConnectionString = connStr
            };
            conn.Open();

            string sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's046'";
            OleDbCommand cmd = new OleDbCommand
            {
                CommandText = sqlQuery,
                Connection = conn
            };
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyName = reader["v217"].ToString();
                SharedGlobals.CompanyName = companyName;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's047'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyAddress = reader["v217"].ToString();
                SharedGlobals.CompanyAddress = companyAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's048'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyPostalCodeAndCity = reader["v217"].ToString();
                SharedGlobals.CompanyPostalCodeAndCity = companyPostalCodeAndCity;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's049'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyPhoneNumber = reader["v217"].ToString();
                SharedGlobals.CompanyPhoneNumber = companyPhoneNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's292'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyKBONumber = reader["v217"].ToString();
                SharedGlobals.CompanyKBONumber = companyKBONumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's051'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyVATNumber = reader["v217"].ToString();
                SharedGlobals.CompanyVATNumber = companyVATNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's293'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyIBANNumber = reader["v217"].ToString();
                SharedGlobals.CompanyIBANNumber = companyIBANNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's294'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyBICNumber = reader["v217"].ToString();
                SharedGlobals.CompanyBICNumber = companyBICNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's295'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyEmailAddress = reader["v217"].ToString();
                SharedGlobals.CompanyEmailAddress = companyEmailAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's052'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyContactPerson = reader["v217"].ToString();
                SharedGlobals.CompanyContactPerson = companyContactPerson;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's050'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyContactEmailAddress = reader["v217"].ToString();
                SharedGlobals.CompanyContactEmailAddress = companyContactEmailAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's060'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string accountingContactPerson = reader["v217"].ToString();
                SharedGlobals.AccountingContactPerson = accountingContactPerson;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's061'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string accountingContactEmailAddress = reader["v217"].ToString();
                SharedGlobals.AccountingContactEmailAddress = accountingContactEmailAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's065'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string accountingReference = reader["v217"].ToString();
                SharedGlobals.AccountingReference = accountingReference;
            }
            reader.Close();
            conn.Close();
        }

        private static string GetHighestCounterTable(string location, string filter)
        {
            // Change provider depending on Access version:
            // For .mdb (Jet)
            string foundTable = string.Empty;
            string connStr = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + location;

            OleDbConnection conn = new OleDbConnection(connStr);
            conn.Open();

            // Retrieve schema for tables only
            DataTable tables = conn.GetSchema("Tables");

            foreach (DataRow row in tables.Rows)
            {
                // Use null-coalescing operator to handle potential null values
                string tableName = row["TABLE_NAME"]?.ToString() ?? string.Empty;
                string tableType = row["TABLE_TYPE"]?.ToString() ?? string.Empty;

                // Filter out system/internal tables if needed
                if (tableType == "TABLE")
                {
                    if (tableName.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
                    {
                        foundTable = tableName;
                    }
                }
            }
            conn.Close();
            return foundTable;  // Return empty string if no matching table is found
        }
    }

    public class MarDataFilesHelper
    {
        public static string ZipFolderToCloudDrive(string sourceFolderPath, string cloudFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
                throw new DirectoryNotFoundException($"Source folder not found: {sourceFolderPath}");

            if (!Directory.Exists(cloudFolderPath))
                // then create it
                Directory.CreateDirectory(cloudFolderPath);

            try
            {
                string zipFileName = $"{Path.GetFileName(sourceFolderPath)}_backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                string tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);
                string destZipPath = Path.Combine(cloudFolderPath, zipFileName);

                // Create zip in temp directory
                if (File.Exists(tempZipPath))
                    File.Delete(tempZipPath);

                ZipFile.CreateFromDirectory(sourceFolderPath, tempZipPath, CompressionLevel.Optimal, false);

                // Copy to OneDrive sync folder
                File.Copy(tempZipPath, destZipPath, true);

                return destZipPath;
            }
            catch (Exception)
            {
                return "Bedrijfsdata is nog in gebruik voor dit bedrijf. Zip Backup zonder succes. Eerst marIntegraal afsluiten a.u.b.";
            }
        }

        public static string MovePeppolFileToCloudDrive(string sourceFolderPathAndFile, string cloudFolderPath)
        {            
            if (!Directory.Exists(cloudFolderPath))
                // then create it
                Directory.CreateDirectory(cloudFolderPath);

            try
            {
                // Move file to OneDrive sync folder
                string fileName = Path.GetFileName(sourceFolderPathAndFile);
                string file = Path.Combine(cloudFolderPath, fileName);
                if (File.Exists(file))
                    File.Delete(file);
                File.Move(sourceFolderPathAndFile, file);
                return file;
            }
            catch (Exception)
            {
                return "File in Use";
            }
        }
    }

    public class DatabaseHelper
    {
        public static string CompactAccessDatabase(string databasePath)
        {
            if (!File.Exists(databasePath))
                return $"Database file not found: {databasePath}";
            string tempDatabasePath = Path.Combine(Path.GetDirectoryName(databasePath), "temp_compact.mdb");
            // Use JRO to compact the database
            var jro = new JetEngine(); // Ensure the JRO namespace is correctly referenced
            jro.CompactDatabase($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};", $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={tempDatabasePath};Jet OLEDB:Engine Type=5");
            // Replace original database with compacted one
            File.Delete(databasePath);
            File.Move(tempDatabasePath, databasePath);
            return "Database compacted successfully.";
        }
    }

    namespace Camt053
    {
        // For mapping XML structure of Camt.053

        // Root <Document>
        [XmlRoot("Document", Namespace = "urn:iso:std:iso:20022:tech:xsd:camt.053.001.02")]
        public class Camt053Document
        {
            [XmlElement("BkToCstmrStmt")]
            public BankToCustomerStatement BkToCstmrStmt { get; set; }
        }

        public class BankToCustomerStatement
        {
            [XmlElement("GrpHdr")]
            public GroupHeader GrpHdr { get; set; }

            [XmlElement("Stmt")]
            public Statement Stmt { get; set; }
        }

        public class GroupHeader
        {
            [XmlElement("MsgId")]
            public string MsgId { get; set; }

            [XmlElement("CreDtTm")]
            public DateTime CreDtTm { get; set; }
        }

        public class Statement
        {
            [XmlElement("Id")]
            public string Id { get; set; }

            [XmlElement("ElctrncSeqNb")]
            public int ElctrncSeqNb { get; set; }

            [XmlElement("CreDtTm")]
            public DateTime CreDtTm { get; set; }

            [XmlElement("FrToDt")]
            public FromToDateTime FrToDt { get; set; }

            [XmlElement("Acct")]
            public Account Acct { get; set; }

            [XmlElement("Bal")]
            public List<Balance> Bal { get; set; }

            [XmlElement("TxsSummry")]
            public TransactionsSummary TxsSummry { get; set; }

            [XmlElement("Ntry")]
            public List<Entry> Ntry { get; set; }
        }

        public class FromToDateTime
        {
            [XmlElement("FrDtTm")]
            public DateTime FrDtTm { get; set; }

            [XmlElement("ToDtTm")]
            public DateTime ToDtTm { get; set; }
        }

        public class Account
        {
            [XmlElement("Id")]
            public AccountId Id { get; set; }

            [XmlElement("Tp")]
            public AccountType Tp { get; set; }

            [XmlElement("Ccy")]
            public string Ccy { get; set; }

            [XmlElement("Ownr")]
            public Party Ownr { get; set; }

            [XmlElement("Svcr")]
            public Servicer Svcr { get; set; }
        }

        public class AccountId
        {
            [XmlElement("IBAN")]
            public string IBAN { get; set; }
        }

        public class AccountType
        {
            [XmlElement("Prtry")]
            public string Prtry { get; set; }
        }

        public class Party
        {
            [XmlElement("Nm")]
            public string Nm { get; set; }
        }

        public class Servicer
        {
            [XmlElement("FinInstnId")]
            public FinancialInstitutionId FinInstnId { get; set; }
        }

        public class FinancialInstitutionId
        {
            [XmlElement("BIC")]
            public string BIC { get; set; }
        }

        public class Balance
        {
            [XmlElement("Tp")]
            public BalanceType Tp { get; set; }

            [XmlElement("Amt")]
            public Amount Amt { get; set; }

            [XmlElement("CdtDbtInd")]
            public string CdtDbtInd { get; set; }

            [XmlElement("Dt")]
            public BalanceDate Dt { get; set; }
        }

        public class BalanceType
        {
            [XmlElement("CdOrPrtry")]
            public CodeOrProprietary CdOrPrtry { get; set; }
        }

        public class CodeOrProprietary
        {
            [XmlElement("Cd")]
            public string Cd { get; set; }

            [XmlElement("Prtry")]
            public string Prtry { get; set; }
        }

        public class Amount
        {
            [XmlAttribute("Ccy")]
            public string Ccy { get; set; }

            [XmlText]
            public decimal Value { get; set; }
        }

        public class BalanceDate
        {
            // <Dt><Dt>2026-03-14</Dt></Dt> in your XML
            [XmlElement("Dt")]
            public DateTime Dt { get; set; }
        }

        public class TransactionsSummary
        {
            [XmlElement("TtlNtries")]
            public SummaryEntries TtlNtries { get; set; }

            [XmlElement("TtlCdtNtries")]
            public SummaryEntries TtlCdtNtries { get; set; }

            [XmlElement("TtlDbtNtries")]
            public SummaryEntries TtlDbtNtries { get; set; }
        }

        public class SummaryEntries
        {
            [XmlElement("NbOfNtries")]
            public int NbOfNtries { get; set; }

            [XmlElement("Sum")]
            public decimal Sum { get; set; }

            [XmlElement("TtlNetNtryAmt")]
            public decimal? TtlNetNtryAmt { get; set; }

            [XmlElement("CdtDbtInd")]
            public string CdtDbtInd { get; set; }
        }

        public class Entry
        {
            [XmlElement("NtryRef")]
            public string NtryRef { get; set; }

            [XmlElement("Amt")]
            public Amount Amt { get; set; }

            [XmlElement("CdtDbtInd")]
            public string CdtDbtInd { get; set; }

            [XmlElement("Sts")]
            public string Sts { get; set; }

            [XmlElement("BookgDt")]
            public SimpleDate BookgDt { get; set; }

            [XmlElement("ValDt")]
            public SimpleDate ValDt { get; set; }

            [XmlElement("AcctSvcrRef")]
            public string AcctSvcrRef { get; set; }

            [XmlElement("BkTxCd")]
            public BankTransactionCode BkTxCd { get; set; }

            [XmlElement("NtryDtls")]
            public EntryDetails NtryDtls { get; set; }
        }

        public class SimpleDate
        {
            [XmlElement("Dt")]
            public DateTime Dt { get; set; }
        }

        public class BankTransactionCode
        {
            [XmlElement("Prtry")]
            public ProprietaryCode Prtry { get; set; }
        }

        public class ProprietaryCode
        {
            [XmlElement("Cd")]
            public string Cd { get; set; }

            [XmlElement("Issr")]
            public string Issr { get; set; }
        }

        public class EntryDetails
        {
            [XmlElement("Btch")]
            public Batch Btch { get; set; }

            [XmlElement("TxDtls")]
            public List<TransactionDetails> TxDtls { get; set; }
        }

        public class Batch
        {
            [XmlElement("NbOfTxs")]
            public int NbOfTxs { get; set; }

            [XmlElement("TtlAmt")]
            public Amount TtlAmt { get; set; }

            [XmlElement("CdtDbtInd")]
            public string CdtDbtInd { get; set; }
        }

        public class TransactionDetails
        {
            [XmlElement("Refs")]
            public References Refs { get; set; }

            [XmlElement("AmtDtls")]
            public AmountDetails AmtDtls { get; set; }

            [XmlElement("BkTxCd")]
            public BankTransactionCode BkTxCd { get; set; }

            [XmlElement("RltdPties")]
            public RelatedParties RltdPties { get; set; }

            [XmlElement("RltdAgts")]
            public RelatedAgents RltdAgts { get; set; }

            [XmlElement("RmtInf")]
            public RemittanceInformation RmtInf { get; set; }
        }

        public class References
        {
            [XmlElement("AcctSvcrRef")]
            public string AcctSvcrRef { get; set; }
        }

        public class AmountDetails
        {
            [XmlElement("TxAmt")]
            public TransactionAmount TxAmt { get; set; }
        }

        public class TransactionAmount
        {
            [XmlElement("Amt")]
            public Amount Amt { get; set; }
        }

        public class RelatedParties
        {
            [XmlElement("Cdtr")]
            public Party Cdtr { get; set; }

            [XmlElement("CdtrAcct")]
            public CreditorAccount CdtrAcct { get; set; }
        }

        public class CreditorAccount
        {
            [XmlElement("Id")]
            public AccountId Id { get; set; }
        }

        public class RelatedAgents
        {
            [XmlElement("CdtrAgt")]
            public CreditorAgent CdtrAgt { get; set; }
        }

        public class CreditorAgent
        {
            [XmlElement("FinInstnId")]
            public FinancialInstitutionId FinInstnId { get; set; }
        }

        public class RemittanceInformation
        {
            [XmlElement("Strd")]
            public StructuredRemittance Strd { get; set; }
        }

        public class StructuredRemittance
        {
            [XmlElement("CdtrRefInf")]
            public CreditorReferenceInformation CdtrRefInf { get; set; }
        }

        public class CreditorReferenceInformation
        {
            [XmlElement("Tp")]
            public ReferenceType Tp { get; set; }

            [XmlElement("Ref")]
            public string Ref { get; set; }
        }

        public class ReferenceType
        {
            [XmlElement("CdOrPrtry")]
            public CodeOrProprietary CdOrPrtry { get; set; }

            [XmlElement("Issr")]
            public string Issr { get; set; }
        }
    }
}







