﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvidencijaVremena.Models
{
	public class RegistracijaModel
	{
		[Display(Name = "Korisničko ime")]
		[Required(ErrorMessage = "Nedostaje korisničko ime")]
		public string KorisnickoIme { get; set; }

		[Required(ErrorMessage = "Nedostaje e-mail adresa")]
		[EmailAddress(ErrorMessage = "Neispravna Email adresa")]
		[Display(Name = "Email adresa")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Nedostaje lozinka")]
		[StringLength(100, ErrorMessage = "{0} mora sadržavati barem {2} znakova", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Lozinka")]
		public string Lozinka { get; set; }

		[Required(ErrorMessage = "Nedostaje ponovljena lozinka")]
		[DataType(DataType.Password)]
		[Display(Name = "Ponovi lozinku")]
		[Compare("Lozinka", ErrorMessage = "Nova lozinka se ne podudara sa potvrđenom lozinkom")]
		public string PonovljenaLozinka { get; set; }
	}
}