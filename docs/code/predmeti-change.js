$("#predmeti").change(function () {
	var prd = $("#predmeti").children(":selected")[0].value;
	$.post('@Url.Action("OsvjeziAktivnosti", "Statistika")', { predmetId: prd }, OsvjeziAktivnosti, "json");
});