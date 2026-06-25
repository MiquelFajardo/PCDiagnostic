using PCDiagnostic.Models;
using PCDiagnostic.Results;

namespace PCDiagnostic.Modules
{
    public class FindingsModule
    {
        public List<Finding> Run(DiagnosticResult diagnostic)
        {
            List<Finding> findings = new();

            // SSD ple

            if (diagnostic.Hardware != null)
            {
                foreach (var disk in diagnostic.Hardware.Disks)
                {
                    if (disk.DriveLetters.Contains("C:") &&
                        disk.UsedPercent >= 90)
                    {
                        findings.Add(new Finding
                        {
                            Severity = "Warning",
                            Title = "SSD del sistema gairebé ple",
                            Description =
                                $"La unitat del sistema està al {disk.UsedPercent}% d'ocupació.",
                            Recommendation =
                                "Alliberar espai o ampliar la capacitat del disc."
                        });
                    }
                }
            }

            // Secure Boot

            if (diagnostic.Security?.IsSecureBootEnabled == false)
            {
                findings.Add(new Finding
                {
                    Severity = "Info",
                    Title = "Secure Boot desactivat",
                    Description =
                        "Secure Boot no està habilitat.",
                    Recommendation =
                        "Valorar activar Secure Boot a la BIOS."
                });
            }

            // BitLocker

            if (diagnostic.Security?.IsBitLockerEnabled == false)
            {
                findings.Add(new Finding
                {
                    Severity = "Info",
                    Title = "BitLocker desactivat",
                    Description =
                        "Les unitats no estan protegides amb BitLocker.",
                    Recommendation =
                        "Valorar activar el xifratge dels discs."
                });
            }

            // Impressora predeterminada offline

            if (diagnostic.Printers != null)
            {
                foreach (var printer in diagnostic.Printers.Printers)
                {
                    if (printer.IsDefault &&
                        printer.Status == "Offline")
                    {
                        findings.Add(new Finding
                        {
                            Severity = "Warning",
                            Title = "Impressora predeterminada fora de línia",
                            Description =
                                $"{printer.Name} està offline.",
                            Recommendation =
                                "Comprovar alimentació, xarxa o connexió USB."
                        });
                    }
                }
            }

            // Historial de fitxers

            if (diagnostic.Backup == null ||
                diagnostic.Backup.FileHistoryRunning)
            {
                return findings;
            }
            findings.Add(new Finding
            {
                Severity = "Info",
                Title = "Historial de fitxers no actiu",
                Description =
                        "No s'ha detectat cap còpia automàtica activa.",
                Recommendation =
                        "Configurar una estratègia de còpies de seguretat."
            });

            return findings;
        }
    }
}