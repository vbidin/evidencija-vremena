using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class PredmetiModel
	{
		public ICollection<Predmet> Pretplaceni { get; set; }
		public ICollection<Predmet> Nepretplaceni { get; set; }

		public PredmetiModel(ICollection<Predmet> pretplaceni, ICollection<Predmet> nepretplaceni)
		{
			this.Pretplaceni = pretplaceni;
			this.Nepretplaceni = nepretplaceni;
		}
	}
}