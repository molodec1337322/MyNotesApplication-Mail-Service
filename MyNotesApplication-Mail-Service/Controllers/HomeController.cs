using Microsoft.AspNetCore.Mvc;
using MyNotesApplication_Mail_Service.Services;

namespace MyNotesApplication_Mail_Service.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailSender _emailSender;

        public HomeController(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Start() 
        {
            return Ok("Mail service Started");
        }
    }
}
