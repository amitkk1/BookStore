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



function bookFilter() {
    $.ajax({
        type: "post",
        data: { "genreFilter": $('#GenreId').val(), "ageFilter": $('#AgeCategoryId').val() },
        url: "/book/ByGenre",
        async: true,
        success: function (data) {
            $("#booksFilter").html(data);
        }
    });
}

