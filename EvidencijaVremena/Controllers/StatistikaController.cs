using EvidencijaVremena.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
    public class StatistikaController : Controller
    {
		private static int ECTS = 28;

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
		public ActionResult OsvjeziPredmet(int predmetID)
		{
			ICollection<Evidencija> evidencije;
			// izracunaj ukupno potroseno vrijeme za korisnika
			evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID && e.Aktivnost.PredmetID == predmetID).ToList();
			double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);
			// izracunaj prosjecno ukupno potroseno vrijeme
			evidencije = db.Evidencija.Where(e => e.Aktivnost.PredmetID == predmetID).ToList();
			double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);
			int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
			double prosjekECTS = ukupniECTS / brojPretplatnika;
			// nadji definiciju ects za predmet
			double predmetECTS = (double)db.Predmet.First(p => p.ID == predmetID).ECTS;

			Debug.WriteLine("korisnik: " + korisnikECTS);
			Debug.WriteLine("prosjek: " + prosjekECTS);
			Debug.WriteLine("predmet: " + predmetECTS);
			return Json(new { korisnikECTS, prosjekECTS, predmetECTS });
		}
    }
}