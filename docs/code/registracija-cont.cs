[HttpPost]
public ActionResult Registracija(RegistracijaModel model)
{	
	Korisnik korisnik = new Korisnik();
	korisnik.KorisnickoIme = model.KorisnickoIme;
	korisnik.Email = model.Email;
	korisnik.Lozinka = Crypto.HashPassword(model.Lozinka);
	korisnik.Uloga = 0;
	
	...
}