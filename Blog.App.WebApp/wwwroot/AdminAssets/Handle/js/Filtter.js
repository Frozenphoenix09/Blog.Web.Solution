 $(document).on("change", "#catID",function () {

     var _catID = jQuery(this).children(":selected").attr("value");
     var catid = parseFloat(_catID);


     $('#catID option')
         .removeAttr('selected');
     $("#catID > [ value=" + catid + "]").attr("selected", "true");

    $.ajax({
        url: "/admin/Post/Filtter",
        type: "POST",
        data: { catID: catid},
        async: true,
        success: function (results) {
            if (results.status == "success") {
                window.location.href = results.redirectUrl;
            }
        },
        error: function (xhr) {
            alert: ('error');
        }
    });

 });

$(document).on("change", "#roleID", function () {

    var _roleID = jQuery(this).children(":selected").attr("value");
    var roleid = parseFloat(_roleID);

    $('#roleID option')
        .removeAttr('selected');
    $("#roleID > [ value=" + roleid + "]").attr("selected", "true");

    $.ajax({
        url: "/admin/User/Filtter",
        type: "POST",
        data: { roleID: roleid },
        async: true,
        success: function (results) {
            if (results.status == "success") {
                window.location.href = results.redirectUrl;
            }
        },
        error: function (xhr) {
            alert: ('error');
        }
    });

});