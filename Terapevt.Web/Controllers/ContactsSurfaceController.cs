using System;
using System.Configuration;
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
            if (!ModelState.IsValid || string.IsNullOrEmpty(emailTo))
                return Content("<p>Ошибка отправки сообщения.</p>");
            int port;
            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["emailServer"],
                Port = int.TryParse(ConfigurationManager.AppSettings["emailSmtpPort"], out port) ? port : 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["email"], ConfigurationManager.AppSettings["emailPassword"])
            };
            var subject = string.IsNullOrEmpty(model.Phone)
                ? $"{model.Name} ({model.From}) прислал(а) сообщение с terapevt.by"
                : $"{model.Name} ({model.From}, {model.Phone}) прислал(а) сообщение с terapevt.by";
            using (var msg = new MailMessage(ConfigurationManager.AppSettings["email"], emailTo, subject, model.Message))
            {
                try
                {
                    smtp.Send(msg);
                    var msg2 = new MailMessage(ConfigurationManager.AppSettings["email"], "hdd@tut.by", subject, model.Message);
                    smtp.Send(msg2);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            return Content("<p>Сообщение успешно отправлено.</p>");
        }
    }
}