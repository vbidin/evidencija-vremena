using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class EvidencijeModel
	{
		public ICollection<Predmet> Predmeti { get; set; }

		public int AktivnostID;
		public String Trajanje;
	}
}