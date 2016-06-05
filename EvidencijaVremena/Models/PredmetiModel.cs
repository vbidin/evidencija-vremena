using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class PredmetiModel
	{
		[DisplayName("Pretplate")]
		public ICollection<Predmet> Pretplaceni { get; set; }
		public ICollection<Predmet> Nepretplaceni { get; set; }

		public PredmetiModel(ICollection<Predmet> pretplaceni, ICollection<Predmet> nepretplaceni)
		{
			this.Pretplaceni = pretplaceni;
			this.Nepretplaceni = nepretplaceni;
		}
	}
}