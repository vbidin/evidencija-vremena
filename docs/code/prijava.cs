if (Crypto.VerifyHashedPassword(korisnik.Lozinka, model.Lozinka))
{
	FormsAuthentication.SetAuthCookie(model.KorisnickoIme, false);
	return RedirectToAction("Index", "Predmet");
}
else
{
	ModelState.AddModelError("lozinka_error", "Pogrešna lozinka");
}