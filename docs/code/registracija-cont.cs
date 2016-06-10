[HttpPost]
public ActionResult Registracija(RegistracijaModel model)
{
	if (!ModelState.IsValid)
	{
		return View();
	}
	if (db.Korisnik.Where(k => k.KorisnickoIme == model.KorisnickoIme).Count() != 0)
	{
		ModelState.AddModelError("ime_error", "Odabrano korisnicko ime vec postoji");
		return View();
	}
	
	Korisnik korisnik = new Korisnik();
	korisnik.KorisnickoIme = model.KorisnickoIme;
	korisnik.Email = model.Email;
	korisnik.Lozinka = Crypto.HashPassword(model.Lozinka);
	korisnik.Uloga = 0;
	...