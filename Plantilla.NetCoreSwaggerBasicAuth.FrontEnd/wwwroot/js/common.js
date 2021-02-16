



function ajax_method(path, model, successResponse, errorResponse) {

    jQuery.ajax({
        type: "POST",
        cache: false,
        url: path,
        data: model,
        dataType: "json",

        success: function (data) {

            successResponse(data);
        },
        error: function (data) {

            errorResponse(data);
        }
    });

}

var errorResponse = function (data) {

    alert("Uppss algo ocurrió :/ ,para detalle teécnico revise el detalle en consola.");

    try {
        console.log(data.responseText);
    } catch (e) {
        console.log();
    }

};