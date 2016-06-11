using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class Termin
	{
		public int ID { get; set; }
		public String Naziv { get; set; }
		public DateTime Pocetak { get; set; }
		public DateTime Kraj { get; set; }
	}
}