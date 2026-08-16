using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarSync.Classes
{
    public static class PeppolClientSettingsDetector
    {
        public static PeppolClientSettingsSnapshot AnalyzeSnapshot(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("JSON is required.", nameof(json));
            }

            var root = JObject.Parse(json);
            var snapshot = new PeppolClientSettingsSnapshot
            {
                CreatedAt = (string)root["creation-dt"],
                QueryTerms = (string)root["query-terms"]
            };

            var matches = root["matches"] as JArray;
            if (matches == null)
            {
                return snapshot;
            }

            foreach (var match in matches.OfType<JObject>())
            {
                var participant = new PeppolParticipantCapabilities
                {
                    ParticipantId = (string)match["participantID"]?["value"]
                };

                var docTypesArray = match["docTypes"] as JArray;
                var docTypeValues = (docTypesArray ?? new JArray())
                    .OfType<JObject>()
                    .Select(dt => (string)dt["value"])
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                participant.DocTypeValues.AddRange(docTypeValues);
                participant.SupportsInvoice = docTypeValues.Any(v => v.IndexOf("Invoice-2::Invoice", StringComparison.OrdinalIgnoreCase) >= 0);
                participant.SupportsCreditNote = docTypeValues.Any(v => v.IndexOf("CreditNote-2::CreditNote", StringComparison.OrdinalIgnoreCase) >= 0);
                participant.SupportsUblBeConformant = docTypeValues.Any(v =>
                    v.IndexOf("#conformant#", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    v.IndexOf("urn:UBL.BE:1.0.0.20180214", StringComparison.OrdinalIgnoreCase) >= 0);
                participant.IsCreditNoteOnly = participant.SupportsCreditNote && !participant.SupportsInvoice;

                snapshot.Participants.Add(participant);
            }

            snapshot.AllDocTypes = snapshot.Participants
                .SelectMany(p => p.DocTypeValues)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                .ToList();

            snapshot.SupportsInvoice = snapshot.Participants.Any(p => p.SupportsInvoice);
            snapshot.SupportsCreditNote = snapshot.Participants.Any(p => p.SupportsCreditNote);
            snapshot.SupportsUblBeConformant = snapshot.Participants.Any(p => p.SupportsUblBeConformant);
            snapshot.IsCreditNoteOnly = snapshot.SupportsCreditNote && !snapshot.SupportsInvoice;

            return snapshot;
        }

        public static PeppolClientSettingsDelta CompareSnapshots(string baselineJson, string currentJson)
        {
            var baseline = AnalyzeSnapshot(baselineJson);
            var current = AnalyzeSnapshot(currentJson);

            var added = current.AllDocTypes
                .Except(baseline.AllDocTypes, StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var removed = baseline.AllDocTypes
                .Except(current.AllDocTypes, StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new PeppolClientSettingsDelta
            {
                Baseline = baseline,
                Current = current,
                AddedDocTypes = added,
                RemovedDocTypes = removed,
                InvoiceSupportDropped = baseline.SupportsInvoice && !current.SupportsInvoice,
                CreditNoteSupportDropped = baseline.SupportsCreditNote && !current.SupportsCreditNote,
                UblBeConformantDropped = baseline.SupportsUblBeConformant && !current.SupportsUblBeConformant
            };
        }

        public static string BuildHumanReport(PeppolClientSettingsDelta delta)
        {
            if (delta == null)
            {
                throw new ArgumentNullException(nameof(delta));
            }

            var sb = new StringBuilder();

            sb.AppendLine("Peppol client settings comparison");
            sb.AppendLine("--------------------------------");
            sb.AppendLine("Baseline creation-dt: " + (delta.Baseline.CreatedAt ?? "(unknown)"));
            sb.AppendLine("Current  creation-dt: " + (delta.Current.CreatedAt ?? "(unknown)"));
            sb.AppendLine();

            sb.AppendLine("Current support");
            sb.AppendLine("- Invoice: " + YesNo(delta.Current.SupportsInvoice));
            sb.AppendLine("- CreditNote: " + YesNo(delta.Current.SupportsCreditNote));
            sb.AppendLine("- UBL.BE conformant profile: " + YesNo(delta.Current.SupportsUblBeConformant));
            sb.AppendLine("- CreditNote only: " + YesNo(delta.Current.IsCreditNoteOnly));
            sb.AppendLine();

            sb.AppendLine("Removed docTypes");
            if (delta.RemovedDocTypes.Count == 0)
            {
                sb.AppendLine("- (none)");
            }
            else
            {
                foreach (var docType in delta.RemovedDocTypes)
                {
                    sb.AppendLine("- " + docType);
                }
            }

            sb.AppendLine();
            sb.AppendLine("Added docTypes");
            if (delta.AddedDocTypes.Count == 0)
            {
                sb.AppendLine("- (none)");
            }
            else
            {
                foreach (var docType in delta.AddedDocTypes)
                {
                    sb.AppendLine("- " + docType);
                }
            }

            return sb.ToString();
        }

        private static string YesNo(bool value)
        {
            return value ? "YES" : "NO";
        }
    }

    public sealed class PeppolClientSettingsSnapshot
    {
        public PeppolClientSettingsSnapshot()
        {
            Participants = new List<PeppolParticipantCapabilities>();
            AllDocTypes = new List<string>();
        }

        public string CreatedAt { get; set; }

        public string QueryTerms { get; set; }

        public List<PeppolParticipantCapabilities> Participants { get; set; }

        public List<string> AllDocTypes { get; set; }

        public bool SupportsInvoice { get; set; }

        public bool SupportsCreditNote { get; set; }

        public bool SupportsUblBeConformant { get; set; }

        public bool IsCreditNoteOnly { get; set; }
    }

    public sealed class PeppolParticipantCapabilities
    {
        public PeppolParticipantCapabilities()
        {
            DocTypeValues = new List<string>();
        }

        public string ParticipantId { get; set; }

        public List<string> DocTypeValues { get; set; }

        public bool SupportsInvoice { get; set; }

        public bool SupportsCreditNote { get; set; }

        public bool SupportsUblBeConformant { get; set; }

        public bool IsCreditNoteOnly { get; set; }
    }

    public sealed class PeppolClientSettingsDelta
    {
        public PeppolClientSettingsDelta()
        {
            AddedDocTypes = new List<string>();
            RemovedDocTypes = new List<string>();
        }

        public PeppolClientSettingsSnapshot Baseline { get; set; }

        public PeppolClientSettingsSnapshot Current { get; set; }

        public List<string> AddedDocTypes { get; set; }

        public List<string> RemovedDocTypes { get; set; }

        public bool InvoiceSupportDropped { get; set; }

        public bool CreditNoteSupportDropped { get; set; }

        public bool UblBeConformantDropped { get; set; }
    }

}
