
using PCDiagnostic.Models;

namespace PCDiagnostic.Results
{
    public class EventLogResult
    {
        public int CriticalCount { get; set; }

        public int ErrorCount { get; set; }

        public List<EventInfo> Events { get; set; } = new();
    }
}