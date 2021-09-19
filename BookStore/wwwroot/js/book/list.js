// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
        $.get("GetGenreList", function (data) {
            $("#GenreId").empty
            $.each(data, function (index, row) {
                $("#GenreId").append("<option value='" + row.ID + "'>" + row.Name + "<option>")
            });
        });
});

