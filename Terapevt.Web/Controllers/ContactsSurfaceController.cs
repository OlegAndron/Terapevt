using System.Net;
using System.Web.Mvc;
using System.Net.Mail;
using Terapevt.Web.Models;
using umbraco.NodeFactory;
using Umbraco.Web.Mvc;

namespace Terapevt.Web.Controllers
{
    public class ContactsSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult SendMessage(EmailMessageModel model)
        {
            var contactsNode = new Node(1140);
            var emailTo = contactsNode.GetProperty("Email").Value;
            if (ModelState.IsValid && !string.IsNullOrEmpty(emailTo))
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("terapevtby@gmail.com", "huyhuy22")
                };
                var subject = string.IsNullOrEmpty(model.Phone)
                    ? string.Format("{0} ({1}) прислал(а) сообщение с terapevt.by", model.Name, model.From)
                    : string.Format("{0} ({1}, {2}) прислал(а) сообщение с terapevt.by", model.Name, model.From, model.Phone);
                using (var msg = new MailMessage("no-reply@terapevt.by", emailTo, subject, model.Message))
                {
                    try
                    {
                        smtp.Send(msg);
                        var msg2 = new MailMessage("no-reply@terapevt.by", "hdd@tut.by", subject, model.Message);
                        smtp.Send(msg2);
                    }
                    catch
                    {
                        return Content("<p>Ошибка отправки сообщения.</p>");
                    }
                }
            }
            return Content("<p>Сообщение успешно отправлено.</p>");
        }
    }
}