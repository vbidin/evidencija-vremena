using EvidencijaVremena.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
    public class StatistikaController : Controller
    {
		private EvidencijaVremenaEntities db;
		private Korisnik Korisnik { get; set; }
		public StatistikaController()
		{
			db = new EvidencijaVremenaEntities();
			// odaberi prvog korisnika (imena "test"), pošto trenutačno ne postoji login sistem
			// kasnije tu stavi korisnika koji je trenutačno ulogiran
			Korisnik = db.Korisnik.First();
		}

		public ActionResult Index(StatistikaModel model)
        {
			ICollection<Predmet> predmeti = db.Pretplata.Where(p => p.KorisnikID == Korisnik.ID).Select(p => p.Predmet).ToList();
			ICollection<OpisPredmeta> opisi = new List<OpisPredmeta>();
			foreach (Predmet predmet in predmeti)
				opisi.Add(new OpisPredmeta() { ID = predmet.ID, Naziv = predmet.Ime + " (" + predmet.Godina + ")" });
			model.OpisiPredmeta = opisi;
            return View(model);
        }

		[HttpPost]
		public ActionResult OsvjeziPredmet()
		{
			return Json("podaci... lol");
		}
    }
}