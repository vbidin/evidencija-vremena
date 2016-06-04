using EvidencijaVremena.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
	[Authorize]
	public class PredmetController : Controller
	{
		private EvidencijaVremenaEntities db = new EvidencijaVremenaEntities();
		private Korisnik Korisnik { get; set; }	

		// GET: Pretplata
		public ActionResult Index()
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			// uzmi sve pretplate za korisnika
			Dictionary<int, Pretplata> pretplate = db.Pretplata
				.Where(p => p.KorisnikID == Korisnik.ID)
				.ToDictionary(p => p.PredmetID);

			// grupiraj pretplaćene i ostale predmete
			IEnumerable<Predmet> predmeti = db.Predmet.ToList();
			ICollection<Predmet> pretplaceni = new List<Predmet>();
			ICollection<Predmet> nepretplaceni = new List<Predmet>();

			foreach (Predmet predmet in predmeti)
			{
				Pretplata _;
				if (pretplate.TryGetValue(predmet.ID, out _))
					pretplaceni.Add(predmet);
				else
					nepretplaceni.Add(predmet);
			}

			PredmetiModel model = new PredmetiModel(pretplaceni, nepretplaceni);
			return View(model);
		}

		public ActionResult Add(int ID)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			Pretplata pretplata = new Pretplata() { KorisnikID = Korisnik.ID, PredmetID = ID };
			db.Pretplata.Add(pretplata);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int ID)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			Pretplata pretplata = db.Pretplata.First(p => p.KorisnikID == Korisnik.ID && p.PredmetID == ID);
			db.Pretplata.Remove(pretplata);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
