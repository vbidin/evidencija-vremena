function SpremiEvidenciju() {
	var id = document.getElementById("aktivnostID").value
	var val = document.getElementById("trajanje").value
	var mj = $("#mjernaJedinica").children(":selected")[0].value;
	// provjeri ispravnost vremena
	if (isNaN(val) || val <= 0) {
		return false;
	}
	// pošalji AJAX-om evidenciju
	$.post('@Url.Action("SpremiEvidenciju", "Evidencija")', { 
		aktivnostID: id, 
		trajanje: val, 
		mjernaJedinica: mj 
		}, EvidencijaSpremljena, "json");
}