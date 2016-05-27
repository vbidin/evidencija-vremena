using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidenciKorisnikVremena.Models
{
	public class Statistika
	{
		public PredmetStatistika Predmet { get; set; }
		public List<TipAktivnostiStatistika> TipoviAktivnosti { get; set; }
		public List<AktivnostStatistika> Aktivnosti { get; set; }
	}

	public class PredmetStatistika
	{
		public string Naziv { get; set; }
		public double Korisnik { get; set; }
		public double Prosjek { get; set; }
		public double Predmet { get; set; }
	}

	public class TipAktivnostiStatistika
	{
		public string Naziv { get; set; }
		public double Korisnik { get; set; }
		public double Prosjek { get; set; }
		public double TipAktivnosti { get; set; }
	}

	public class AktivnostStatistika
	{
		public string Naziv { get; set; }
		public double Korisnik { get; set; }
		public double Prosjek { get; set; }
		public double Aktivnost { get; set; }
	}
}