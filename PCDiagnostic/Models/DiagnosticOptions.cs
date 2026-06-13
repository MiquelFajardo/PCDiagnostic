using System;
using System.Collections.Generic;
using System.Text;

namespace PCDiagnostic.Models
{
    public class DiagnosticOptions
    {
        public bool System { get; set; }

        public bool Hardware { get; set; }

        public bool Performance { get; set; }

        public bool Security { get; set; }

        public bool Network { get; set; }

        public bool Printers { get; set; }

        public bool Backup { get; set; }
    }
}
