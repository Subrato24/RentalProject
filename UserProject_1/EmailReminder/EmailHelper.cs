using MailKit.Net.Smtp;
using MimeKit;

namespace UserProject_1.EmailReminder
{
    public class EmailHelper
    {
        public static async Task SendReminderAsync(string email, string name)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rent reminder", "rentmonthly87@gmail.com"));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Rent Payment Reminder";
            message.Body = new TextPart("plain")
            {
                Text = $"Hi {name}, your rent for this month is due. Please make a payment soon. " +
                       $"If you have already paid your rent, please ignore this message."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //Google blocks SMTP access using your actual Gmail password for security reasons.For that appkey is used 
            await client.AuthenticateAsync("rentmonthly87@gmail.com", "mqkt pleh iubx tixb");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        } 
    }
}
