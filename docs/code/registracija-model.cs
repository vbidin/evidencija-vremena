public class RegistracijaModel
{
	[Display(Name = "Korisnicko ime")]
	[Required(ErrorMessage = "Nedostaje korisnicko ime")]
	public string KorisnickoIme { get; set; }

	[Required(ErrorMessage = "Nedostaje e-mail adresa")]
	[EmailAddress(ErrorMessage = "Neispravna email adresa")]
	[Display(Name = "Email adresa")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Nedostaje lozinka")]
	[StringLength(100, ErrorMessage = "Lozinka mora sadržavati barem 6 znakova", MinimumLength = 6)]
	[DataType(DataType.Password)]
	[Display(Name = "Lozinka")]
	public string Lozinka { get; set; }

	[Required(ErrorMessage = "Nedostaje ponovljena lozinka")]
	[DataType(DataType.Password)]
	[Display(Name = "Ponovi lozinku")]
	[Compare("Lozinka", ErrorMessage = "Lozinka se ne podudara")]
	public string PonovljenaLozinka { get; set; }
}