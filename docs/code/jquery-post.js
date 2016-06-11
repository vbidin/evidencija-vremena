$(selector).post(
	URL,            // URL na koji se šalje zahtjev
	data,           // podaci koji se šalju poslužitelju
	function(data), // funkcija koja se poziva nakon uspješnog zahtjeva,
					// gdje data sadrži podatke od poslužitelja
	dataType        // tip podatka koji se očekuje od servera
)