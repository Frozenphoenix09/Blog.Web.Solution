$(document).on("change", "#keyword", function () {


    var strkeyword = $('#keyword').val();

    $.ajax({
        url: '/admin/Post/SearchForPost',
        datatype: "json",
        type: "GET",
        data: { keyword: strkeyword },
        async: true,
        success: function (results) {
            console.log(results)
            $("#record_table").html("");
            $("#record_table").html(results);
        },
        error: function (xhr) {
            alert('error');
        }
    });

});