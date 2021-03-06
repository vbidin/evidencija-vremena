﻿using EvidencijaVremena.Models;
using EvidenciKorisnikVremena.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Controllers
{
	[Authorize]
	public class StatistikaController : Controller
	{
		private static int ECTS = 28;

		private EvidencijaVremenaEntities db = new EvidencijaVremenaEntities();
		private Korisnik Korisnik { get; set; }

		public ActionResult Index(StatistikaModel model)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			ICollection<Predmet> predmeti = db.Pretplata.Where(p => p.KorisnikID == Korisnik.ID).Select(p => p.Predmet).ToList();
			ICollection<OpisPredmeta> opisiPredmeta = new List<OpisPredmeta>();
			foreach (Predmet predmet in predmeti)
				opisiPredmeta.Add(new OpisPredmeta() { ID = predmet.ID, Naziv = predmet.Ime + " (" + predmet.Godina + ")" });
			model.OpisiPredmeta = opisiPredmeta;

			int predmetID = 0;
			if (predmeti.Count() != 0)
			{
				predmetID = predmeti.ElementAt(0).ID;
			}
			ICollection<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
			ICollection<OpisTipaAktivnosti> opisiTipaAktivnosti = new List<OpisTipaAktivnosti>();
			foreach (TipAktivnosti tipAktivnosti in tipoviAktivnosti)
				opisiTipaAktivnosti.Add(new OpisTipaAktivnosti() { ID = tipAktivnosti.ID, Naziv = tipAktivnosti.Ime });
			model.OpisiTipovaAktivnosti = opisiTipaAktivnosti;

			int tipAktivnostiID = 0;
			if (tipoviAktivnosti.Count() != 0)
			{
				tipAktivnostiID = tipoviAktivnosti.ElementAt(0).ID;
			}
			ICollection<Aktivnost> aktivnosti = db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).ToList();
			ICollection<OpisAktivnosti> opisiAktivnosti = new List<OpisAktivnosti>();
			foreach (Aktivnost aktivnost in aktivnosti)
				opisiAktivnosti.Add(new OpisAktivnosti() { ID = aktivnost.ID, Naziv = aktivnost.Ime });
			model.OpisiAktivnosti = opisiAktivnosti;

			return View(model);
		}

		[HttpPost]
		public ActionResult OsvjeziPredmet(int predmetID, int tipAktivnostiID, string mjernaJedinica)
		{
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

			Statistika stat = new Statistika();
			// izracunaj statistiku predmeta
			{
				ICollection<Evidencija> evidencije;

				evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID && e.Aktivnost.PredmetID == predmetID).ToList();
				double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
				evidencije = db.Evidencija.Where(e => e.Aktivnost.PredmetID == predmetID).ToList();
				double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
				int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
				double prosjekECTS = ukupniECTS / brojPretplatnika;
				double predmetECTS = (double)db.Predmet.First(p => p.ID == predmetID).ECTS;

				stat.Predmet = new PredmetStatistika();
				stat.Predmet.Korisnik = korisnikECTS;
				stat.Predmet.Prosjek = prosjekECTS;
				stat.Predmet.Predmet = predmetECTS;
			}

			// izracunaj statistiku tipa aktivnosti
			{
				stat.TipoviAktivnosti = new List<TipAktivnostiStatistika>();
				ICollection<TipAktivnosti> tipoviAktivnosti = db.Opterecenje.Where(o => o.PredmetID == predmetID).Select(o => o.TipAktivnosti).ToList();
				foreach (TipAktivnosti tipAktivnost in tipoviAktivnosti)
				{
					ICollection<Evidencija> evidencije;

					evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID
													   && e.Aktivnost.PredmetID == predmetID
													   && e.Aktivnost.TipAktivnostiID == tipAktivnost.ID).ToList();
					double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
					evidencije = db.Evidencija.Where(e => e.Aktivnost.PredmetID == predmetID
													   && e.Aktivnost.TipAktivnostiID == tipAktivnost.ID).ToList();
					double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
					int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
					double prosjekECTS = ukupniECTS / brojPretplatnika;
					double tipAktivnostiECTS;
					double? temp = db.Opterecenje.Where(o => o.PredmetID == predmetID && o.TipAktivnostiID == tipAktivnost.ID).Single().Iznos;
					if (temp == null)
						tipAktivnostiECTS = 0;
					else
						tipAktivnostiECTS = (double)temp;

					TipAktivnostiStatistika aktStat = new TipAktivnostiStatistika()
					{ Naziv = tipAktivnost.Ime, Korisnik = korisnikECTS, Prosjek = prosjekECTS, TipAktivnosti = tipAktivnostiECTS };
					stat.TipoviAktivnosti.Add(aktStat);
				}
			}

			// izracunaj statistiku aktivnosti
			{
				stat.Aktivnosti = new List<AktivnostStatistika>();
				ICollection<Aktivnost> aktivnosti = db.Aktivnost.Where(a => a.PredmetID == predmetID && a.TipAktivnostiID == tipAktivnostiID).ToList();
				foreach (Aktivnost aktivnost in aktivnosti)
				{
					ICollection<Evidencija> evidencije;

					evidencije = db.Evidencija.Where(e => e.KorisnikID == Korisnik.ID
													   && e.AktivnostID == aktivnost.ID).ToList();
					double korisnikECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
					evidencije = db.Evidencija.Where(e => e.AktivnostID == aktivnost.ID).ToList();
					double ukupniECTS = evidencije.Select(e => e.Trajanje).Sum() / (60.0 * ECTS);
					int brojPretplatnika = db.Pretplata.Where(p => p.PredmetID == predmetID).Count();
					double prosjekECTS = ukupniECTS / brojPretplatnika;
					double aktivnostECTS;
					double? temp = db.Aktivnost.Where(a => a.ID == aktivnost.ID).Single().Trajanje;
					if (temp == null)
						aktivnostECTS = 0;
					else
						aktivnostECTS = (double)temp / (60.0 * ECTS);					

					AktivnostStatistika aktStat = new AktivnostStatistika()
					{ Naziv = aktivnost.Ime, Korisnik = korisnikECTS, Prosjek = prosjekECTS, Aktivnost = aktivnostECTS };
					stat.Aktivnosti.Add(aktStat);
				}
			}

			// stavi sve podatke u arraye i posalji
			{
				string imePredmeta = null;
				double korisnikECTS = -1;
				double prosjekECTS = -1;
				double predmetECTS = -1;

				String[] tipoviAktivnostiNaziv = null;
				double[] korisnikTipoviAktivnostiECTS = null;
				double[] prosjekTipoviAktivnostiECTS = null;
				double[] tipoviAktivnostiECTS = null;

				String[] aktivnostiNaziv = null;
				double[] korisnikAktivnostiECTS = null;
				double[] prosjekAktivnostiECTS = null;
				double[] aktivnostiECTS = null;

				if (mjernaJedinica == "ects")
				{
					imePredmeta = db.Predmet.Where(p => p.ID == predmetID).Select(p => p.Ime).Single();
					korisnikECTS = stat.Predmet.Korisnik;
					prosjekECTS = stat.Predmet.Prosjek;
					predmetECTS = stat.Predmet.Predmet;

					tipoviAktivnostiNaziv = stat.TipoviAktivnosti.Select(t => t.Naziv).ToArray();
					korisnikTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Korisnik).ToArray();
					prosjekTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Prosjek).ToArray();
					tipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.TipAktivnosti).ToArray();

					aktivnostiNaziv = stat.Aktivnosti.Select(t => t.Naziv).ToArray();
					korisnikAktivnostiECTS = stat.Aktivnosti.Select(a => a.Korisnik).ToArray();
					prosjekAktivnostiECTS = stat.Aktivnosti.Select(a => a.Prosjek).ToArray();
					aktivnostiECTS = stat.Aktivnosti.Select(a => a.Aktivnost).ToArray();
				}
				else if (mjernaJedinica == "sati")
				{
					imePredmeta = db.Predmet.Where(p => p.ID == predmetID).Select(p => p.Ime).Single();
					korisnikECTS = stat.Predmet.Korisnik * ECTS;
					prosjekECTS = stat.Predmet.Prosjek * ECTS;
					predmetECTS = stat.Predmet.Predmet * ECTS;

					tipoviAktivnostiNaziv = stat.TipoviAktivnosti.Select(t => t.Naziv).ToArray();
					korisnikTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Korisnik * ECTS).ToArray();
					prosjekTipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.Prosjek * ECTS).ToArray();
					tipoviAktivnostiECTS = stat.TipoviAktivnosti.Select(t => t.TipAktivnosti * ECTS).ToArray();

					aktivnostiNaziv = stat.Aktivnosti.Select(t => t.Naziv).ToArray();
					korisnikAktivnostiECTS = stat.Aktivnosti.Select(a => a.Korisnik * ECTS).ToArray();
					prosjekAktivnostiECTS = stat.Aktivnosti.Select(a => a.Prosjek * ECTS).ToArray();
					aktivnostiECTS = stat.Aktivnosti.Select(a => a.Aktivnost * ECTS).ToArray();
				}

				return Json(new
				{
					imePredmeta,
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
			Korisnik = db.Korisnik.Where(k => k.KorisnickoIme == User.Identity.Name).First();

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