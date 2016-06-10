$('#azurirajPretplate').on('click', function () {
	var $btn = $(this).button('loading');
	var pretplaceni = [];
	$("#pretplate option").each(function () {
		if ($(this).is(':selected')) {
			pretplaceni.push($(this).val());
		}
	});
	$.post('@Url.Action("AzurirajPretplate", "Predmet")', { pretplaceni: pretplaceni }, PretplateAzurirane, "json");
});