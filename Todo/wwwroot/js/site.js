function deleteTodo(i) {
	$.ajax({
		url: "Delete",
		type: "POST",
		data: {
			id: i,
		},
		success: function () {
			window.location.reload();
		},
	});
}

function populateForm(i) {
	$.ajax({
		url: "PopulateForm",
		type: "GET",
		data: {
			id: i,
		},
		dataType: "json",
		success: function (response) {
			$("#Todo_Name").val(response.name);
			$("#Todo_Category").val(response.category);
			$("#Todo_Id").val(response.id);
			$("#form-button").val("Update Todo");
			$("#form-action").attr("action", "/Home/Update");
		},
	});
}
