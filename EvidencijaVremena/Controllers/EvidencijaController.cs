using EvidencijaVremena.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
	public class EvidencijaController : Controller
	{
		private EvidencijaVremenaEntities db;
		private Korisnik Korisnik { get; set; }

		public EvidencijaController()
		{
			db = new EvidencijaVremenaEntities();
			// odaberi prvog korisnika (imena "test"), pošto trenutacno ne postoji login sistem
			// kasnije tu stavi korisnika koji je trenutacno ulogiran
			Korisnik = db.Korisnik.First();
		}
		public ActionResult Index()
		{
			// uzmi sve pretplaćene predmete korisnika
			List<Pretplata> pretplate = db.Pretplata.Where(p => p.KorisnikID == Korisnik.ID).ToList();
			List<Predmet> predmeti = new List<Predmet>();

			foreach (Pretplata pretplata in pretplate)
			{
				Predmet predmet = pretplata.Predmet;
				if (predmet.Aktivnost.Count != 0)
					predmeti.Add(predmet);
			}

			EvidencijeModel model = new EvidencijeModel() { Predmeti = predmeti };
			return View(model);
		}

		public ActionResult Add(int ID)
		{
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int ID)
		{
			Evidencija evidencija = db.Evidencija.First(p => p.KorisnikID == Korisnik.ID && p.AktivnostID == ID);
			db.Evidencija.Remove(evidencija);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}