namespace PCDiagnostic.Results
{
    public class BackupResult
    {
        public bool FileHistoryInstalled { get; set; }

        public bool FileHistoryRunning { get; set; }

        public bool OneDriveInstalled { get; set; }

        public bool ExternalDriveDetected { get; set; }

        public string ExternalDriveLetters { get; set; } = string.Empty;
    }
}
