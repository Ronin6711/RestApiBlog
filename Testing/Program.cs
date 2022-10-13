using System.Net;
using System.Net.Mail;

namespace Test
{
    public class Test
    {
        public static void Main(string[] args)
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("teamapi@gmail.com", "team");
            // кому отправляем
            MailAddress to = new MailAddress("arturdemchenko05@gmail.com");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Код подтверждения";
            // текст письма
            m.Body = $"<h2>{code}</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 465);
            // логин и пароль
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("teamapi@mail.ru", "Ronin67116677");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(m);
            }
            catch (Exception)
            {

                throw;
            }
            

            Console.WriteLine(code);
        }
    }
}