var len = data.termini.length;
for (var i = 0; i < len; i++) {
	var event = {
		id: data.termini[i].ID,
		title: data.termini[i].Naziv,
		start: data.termini[i].Pocetak,
		end: data.termini[i].Kraj
    };
    $('#kalendar').fullCalendar('renderEvent', event, true);
}