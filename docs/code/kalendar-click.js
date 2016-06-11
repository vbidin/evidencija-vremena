$('#kalendar').fullCalendar({
	eventClick: function (calEvent, jsEvent, view) {
		var akt = {
			id: String(calEvent.id),
			name: String(calEvent.title),
			getAttribute: function (id) {
				if (id == "id")
					return this.id;
                else if (id == "name")
                    return this.name;
            }
        };
        aktivnostOdabrana(akt);
    }
}