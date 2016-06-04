using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
	[Authorize]
    public class AdministracijaController : Controller
    {
        // GET: Administracija
        public ActionResult Index()
        {
            return View();
        }
    }
}