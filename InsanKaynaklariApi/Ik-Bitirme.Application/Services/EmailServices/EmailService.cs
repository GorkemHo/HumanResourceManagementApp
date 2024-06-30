using Ik_Bitirme.Application.Models.VMs.UserVMs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("your-email@example.com")); // Gönderen e-posta adresi
                email.To.Add(MailboxAddress.Parse(toEmail)); // Alıcı e-posta adresi
                email.Subject = "Şifre Sıfırlama"; // E-posta konusu

                var builder = new BodyBuilder();
                builder.HtmlBody = $"Şifrenizi sıfırlamak için lütfen <a href='{resetLink}'>buraya</a> tıklayın."; // Şifre sıfırlama bağlantısı

                email.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls); // SMTP sunucu bağlantısı
                await client.AuthenticateAsync("kolayik469@gmail.com", "ityvbxelnkcgvryq"); // SMTP sunucu kimlik doğrulama bilgileri
                await client.SendAsync(email); // E-postayı gönder
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendWelcomeEmailAsync(WelcomeEmailModel model)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse("your-email@example.com")); // Gönderen e-posta adresi
                emailMessage.To.Add(MailboxAddress.Parse(model.ToEmail)); // Alıcı e-posta adresi
                emailMessage.Subject = "Hoş Geldiniz"; // E-posta konusu

                var builder = new BodyBuilder();
                builder.HtmlBody = $@"
                    <p>Merhaba {model.Username},</p>
                    <p>Kaydınız başarıyla tamamlandı. Aşağıdaki bilgilerle giriş yapabilirsiniz:</p>
                    <ul>
                        <li>Kullanıcı Adı: {model.Username}</li>
                        <li>E-posta: {model.Email}</li>
                        <li>Şifre: {model.Password}</li>
                    </ul>
                    <p>İyi günler dileriz!</p>";

                emailMessage.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls); // SMTP sunucu bağlantısı
                await client.AuthenticateAsync("kolayik469@gmail.com", "ityvbxelnkcgvryq"); // SMTP sunucu kimlik doğrulama bilgileri
                await client.SendAsync(emailMessage); // E-postayı gönder
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                Console.WriteLine(ex.Message);
            }
        }
    }
}
