// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
  $("#CountryId").change(function () {
    var countryId = $(this).val();
    $("#CityId").empty();
    if (countryId) {
      $.getJSON(
        "/City/GetCitiesByCountry",
        { countryId: countryId },
        function (data) {
          $("#CityId").append(
            $("<option>").text("----Select City----").attr("value", "")
          );
          $.each(data, function (i, city) {
            $("#CityId").append(
              $("<option>").text(city.name).attr("value", city.id)
            );
          });
        }
      );
    } else {
      $("#CityId").append(
        $("<option>").text("----Select City----").attr("value", "")
      );
    }
  });
  $(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    document.getElementById("PreviewPhoto").src = window.URL.createObjectURL(
      this.files[0]
    );
    document.getElementById("PhotoUrl").value = fileName;
  });
});

function ShowCreateModalForm() {
  $("#createModelDialog").modal("show");
  return;
}

function ShowCountryCreateModal() {
  $.ajax({
    url: "/country/CreateModalForm",
    type: "get",
    success: function (response) {
      $("#createModelDialog .modal-body").html(response);
      ShowCreateModalForm();
    },
  });
  return;
}

function onCountryCreated(response) {
  if (response.success) {
    $("#createModelDialog").modal("hide");
    var newOption = new Option(
      response.country.name,
      response.country.id,
      true,
      true
    );
    $("select[asp-for='CountryId']").append(newOption).trigger("change");
  } else {
    $("#createModelDialog .modal-body").html(response);
  }
}
