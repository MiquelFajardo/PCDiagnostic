namespace PCDiagnostic.Models
{
    public class EventInfo
    {
        public DateTime TimeCreated { get; set; }

        public string Source { get; set; } = string.Empty;

        public int EventId { get; set; }

        public string Level { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Severity { get; set; } = "";
    }
}