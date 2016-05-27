using EvidencijaVremena.Models;
using EvidenciKorisnikVremena.Models;
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
			ICollection<OpisPredmeta> opisiPredmeta = new List<OpisPredmeta>();
			foreach (Predmet predmet in predmeti)
				opisiPredmeta.Add(new OpisPredmeta() { ID = predmet.ID, Naziv = predmet.Ime + " (" + predmet.Godina + ")" });
			model.OpisiPredmeta = opisiPredmeta;

			int predmetID = predmeti.ElementAt(0).ID;
			ICollection<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
			ICollection<OpisTipaAktivnosti> opisiTipaAktivnosti = new List<OpisTipaAktivnosti>();
			foreach (TipAktivnosti tipAktivnosti in tipoviAktivnosti)
				opisiTipaAktivnosti.Add(new OpisTipaAktivnosti() { ID = tipAktivnosti.ID, Naziv = tipAktivnosti.Ime });
			model.OpisiTipovaAktivnosti = opisiTipaAktivnosti;

			int tipAktivnostiID = tipoviAktivnosti.ElementAt(0).ID;
			ICollection<Aktivnost> aktivnosti = db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).ToList();
			ICollection<OpisAktivnosti> opisiAktivnosti = new List<OpisAktivnosti>();
			foreach (Aktivnost aktivnost in aktivnosti)
				opisiAktivnosti.Add(new OpisAktivnosti() { ID = aktivnost.ID, Naziv = aktivnost.Ime });
			model.OpisiAktivnosti = opisiAktivnosti;

			return View(model);
		}

		[HttpPost]
		public ActionResult OsvjeziPredmet(int predmetID, int tipAktivnostiID)
		{
			Statistika stat = new Statistika();
			// izracunaj statistiku predmeta
			{
				ICollection<Evidencija> evidencije;

				evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID && e.Aktivnost.PredmetID == predmetID).ToList();
				double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);

				evidencije = db.Evidencija.Where(e => e.Aktivnost.PredmetID == predmetID).ToList();
				double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);
				int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
				double prosjekECTS = ukupniECTS / brojPretplatnika;

				double predmetECTS = (double)db.Predmet.First(p => p.ID == predmetID).ECTS;

				Debug.WriteLine("korisnik: " + korisnikECTS);
				Debug.WriteLine("prosjek: " + prosjekECTS);
				Debug.WriteLine("predmet: " + predmetECTS);
				Debug.WriteLine("--------------------");

				stat.Predmet = new PredmetStatistika();
				stat.Predmet.Korisnik = korisnikECTS;
				stat.Predmet.Prosjek = prosjekECTS;
				stat.Predmet.Predmet = predmetECTS;
			}

			// izracunaj statistiku tipa aktivnosti
			{
				stat.TipoviAktivnosti = new List<TipAktivnostiStatistika>();
				ICollection<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
				foreach(TipAktivnosti tipAktivnost in tipoviAktivnosti)
				{
					ICollection<Evidencija> evidencije;

					evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID
													   && e.Aktivnost.PredmetID == predmetID
													   && e.Aktivnost.TipAktivnostiID == tipAktivnost.ID).ToList();
					double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);

					evidencije = db.Evidencija.Where(e => e.Aktivnost.PredmetID == predmetID
													   && e.Aktivnost.TipAktivnostiID == tipAktivnost.ID).ToList();
					double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);
					int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
					double prosjekECTS = ukupniECTS / brojPretplatnika;

					double tipAktivnostiECTS;
					double? temp = db.Opterecenje.Where(o => o.PredmetID == predmetID && o.TipAktivnostiID == tipAktivnost.ID).Single().Iznos;
					if (temp == null)
						tipAktivnostiECTS = 0;
					else
						tipAktivnostiECTS = (double)temp;

					Debug.WriteLine("TipAktivnosti: " + tipAktivnost.Ime);
					Debug.WriteLine("korisnik: " + korisnikECTS);
					Debug.WriteLine("prosjek: " + prosjekECTS);
					Debug.WriteLine("tipAktivnosti: " + tipAktivnostiECTS);
					Debug.WriteLine("--------------------");

					TipAktivnostiStatistika aktStat = new TipAktivnostiStatistika()
					{ Naziv = tipAktivnost.Ime, Korisnik = korisnikECTS, Prosjek = prosjekECTS, TipAktivnosti = tipAktivnostiECTS };
					stat.TipoviAktivnosti.Add(aktStat);
				}
			}

			// izracunaj statistiku aktivnosti
			{
				stat.Aktivnosti = new List<AktivnostStatistika>();
				ICollection<Aktivnost> aktivnosti = db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).ToList();
				foreach(Aktivnost aktivnost in aktivnosti)
				{
					ICollection<Evidencija> evidencije;

					evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID
													   && e.AktivnostID == aktivnost.ID).ToList();
					double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);

					evidencije = db.Evidencija.Where(e => e.AktivnostID == aktivnost.ID).ToList();
					double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (24.0 * ECTS);
					int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
					double prosjekECTS = ukupniECTS / brojPretplatnika;

					double aktivnostECTS;
					double ? temp = db.Aktivnost.Where(a => a.ID == aktivnost.ID).Single().Trajanje;
					if (temp == null)
						aktivnostECTS = 0;
					else
						aktivnostECTS = (double)temp / (24.0 * ECTS);

					Debug.WriteLine("aktivnost: " + aktivnost.Ime);
					Debug.WriteLine("korisnik: " + korisnikECTS);
					Debug.WriteLine("prosjek: " + prosjekECTS);
					Debug.WriteLine("aktivnost:" + aktivnostECTS);
					Debug.WriteLine("--------------------");

					AktivnostStatistika aktStat = new AktivnostStatistika()
					{ Naziv = aktivnost.Ime, Korisnik = korisnikECTS, Prosjek = prosjekECTS, Aktivnost = aktivnostECTS };
					stat.Aktivnosti.Add(aktStat);
				}
			}

			{
				// stavi sve podatke u arraye i posalji
				double korisnikECTS = stat.Predmet.Korisnik;
				double prosjekECTS = stat.Predmet.Prosjek;
				double predmetECTS = stat.Predmet.Predmet;

				String[] tipoviAktivnostiNaziv = stat.TipoviAktivnosti.Select(t => t.Naziv).ToArray();
				double[] korisnikTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Korisnik).ToArray();
				double[] prosjekTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Prosjek).ToArray();
				double[] tipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.TipAktivnosti).ToArray();

				String[] aktivnostiNaziv = stat.Aktivnosti.Select(t => t.Naziv).ToArray();
				double[] korisnikAktivnostiECTS = stat.Aktivnosti.Select(a => a.Korisnik).ToArray();
				double[] prosjekAktivnostiECTS = stat.Aktivnosti.Select(a => a.Prosjek).ToArray();
				double[] aktivnostiECTS = stat.Aktivnosti.Select(a => a.Aktivnost).ToArray();

				return Json(new
				{
					korisnikECTS,
					prosjekECTS,
					predmetECTS,
					tipoviAktivnostiNaziv,
					korisnikTipoviAktivnostiECTS,
					prosjekTipoviAktivnostiECTS,
					tipoviAktivnostiECTS,
					aktivnostiNaziv,
					korisnikAktivnostiECTS,
					prosjekAktivnostiECTS,
					aktivnostiECTS
				});
			}
		}

		[HttpPost]
		public ActionResult OsvjeziAktivnosti(int predmetID)
		{
			ICollection<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
			ICollection<OpisTipaAktivnosti> opisiTipaAktivnosti = new List<OpisTipaAktivnosti>();
			foreach (TipAktivnosti tipAktivnosti in tipoviAktivnosti)
				opisiTipaAktivnosti.Add(new OpisTipaAktivnosti() { ID = tipAktivnosti.ID, Naziv = tipAktivnosti.Ime });

			int[] id = opisiTipaAktivnosti.Select(o => o.ID).ToArray();
			string[] naziv = opisiTipaAktivnosti.Select(o => o.Naziv).ToArray();
			return Json(new { id, naziv });
		}
	}
}