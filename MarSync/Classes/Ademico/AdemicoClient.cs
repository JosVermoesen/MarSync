using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static MarSync.AdemicoModels;

namespace MarSync
{
    public class AdemicoClient
    {
        static readonly string ademicoUrl = SharedGlobals.AdemicoApiUrl;
        static readonly string accessToken = SharedGlobals.AdemicoAccessToken;
        static readonly string username = SharedGlobals.AdemicoUsername;
        static readonly string password = SharedGlobals.AdemicoPassword;

        public static async Task<ApiResult> DeleteNotificationAsync(
            int notificationId,
            CancellationToken cancellationToken = default)
        {
            string requestUri = ademicoUrl.TrimEnd('/')
                + "/api/peppol/v1/notifications/" + notificationId
                + "?accessToken=" + Uri.EscapeDataString(accessToken);

            using (var http = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
                {
                    try
                    {
                        using (var response = await http.SendAsync(request, cancellationToken).ConfigureAwait(false))
                        {
                            string body = response.Content == null
                                ? null
                                : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            return new ApiResult(response.StatusCode, body);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        return new ApiResult(0, "HttpRequestException in DeleteNotificationAsync: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return new ApiResult(0, "Exception in DeleteNotificationAsync: " + ex.Message);
                    }
                }
            }
        }

        public static class PeppolInvoiceSender
        {
            public sealed class ApiResult
            {
                public HttpStatusCode StatusCode { get; }
                public string ResponseBody { get; }
                public ApiResult(HttpStatusCode statusCode, string responseBody)
                {
                    StatusCode = statusCode;
                    ResponseBody = responseBody;
                }
            }

            public static async Task<ApiResult> SendUblInvoiceAsync(
                string filePath,
                bool xC5Reporting = false,
                CancellationToken cancellationToken = default)
            {
                string requestUri = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/invoices/ubl-submissions"
                                    + "?accessToken=" + Uri.EscapeDataString(accessToken);

                using (var http = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    http.DefaultRequestHeaders.Accept.Clear();
                    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    http.DefaultRequestHeaders.Add("X-C5-REPORTING", xC5Reporting.ToString().ToLowerInvariant());

                    using (var form = new MultipartFormDataContent())
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                        form.Add(fileContent, "file", Path.GetFileName(filePath));

                        try
                        {
                            using (var response = await http.PostAsync(requestUri, form, cancellationToken).ConfigureAwait(false))
                            {
                                string body = response.Content == null
                                    ? null
                                    : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                                return new ApiResult(response.StatusCode, body);
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            return new ApiResult(0, "HttpRequestException in SendUblInvoiceAsync: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            return new ApiResult(0, "Exception in SendUblInvoiceAsync: " + ex.Message);
                        }
                    }
                }
            }
        }

        public static async Task<string> GetNotificationsAsync(
            string transmissionId,
            string documentId,
            string eventType,
            string peppolDocumentType,
            string sender,
            string receiver,
            string startDateTime,
            string endDateTime,
            string page,
            string pageSize,
        CancellationToken cancellationToken = default)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(transmissionId)) queryParams.Add("transmissionId=" + Encode(transmissionId));
            if (!string.IsNullOrWhiteSpace(documentId)) queryParams.Add("documentId=" + Encode(documentId));
            if (!string.IsNullOrWhiteSpace(eventType)) queryParams.Add("eventType=" + Encode(eventType));
            if (!string.IsNullOrWhiteSpace(peppolDocumentType)) queryParams.Add("peppolDocumentType=" + Encode(peppolDocumentType));
            if (!string.IsNullOrWhiteSpace(sender)) queryParams.Add("sender=" + Encode(sender));
            if (!string.IsNullOrWhiteSpace(receiver)) queryParams.Add("receiver=" + Encode(receiver));
            if (!string.IsNullOrWhiteSpace(startDateTime)) queryParams.Add("startDateTime=" + Encode(startDateTime));
            if (!string.IsNullOrWhiteSpace(endDateTime)) queryParams.Add("endDateTime=" + Encode(endDateTime));
            if (!string.IsNullOrWhiteSpace(page)) queryParams.Add("page=" + Encode(page));
            if (!string.IsNullOrWhiteSpace(pageSize)) queryParams.Add("pageSize=" + Encode(pageSize));
            if (!string.IsNullOrWhiteSpace(accessToken)) queryParams.Add("accessToken=" + Encode(accessToken));

            int requestCounter = SharedGlobals.RequestCounter + 1;
            SharedGlobals.RequestCounter = requestCounter;
            Console.WriteLine(SharedGlobals.RequestCounter);

            var fullUrl = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/notifications";
            if (queryParams.Count > 0)
                fullUrl += "?" + string.Join("&", queryParams);

            int maxRetries = 3;
            int attempt = 0;

            while (true)
            {
                attempt++;
                using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, fullUrl))
                {
                    var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                    try
                    {
                        using (HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                        {
                            if (response.StatusCode == (HttpStatusCode)429) // Too Many Requests
                            {
                                // Read Retry-After header (seconds)
                                double retryAfterSeconds = response.Headers.RetryAfter?.Delta?.TotalSeconds ?? 5;
                                if (attempt >= maxRetries)
                                {
                                    Console.WriteLine("Max retry attempts reached.");


                                    return "{\"error\": \"Too many requests. Please try again later.\", \"retryAfterSeconds\": " + retryAfterSeconds + "}";
                                }
                                await Task.Delay(TimeSpan.FromSeconds(retryAfterSeconds), cancellationToken);
                                continue; // Retry
                            }

                            response.EnsureSuccessStatusCode();
                            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine("HttpRequestException: " + ex.Message);
                        return "{\"error\": \"HttpRequestException: " + ex.Message + "\"}";
                    }
                    catch (Exception ex)
                    {
                        return "{\"error\": \"Exception: " + ex.Message + "\"}";
                    }
                }
            }
        }

        public sealed class LegalEntityResult
        {
            public HttpStatusCode StatusCode { get; }
            public string ResponseBody { get; }
            public LegalEntityResult(HttpStatusCode statusCode, string responseBody)
            {
                StatusCode = statusCode;
                ResponseBody = responseBody;
            }
        }

        public static async Task<LegalEntityResult> GetPeppolRegistrationAsync(
                string country,
                string peppolRegistrationScheme,
                string peppolRegistrationIdentifier,
                string peppolSupportedDocument,
                string legalEntityId,
                int page = 0,
                int pageSize = 100,
                CancellationToken cancellationToken = default)
        {
            var baseUrl = ademicoUrl.EndsWith("/") ? ademicoUrl : ademicoUrl + "/";

            var query = new Dictionary<string, string>
            {
                ["country"] = country ?? throw new ArgumentNullException("country"),
                ["peppolRegistrationScheme"] = peppolRegistrationScheme ?? throw new ArgumentNullException("peppolRegistrationScheme"),
                ["peppolRegistrationIdentifier"] = peppolRegistrationIdentifier ?? throw new ArgumentNullException("peppolRegistrationIdentifier"),
                ["peppolSupportedDocument"] = peppolSupportedDocument ?? throw new ArgumentNullException("peppolSupportedDocument"),
                ["legalEntityId"] = legalEntityId ?? throw new ArgumentNullException("legalEntityId"),
                ["page"] = page.ToString(),
                ["pageSize"] = pageSize.ToString(),
                ["accessToken"] = accessToken
            };

            string queryString;
            using (var formContent = new FormUrlEncodedContent(query))
            {
                queryString = await formContent.ReadAsStringAsync().ConfigureAwait(false);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) })
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestUri = "api/peppol/v1/legal-entities?" + queryString;

                try
                {
                    using (var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false))
                    {
                        string body = response.Content == null
                            ? null
                            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        return new LegalEntityResult(response.StatusCode, body);
                    }
                }
                catch (HttpRequestException ex)
                {
                    return new LegalEntityResult(0, "HttpRequestException in GetPeppolRegistrationAsync: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return new LegalEntityResult(0, "Exception in GetPeppolRegistrationAsync: " + ex.Message);
                }
            }
        }

        public static async Task<LegalEntityResult> GetLegalEntitiesAsync(
            string legalEntityId,
            string name,
            string country,
            string peppolRegistrationScheme,
            string peppolRegistrationIdentifier,
            string peppolSupportedDocument,
            int page = 0,
            int pageSize = 100,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["legalEntityId"] = legalEntityId,
                ["name"] = name,
                ["country"] = country,
                ["peppolRegistrationScheme"] = peppolRegistrationScheme,
                ["peppolRegistrationIdentifier"] = peppolRegistrationIdentifier,
                ["peppolSupportedDocument"] = peppolSupportedDocument,
                ["page"] = page.ToString(),
                ["pageSize"] = pageSize.ToString(),
                ["accessToken"] = accessToken
            };

            var baseUrl = ademicoUrl.TrimEnd('/');
            var requestUri = baseUrl + "/api/peppol/v1/legal-entities?" + string.Join("&", queryParams
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                .Select(kvp => kvp.Key + "=" + Uri.EscapeDataString(kvp.Value)));

            using (var httpClient = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    using (var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false))
                    {
                        string body = response.Content == null
                            ? null
                            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        return new LegalEntityResult(response.StatusCode, body);
                    }
                }
                catch (HttpRequestException ex)
                {
                    return new LegalEntityResult(0, "HttpRequestException in GetLegalEntitiesAsync: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return new LegalEntityResult(0, "Exception in GetLegalEntitiesAsync: " + ex.Message);
                }
            }
        }

        public static async Task<string> CheckConnection(HttpClient client)
        {
            string requestUri = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/tools/connectivity"
                                + "?accessToken=" + Uri.EscapeDataString(accessToken);

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return "Toegang toegestaan ✅";
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Toegang geweigerd ❌";
                }

                return "Unexpected status: " + (int)response.StatusCode + " " + response.ReasonPhrase;
            }
            catch (HttpRequestException ex)
            {
                return "HttpRequestException in CheckConnection: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error in CheckConnection: " + ex.Message;
            }
        }

        public sealed class ApiResult
        {
            public HttpStatusCode StatusCode { get; }
            public string ResponseBody { get; }
            public ApiResult(HttpStatusCode statusCode, string responseBody)
            {
                StatusCode = statusCode;
                ResponseBody = responseBody;
            }
        }

        public static async Task<ApiResult> CreateOrRegisterLegalEntityAsync(
            CreateLegalEntityModel request,
            CancellationToken cancellationToken = default)
        {
            var baseUrl = ademicoUrl.TrimEnd('/');
            var requestUri = baseUrl + "/api/peppol/v1/legal-entities?accessToken=" + Uri.EscapeDataString(accessToken);

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };

            using (var http = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonSerializer.Serialize(request, jsonSerializerOptions);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    try
                    {
                        using (var response = await http.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
                        {
                            string body = response.Content == null
                                ? null
                                : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            return new ApiResult(response.StatusCode, body);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        return new ApiResult(0, "HttpRequestException in CreateOrRegisterLegalEntityAsync: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return new ApiResult(0, "Exception in CreateOrRegisterLegalEntityAsync: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Sends an Invoice Response (e.g., accept/reject) for a received Peppol invoice back to the seller.
        /// </summary>
        public static async Task<ApiResult> SendInvoiceResponseAsync(
            string invoiceTransmissionId,
            string responseCode,
            string note,
            string effectiveDate,
            List<InvoiceResponseClarification> clarifications = null,
            CancellationToken cancellationToken = default)
        {
            string requestUri = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/invoice-responses"
                                + "?accessToken=" + Uri.EscapeDataString(accessToken);

            var requestObj = new InvoiceResponseRequest
            {
                InvoiceTransmissionId = invoiceTransmissionId,
                ResponseCode = responseCode,
                Note = note,
                EffectiveDate = effectiveDate,
                InvoiceClarifications = clarifications
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string json = JsonSerializer.Serialize(requestObj, jsonOptions);

            using (var http = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    try
                    {
                        using (var response = await http.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
                        {
                            string body = response.Content == null
                                ? null
                                : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            return new ApiResult(response.StatusCode, body);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        return new ApiResult(0, "HttpRequestException in SendInvoiceResponseAsync: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return new ApiResult(0, "Exception in SendInvoiceResponseAsync: " + ex.Message);
                    }
                }
            }
        }

        private static string Encode(string val)
        {
            return Uri.EscapeDataString(val ?? string.Empty);
        }
    }

    public class AdemicoModels
    {
        public class ViesApproximate
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string CompanyType { get; set; }
            public int MatchName { get; set; }
            public int MatchStreet { get; set; }
            public int MatchPostalCode { get; set; }
            public int MatchCity { get; set; }
            public int MatchCompanyType { get; set; }
        }
        public class VatResponse
        {
            public bool IsValid { get; set; } = false;
            public DateTime RequestDate { get; set; }
            public string UserError { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string RequestIdentifier { get; set; }
            public string OriginalVatNumber { get; set; }
            public string VatNumber { get; set; }
            public ViesApproximate ViesApproximate { get; set; }
        }
        public class Contact
        {
            public string ContactType { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
        public class LegalEntityDetails
        {
            public bool PublishInPeppolDirectory { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string GeographicalInformation { get; set; }
            public string WebsiteUrl { get; set; }
            public List<Contact> Contacts { get; set; }
            public string AdditionalInformation { get; set; }
        }
        public class PeppolIdentifier
        {
            public string Scheme { get; set; }
            public string Identifier { get; set; }
        }
        public class PeppolRegistration
        {
            public PeppolIdentifier PeppolIdentifier { get; set; }
            public List<string> SupportedDocuments { get; set; }
            public bool PublishInSmp { get; set; }
        }
        public class CreateLegalEntityModel
        {
            public LegalEntityDetails LegalEntityDetails { get; set; }
            public List<PeppolRegistration> PeppolRegistrations { get; set; }
        }

        public class Pagination
        {
            public int Count { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class Notification
        {
            public string EventType { get; set; }
            public int NotificationId { get; set; }
            public string TransmissionId { get; set; }
            public string SbdhTransmissionId { get; set; }
            public DateTime NotificationDate { get; set; }
            public string DocumentId { get; set; }
            public string DocumentStatus { get; set; }
            public string PeppolDocumentType { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public DateTime ReceivedDate { get; set; }
        }
        public class Root
        {
            public Pagination Pagination { get; set; }
            public List<Notification> Notifications { get; set; }
        }

        public class InvoiceResponseClarification
        {
            public string ClarificationCode { get; set; }
            public string ClarificationType { get; set; }
            public string Clarification { get; set; }
        }
        public class InvoiceResponseRequest
        {
            public string InvoiceTransmissionId { get; set; }
            public string ResponseCode { get; set; }
            public string Note { get; set; }
            public string EffectiveDate { get; set; }
            public List<InvoiceResponseClarification> InvoiceClarifications { get; set; }
        }
    }
}
