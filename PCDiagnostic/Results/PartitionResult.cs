using System;
using System.Collections.Generic;
using System.Text;

namespace PCDiagnostic.Results
{
    public class PartitionResult
    {
        public int PartitionNumber { get; set; }

        public string DriveLetter { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public double SizeGB { get; set; }

      }
}
