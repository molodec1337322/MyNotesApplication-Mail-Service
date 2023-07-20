using Microsoft.AspNetCore.Mvc;

namespace MyNotesApplication_Mail_Service.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        [Route("")]
        [Route("/Home")]
        public IActionResult Index() 
        {
            return Ok("Mail service started");
        }
    }
}
