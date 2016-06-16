// vraća email-ove svih korisnika naziva 'Pero'
db.Korisnik.Where(k => k.KorisnickoIme == "Pero")
		   .Select(k => k.Email);