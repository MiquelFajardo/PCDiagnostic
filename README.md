# PCDiagnostic

PCDiagnostic és una aplicació d'escriptori desenvolupada amb **C# (.NET 10 WPF)** que permet obtenir un diagnòstic complet d'un ordinador Windows i generar un informe estructurat.

L'objectiu del projecte és facilitar la revisió de l'estat d'un equip abans d'una reparació, manteniment o auditoria tècnica.

---

## Característiques

- Diagnòstic del sistema operatiu
- Informació del maquinari
- Rendiment del sistema
- Estat de la xarxa
- Revisió de la seguretat
- Estat de BitLocker
- Estat de Secure Boot
- Revisió del sistema de còpies de seguretat
- Estat de les impressores
- Consulta del registre d'esdeveniments
- Detecció automàtica de problemes (Findings)
- Generació d'informes JSON
- Visualització dels informes en format HTML
- Enviament d'informes per correu electrònic

---

## Tecnologies utilitzades

- C#
- .NET 10
- WPF
- Windows Management Instrumentation (WMI)
- MailKit
- HTML + CSS
- JSON

---

## Instal·lació

Descarrega l'última versió des de la secció **Releases**.

Descomprimeix el fitxer ZIP i executa:

```
https://informaticassa.tailc888e4.ts.net/arxius/PCDiagnostic_V1.0.0_Windows_x64.zip
```

No cal instal·lar l'aplicació.

---

## Informes

Els informes es desen automàticament a:

```
Documents/
└── PCDiagnostic/
    └── Reports/
```

Cada diagnòstic genera un informe en format JSON.

L'aplicació permet obrir-lo en format HTML i enviar-lo per correu electrònic.

---

## Mòduls disponibles

- Sistema Operatiu
- Maquinari
- Rendiment
- Xarxa
- Seguretat
- Còpies de seguretat
- Registre d'esdeveniments
- Impressores

---

## Estat del projecte

Versió actual:

**v1.0.0**

Projecte en desenvolupament actiu.

Pròximes funcionalitats:

- Exportació PDF
- Historial d'informes
- Comparació entre diagnòstics
- Actualitzacions automàtiques
- Nous mòduls de diagnòstic

---

## Llicència

Aquest projecte es distribueix sota la llicència GPL v3.

---

## Autor

**Miquel Fajardo**

INFORMATICASSA

https://informaticassa.tailc888e4.ts.net

GitHub:
https://github.com/MiquelFajardo
