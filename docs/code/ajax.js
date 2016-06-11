$.ajax({
	type,    // tip zahtjeva
	url,     // URL na koji se šalje zahtjev
	data,    // podaci koji se šalju poslužitelju
	success, // funkcija koja se poziva nakon uspješnog zahtjeva (engl. callback function)
	dataType // tip podatka koji se očekuje od servera (npr. JSON)
});