using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarSync.Models
{
    public class MailBody
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string RR { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public bool CopySender { get; set; }

        [Required]
        public string Message { get; set; }

        public string Template { get; set; }

        public string Data { get; set; }

        [Required]
        public string ApiGuid { get; set; }

        [Required]
        public string ApiMailKey { get; set; }

        [Required]
        public string ApiNameKey { get; set; }
    }

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