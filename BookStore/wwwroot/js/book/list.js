// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $.get("GetGenreList", function (data) {
        console.log(data);
           $("#GenreId").empty
            $.each(data, function (index, row) {
                $("#GenreId").append("<option value='" + row.id + "' >" + row.name + "</option>")
            });
        });

    $.get("GetAgeCategoryList", function (data) {
        console.log(data);
        $("#AgeCategoryId").empty
        $.each(data, function (index, row) {
            $("#AgeCategoryId").append("<option value='" + row.id + "' >" + row.name + "</option>")
        });
    });

   
});

function genreFilter(genre) {
    //$.get("ByGenre", { genreFilter: genre });
    $.ajax({
        type: "post",
        datatype: "json",
        url: "/book/ByGenre",
        async: true,
        success: function (data) {
            $("#someDiv").html(data);
        }
    });
}

function ageFilter(age) {
    $.get("List", { ageFilter: age });
}

