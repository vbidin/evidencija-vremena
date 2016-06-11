public class RegistracijaModel
{
	[Display(Name = "Korisničko ime")]
	[Required(ErrorMessage = "Nedostaje korisničko ime")]
	public string KorisnickoIme { get; set; }

	[Required(ErrorMessage = "Nedostaje lozinka")]
	[StringLength(100, ErrorMessage = "Lozinka mora sadržavati barem 6 znakova", MinimumLength = 6)]
	[DataType(DataType.Password)]
	[Display(Name = "Lozinka")]
	public string Lozinka { get; set; }

	...
}