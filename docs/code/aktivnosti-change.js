$("#aktivnosti").change(function () {
	var prd = $("#predmeti").children(":selected")[0].value;
	var akt = $("#aktivnosti").children(":selected")[0].value;
	var mj;
	if ($('#sati').is(':checked')) mj = "sati";
	else mj = "ects";
	$.post('@Url.Action("OsvjeziPredmet", "Statistika")', { predmetId: prd, tipAktivnostiID: akt, mjernaJedinica: mj }, OsvjeziPredmet, "json");
});