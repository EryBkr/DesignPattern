using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace BaseProject.ChainOfResponsibility
{
    //Zincirin son halkası
    public class SendEmailProcessHandler : ProcessHandler
    {
        private readonly string _fileName;

        public SendEmailProcessHandler(string fileName)
        {
            _fileName = fileName;
        }

        public override object Handle(object o)
        {
            var zipFile = o as MemoryStream;
            zipFile.Position = 0;

            #region Send Email
            var mailMessage = new MailMessage();

            var smptClient = new SmtpClient("***");
            Attachment attachment = new Attachment(zipFile, _fileName, MediaTypeNames.Application.Zip);

            mailMessage.From = new MailAddress("***");
            mailMessage.To.Add(new MailAddress("***"));
            mailMessage.Subject = "Zip File Already";
            mailMessage.Body = "<p>Zip Dosyası ektedir</p>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Attachments.Add(attachment);


            smptClient.Port = 587;
            smptClient.EnableSsl = true;
            smptClient.Credentials = new NetworkCredential("***", "***");

            smptClient.Send(mailMessage);
            #endregion

            //Zincirin son halkası olduğu için null olarak set ettik,çünkü zaten son halka olduğu için sonrasında çalışacak bir şey yok
            return base.Handle(null);
        }
    }
}
