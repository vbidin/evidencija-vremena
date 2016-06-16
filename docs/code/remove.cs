// briše korisnika 'Pero' iz baze
Korisnik k = db.Korisnik.First(k => k.KorisnickoIme == "Pero");
db.Korisnik.Remove(k);
