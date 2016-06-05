using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Models
{
	public class StatistikaModel
	{
		[Display(Name = "Predmet")]
		public int OdabraniPredmetID { get; set; }
		public ICollection<OpisPredmeta> OpisiPredmeta { get; set; }
		public IEnumerable<SelectListItem> SelectListPredmeta
		{
			get { return new SelectList(OpisiPredmeta, "ID", "Naziv"); }
		}

		[Display(Name = "Tipovi aktivnosti")]
		public int OdabraniTipAktivnostiID { get; set; }
		public ICollection<OpisTipaAktivnosti> OpisiTipovaAktivnosti { get; set; }
		public IEnumerable<SelectListItem> SelectListTipAktivnosti
		{
			get { return new SelectList(OpisiTipovaAktivnosti, "ID", "Naziv"); }
		}

		[Display(Name = "Aktivnosti")]
		public int OdabranaAktivnostID { get; set; }
		public ICollection<OpisAktivnosti> OpisiAktivnosti { get; set; }
		public IEnumerable<SelectListItem> SelectListAktivnosti
		{
			get { return new SelectList(OpisiAktivnosti, "ID", "Naziv"); }
		}
	}

	public class OpisPredmeta
	{
		public int ID { get; set; }
		public String Naziv { get; set; }
	}

	public class OpisTipaAktivnosti
	{
		public int ID { get; set; }
		public String Naziv { get; set; }
	}

	public class OpisAktivnosti
	{
		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}