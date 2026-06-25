using Microsoft.Win32;
using PCDiagnostic.Results;
using System.IO;


namespace PCDiagnostic.Modules
{
    public class BackupModule
    {
        public BackupResult Run()
        {
            BackupResult result = new();

            // Historial de fitxers instal·lat

            try
            {
                using RegistryKey? key =
                    Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\FileHistory");

                result.FileHistoryInstalled =
                    key != null;
            }
            catch
            {
            }

          
            // OneDrive

            try
            {
                string oneDrivePath =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.UserProfile),
                        "OneDrive");

                result.OneDriveInstalled =
                    Directory.Exists(oneDrivePath);
            }
            catch
            {
            }

            // Discos USB

            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (!drive.IsReady)
                        continue;

                    if (drive.DriveType == DriveType.Removable)
                    {
                        result.ExternalDriveDetected = true;

                        result.ExternalDriveLetters +=
                            drive.Name.Replace("\\", "") + " ";
                    }
                }

                result.ExternalDriveLetters =
                    result.ExternalDriveLetters.Trim();
            }
            catch
            {
            }

            return result;
        }
    }
}