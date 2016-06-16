// mijenja ulogu korisnika 'Pero'
Korisnik k = db.Korisnik.First(k => k.KorisnickoIme == "Pero");
k.Uloga = 2;
