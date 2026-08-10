using MailKit.Net.Smtp;
using MarSync;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;

//var files = Directory.GetFiles(@"C:\Peppol\Export\Week", "*.xml");

//PeppolMailSender.SendWeeklyPeppolMail(
//    smtpServer: "smtp.telenet.be",
//    smtpPort: 587,
//    enableSsl: true,
//    smtpUser: "info@bedrijf.be",
//    smtpPassword: "wachtwoord",
//    fromEmail: "info@bedrijf.be",
//    fromName: "Vsoft 1985",
//    customerFileNumber: "VF1985",
//    accountantEmail: "boekhouding@kantoor.be",
//    accountantName: "Sarah",
//    peppolFiles: files,
//    log: Console.WriteLine
//);

namespace Vsoft.Mail
{
    public static class PeppolMailSender
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);

                // Extra domain checks: must have at least one dot and 2+ char TLD
                string host = mailAddress.Host; // part after '@'
                if (string.IsNullOrWhiteSpace(host))
                {
                    return false;
                }

                // Require at least one dot
                int lastDot = host.LastIndexOf('.');
                if (lastDot <= 0 || lastDot == host.Length - 1)
                {
                    return false;
                }

                // Require TLD length >= 2
                string tld = host.Substring(lastDot + 1);
                if (tld.Length < 2)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verstuur periodiek Peppol-overzicht naar de klant.
        /// </summary>
        public static string SendPeriodOverviewPeppolMail(
            string smtpServer,
            int smtpPort,
            bool enableSsl,
            string smtpUser,
            string smtpPassword,
            string fromEmail,
            string fromName,            
            string clientEmail,
            string reportHtml,            
            Action<string> log = null)
        {
            try
            {
                log?.Invoke("Start sending report mail...");
                                
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, fromEmail));
                    message.To.Add(MailboxAddress.Parse(clientEmail));
                    message.Subject = $"Status rapport " + SharedGlobals.ApiModus + " " + DateTime.Now.ToString("dd/MM/yyyy");

                    // Create the multipart/mixed container
                    var multipart = new Multipart("mixed");

                    // Add the HTML body
                    var body = new TextPart("html")
                    {
                        Text = BuildReportHtmlBody(reportHtml)
                    };
                    multipart.Add(body);

                    message.Body = multipart;

                    using (var mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(smtpServer, smtpPort, enableSsl);
                        mailClient.Authenticate(smtpUser, smtpPassword);
                        mailClient.Send(message);
                        mailClient.Disconnect(true);
                    }
                    log?.Invoke("Mail sent successfully (MailKit).");
                    return "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    log?.Invoke("MailKit ERROR: " + ex.Message);
                    return "ERROR sending mail: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                log?.Invoke("ERROR sending mail: " + ex.Message);
                return "ERROR sending mail: " + ex.Message;
            }
        }
        /// <summary>
        /// Verstuur wekelijkse Peppol-documenten naar de accountant.
        /// </summary>
        public static string SendWeeklyPeppolMail(
            string smtpServer,
            int smtpPort,
            bool enableSsl,
            string smtpUser,
            string smtpPassword,
            string fromEmail,
            string fromName,
            string customerFileNumber,
            string accountantEmail,
            string accountantName,
            string selectedYear,
            string selectedWeek,
            Action<string> log = null)
        {
            try
            {
                log?.Invoke("Start sending weekly Peppol mail...");

                log?.Invoke("Selected week number: " + selectedWeek);

                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, fromEmail));
                    message.To.Add(MailboxAddress.Parse(accountantEmail));
                    message.Subject = $"{customerFileNumber}: Wekelijkse Peppol-documenten – Week {selectedWeek}";

                    // Create the multipart/mixed container
                    var multipart = new Multipart("mixed");

                    // Add the HTML body
                    var body = new TextPart("html")
                    {
                        Text = BuildAccountingHtmlBody(accountantName, fromName, fromEmail)
                    };
                    multipart.Add(body);

                    // Add attachments as a single ZIP file for the week
                    string folderInPath =
                        SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany
                        + "\\peppol\\archive\\"
                        + selectedYear
                        + "\\" + selectedWeek.Substring(0,6)  + "\\";

                    try
                    {
                        var peppolFiles = Directory.GetFiles(folderInPath, "*.xml");
                        if (peppolFiles.Length == 0)
                        {
                            return "Geen Peppol-bestanden aanwezig voor week " + selectedWeek;
                        }

                        // Create ZIP in temp folder
                        string tempFolder = Path.GetTempPath();
                        string zipFileName = "Peppol_Week" + selectedWeek + "_" + selectedYear + ".zip";
                        string zipFilePath = Path.Combine(tempFolder, zipFileName);

                        if (File.Exists(zipFilePath))
                        {
                            File.Delete(zipFilePath);
                        }

                        using (var zipStream = new ZipArchive(
                                   File.Create(zipFilePath),
                                   ZipArchiveMode.Create))
                        {
                            foreach (var file in peppolFiles)
                            {
                                if (!File.Exists(file))
                                {
                                    log?.Invoke("WARNING: File not found: " + file);
                                    continue;
                                }

                                // Use Optimal compression level instead of the default (no compression)
                                var entry = zipStream.CreateEntry(
                                    Path.GetFileName(file),
                                    CompressionLevel.Optimal);

                                using (var entryStream = entry.Open())
                                using (var fileStream = File.OpenRead(file))
                                {
                                    fileStream.CopyTo(entryStream);
                                }

                                log?.Invoke("Added to ZIP: " + file);
                            }
                        }

                        // Attach ZIP
                        var zipAttachment = new MimePart("application", "zip")
                        {
                            Content = new MimeContent(File.OpenRead(zipFilePath)),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = zipFileName
                        };
                        multipart.Add(zipAttachment);
                        log?.Invoke("Attached ZIP: " + zipFilePath);
                    }
                    catch (Exception)
                    {
                        return "Geen Peppol-bestanden aanwezig voor week " + selectedWeek;
                    }

                    message.Body = multipart;

                    using (var mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(smtpServer, smtpPort, enableSsl);
                        mailClient.Authenticate(smtpUser, smtpPassword);
                        mailClient.Send(message);
                        mailClient.Disconnect(true);
                    }
                    log?.Invoke("Mail sent successfully (MailKit).");
                    return "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    log?.Invoke("MailKit ERROR: " + ex.Message);
                    return "ERROR sending mail: " + ex.Message;
                }               
            }
            catch (Exception ex)
            {
                log?.Invoke("ERROR sending mail: " + ex.Message);
                return "ERROR sending mail: " + ex.Message;
            }
        }

        /// <summary>
        /// Verstuur wekelijkse Peppol-documenten naar de accountant.
        /// </summary>
        public static string SendBadRequestPeppolMail(
            string smtpServer,
            int smtpPort,
            bool enableSsl,
            string smtpUser,
            string smtpPassword,
            string fromEmail,
            string fromName,            
            string vsoftAdminEmail,
            string vsoftAdminName,
            string selectedPeppolFile,  
            string responseBody,
            string apiModus,
            Action<string> log = null)
        {
            try
            {
                log?.Invoke("Start sending BadRequest Peppol mail...");

                log?.Invoke("Selected File: " + selectedPeppolFile);

                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, fromEmail));
                    message.To.Add(MailboxAddress.Parse(vsoftAdminEmail));
                    message.Subject = "BadRequest melding in " + apiModus;

                    // Create the multipart/mixed container
                    var multipart = new Multipart("mixed");

                    // Add the HTML body
                    var body = new TextPart("html")
                    {
                        Text = BuildBadRequestHtmlBody(vsoftAdminName, fromName, fromEmail, responseBody)
                    };
                    multipart.Add(body);

                    // Add attachment as a single ZIP containing only selectedPeppolFile
                    try
                    {
                        string tempFolder = Path.GetTempPath();
                        string fileToZip = selectedPeppolFile;

                        if (!File.Exists(fileToZip))
                        {
                            log?.Invoke("WARNING: File not found: " + fileToZip);
                            return "Bestand niet gevonden: " + fileToZip;
                        }

                        // Create a ZIP name based on the selected file name
                        string zipFileName = Path.GetFileNameWithoutExtension(fileToZip) + ".zip";
                        string zipFilePath = Path.Combine(tempFolder, zipFileName);

                        if (File.Exists(zipFilePath))
                        {
                            File.Delete(zipFilePath);
                        }

                        using (var zipStream = new ZipArchive(
                                   File.Create(zipFilePath),
                                   ZipArchiveMode.Create))
                        {
                            // Single entry: the selectedPeppolFile
                            var entry = zipStream.CreateEntry(
                                Path.GetFileName(fileToZip),
                                CompressionLevel.Optimal);

                            using (var entryStream = entry.Open())
                            using (var fileStream = File.OpenRead(fileToZip))
                            {
                                fileStream.CopyTo(entryStream);
                            }

                            log?.Invoke("Added to ZIP: " + fileToZip);
                        }

                        // Attach ZIP
                        var zipAttachment = new MimePart("application", "zip")
                        {
                            Content = new MimeContent(File.OpenRead(zipFilePath)),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = zipFileName
                        };
                        multipart.Add(zipAttachment);
                        log?.Invoke("Attached ZIP: " + zipFilePath);
                    }
                    catch (Exception ex)
                    {
                        log?.Invoke("Error creating ZIP: " + ex.Message);
                        return "Er ging iets fout bij het maken van de ZIP: " + ex.Message;
                    }

                    message.Body = multipart;

                    using (var mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(smtpServer, smtpPort, enableSsl);
                        mailClient.Authenticate(smtpUser, smtpPassword);
                        mailClient.Send(message);
                        mailClient.Disconnect(true);
                    }
                    log?.Invoke("Mail sent successfully (MailKit).");
                    return "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    log?.Invoke("MailKit ERROR: " + ex.Message);
                    return "ERROR sending mail: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                log?.Invoke("ERROR sending mail: " + ex.Message);
                return "ERROR sending mail: " + ex.Message;
            }
        }

        private static string BuildReportHtmlBody(string htmlReport)
        {
            return
$@"<p>Beste klant,</p>

        <p>Het <strong>wekelijkse rapport</strong> van Peppol-documenten in verwerking:</p>

        {htmlReport}
                
        <p>Indien er bijkomende info nodig is, laat het gerust weten.</p>

        <p>Met vriendelijke groeten,<br>
        <strong>Jos Vermoesen</strong><br>
        info@rv.be</p>";
        }

        private static string BuildAccountingHtmlBody(
            string accountantName,
            string senderName,
            string senderEmail)
        {
            return
$@"<p>Beste {accountantName},</p>

        <p>Zoals afgesproken bezorg ik hierbij de <strong>wekelijkse export</strong> van onze Peppol-documenten:</p>

        <ul>
          <li>Aankoopfacturen (BIS Billing 3.0 – ontvangen)</li>
          <li>Verkoopfacturen (BIS Billing 3.0 – verzonden)</li>
          <li>Eventuele creditnota’s en correctieberichten</li>
        </ul>

        <p>Alle documenten zijn opgenomen in de bijlagen in hun originele Peppol-formaat (UBL/XML).</p>

        <p>Indien er bijkomende info nodig is of bepaalde documenten ontbreken, laat het gerust weten.</p>

        <p>Met vriendelijke groeten,<br>
        <strong>{senderName}</strong><br>
        {senderEmail}</p>";
        }

        private static string BuildBadRequestHtmlBody(
            string accountantName,
            string senderName,
            string senderEmail,
            string responseBody)
        {
            string structuredResponseBody;

            if (string.IsNullOrWhiteSpace(responseBody))
            {
                structuredResponseBody = "(geen response body ontvangen)";
            }
            else
            {
                try
                {
                    var deserializedString = JsonConvert.DeserializeObject(responseBody);
                    structuredResponseBody = JsonConvert.SerializeObject(deserializedString, Formatting.Indented);
                }
                catch
                {
                    structuredResponseBody = responseBody;
                }
            }

            string encodedResponseBody = WebUtility.HtmlEncode(structuredResponseBody);

            return
$@"<p>Beste {accountantName},</p>

        <p>In bijlage een verkoopdocument BadRequest.</p>

        <p><strong>API response body:</strong></p>
        <pre style=""background:#f6f8fa;border:1px solid #d0d7de;padding:12px;white-space:pre-wrap;word-break:break-word;font-family:Consolas,'Courier New',monospace;font-size:12px;"">{encodedResponseBody}</pre>
        
        <strong>{senderName}</strong><br>
        {senderEmail}</p>";
        }

        // Custom method to calculate ISO 8601 week number
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat. If its Monday, Tuesday or Wednesday, then it'll
            // be the same week number as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the ISO 8601 week number
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
