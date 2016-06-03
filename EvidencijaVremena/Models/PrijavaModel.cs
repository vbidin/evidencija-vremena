using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class PrijavaModel
	{
		[DisplayName("Korisničko ime")]
		[Required(ErrorMessage = "Nedostaje korisničko ime")]
		public string KorisnickoIme { get; set; }

		[DisplayName("Lozinka")]
		[Required(ErrorMessage = "Nedostaje lozinka")]
		[DataType(DataType.Password)]
		public string Lozinka { get; set; }

		[DisplayName("Zapamti me?")]
		public Boolean ZapamtiMe { get; set; }
		public Boolean JeIspravna(string korisnickoIme, string lozinka)
		{
			using (EvidencijaVremenaEntities data = new EvidencijaVremenaEntities())
			{
				Korisnik korisnik = null;
				try
				{
					korisnik = data.Korisnik.Where(k => k.KorisnickoIme == korisnickoIme).Single();
				}
				catch (InvalidOperationException)
				{
					return false;
				}

				if (String.Compare(korisnik.Lozinka, lozinka) == 0)
				{
					return true;
				}
				return false;
			}		
		}
	}
}