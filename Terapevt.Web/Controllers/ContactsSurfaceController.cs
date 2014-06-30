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
            contactsNode.GetProperty("Email");
            var x = model.From;
            var client = new SmtpClient("mail.google.com");
            return Redirect("/");
        }
    }
}