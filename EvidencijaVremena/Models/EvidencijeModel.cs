using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaVremena.Models
{
	public class EvidencijeModel
	{
		[Display(Name = "Predmet")]
		public int OdabraniPredmetID { get; set; }
		public ICollection<OpisPredmeta> OpisiPredmeta { get; set; }
		public IEnumerable<SelectListItem> SelectListPredmeta
		{
			get { return new SelectList(OpisiPredmeta, "ID", "Naziv"); }
		}
	}
}