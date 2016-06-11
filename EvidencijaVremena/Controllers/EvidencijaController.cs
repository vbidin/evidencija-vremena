using EvidencijaVremena.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
	[Authorize]
	public class EvidencijaController : Controller
	{
		private EvidencijaVremenaEntities db = new EvidencijaVremenaEntities();
		private Korisnik Korisnik { get; set; }

		public ActionResult Index()
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			// uzmi sve pretplaćene predmete korisnika
			List<OpisPredmeta> opisiPredmeta = new List<OpisPredmeta>();
			List<Pretplata> pretplate = db.Pretplata.Where(p => p.KorisnikID == Korisnik.ID).ToList();
			foreach (Pretplata pretplata in pretplate)
			{
				Predmet predmet = pretplata.Predmet;
				OpisPredmeta opis = new OpisPredmeta();
				opis.ID = predmet.ID;
				opis.Naziv = predmet.Ime + " (" + predmet.Godina + ")";
				opisiPredmeta.Add(opis);
			}

			EvidencijeModel model = new EvidencijeModel();
			model.OpisiPredmeta = opisiPredmeta;
			if (opisiPredmeta.Count() != 0)
				model.OdabraniPredmetID = opisiPredmeta.ElementAt(0).ID;
			return View(model);
		}

		[HttpPost]
		public ActionResult OsvjeziAktivnosti(int predmetID)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			Debug.WriteLine("Predmet selektiran: " + predmetID);
			List<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
			int[] tipoviAktivnostiID = tipoviAktivnosti.Select(t => t.ID).ToArray();
			string[] tipoviAktivnostiNaziv = tipoviAktivnosti.Select(t => t.Ime).ToArray();

			List<int[]> aktivnostiID = new List<int[]>();
			List<string[]> aktivnostiNaziv = new List<string[]>();
			foreach (int tipAktivnostiID in tipoviAktivnostiID)
			{
				aktivnostiID.Add(db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).Select(a => a.ID).ToArray());
				aktivnostiNaziv.Add(db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).Select(a => a.Ime).ToArray());
			}

			int[][] aktivnostiArrayID = aktivnostiID.ToArray();
			string[][] aktivnostiArrayNaziv = aktivnostiNaziv.ToArray();

			/*
			foreach (string[] a1 in aktivnostiArrayNaziv)
			{
				Debug.WriteLine("---------------")
				foreach (string a in a1)
				{
					Debug.WriteLine(a);
				}
			}
			*/

			// nabavi sve kalendarske aktivnosti
			List<Aktivnost> aktivnosti = db.Aktivnost.Where(a => a.PredmetID == predmetID && a.Termin != null).ToList();
			List<Termin> termini = new List<Termin>();
			foreach (Aktivnost a in aktivnosti)
			{
				Termin t = new Termin();
				t.ID = a.ID;
				t.Naziv = a.Ime;
				t.Pocetak = ((DateTime)a.Termin).ToLocalTime();
				t.Kraj = t.Pocetak.AddMinutes((double)a.Trajanje).ToLocalTime();
				termini.Add(t);
			}
		
			return Json(new { tipoviAktivnostiID, tipoviAktivnostiNaziv, aktivnostiArrayID, aktivnostiArrayNaziv, termini });
		}

		[HttpPost]
		public ActionResult SpremiEvidenciju(int aktivnostID, double trajanje, string mjernaJedinica)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			Evidencija e = new Evidencija();
			e.AktivnostID = aktivnostID;
			e.DatumUnosa = DateTime.Now;
			e.KorisnikID = Korisnik.ID;
			int t;
			if (mjernaJedinica == "sati")
				t = (int)(trajanje * 60);
			else if (mjernaJedinica == "minute")
				t = (int)trajanje;
			else
				throw new ArgumentException("Nepodržana mjerna jedinica.");
			e.Trajanje = t;

			Debug.WriteLine(e);
			db.Evidencija.Add(e);
			db.SaveChanges();
			return Json("");
		}
	}
}