
using MailKit.Net.Smtp;
using MimeKit;


namespace PCDiagnostic.Services
{
    public static class EmailService
    {
        public static void SendReport(string reportPath, string clientName, string clientEmail, string company)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse("miquifajardo@gmail.com"));

            message.To.Add(MailboxAddress.Parse("miquifajardo@gmail.com"));
            if (!string.IsNullOrWhiteSpace(clientEmail))
            {
                message.Cc.Add(
                    MailboxAddress.Parse(clientEmail));
            }

            message.Subject = $"Informe PCDiagnostic - {clientName}";

            var builder = new BodyBuilder();

            builder.TextBody = $"""
                Nom: {clientName}

                Correu: {clientEmail}

                Empresa: {company}

                Data: {DateTime.Now:dd/MM/yyyy HH:mm}

                Informe generat automàticament per PCDiagnostic.

                Miquel Fajardo

                INFORMATICASSA
            """;

            builder.Attachments.Add(reportPath);

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            client.Authenticate( "miquifajardo@gmail.com", "qbdg ducd tpya bhhf");

            client.Send(message);

            client.Disconnect(true);
        }
    }
}
