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
	}

	public class OpisPredmeta
	{
		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}