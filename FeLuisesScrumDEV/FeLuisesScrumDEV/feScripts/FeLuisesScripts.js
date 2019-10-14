/*
 * Se esta usando un webservice para proveer a las perosnas de la P-C-D
 * El problema es que en la base de datos solo se puede guardar un campo y se esta guardando el ID
 * Abría que modificar o incluso crear una tabla con la información de ubicación y eso rompería el esquema y
 * estructura de nuestro proyecto
 */

function getProvincia() { 
    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincias.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option data-name="' + data[key] + '"value="' + key + '">' + data[key] + '</option>';
            }
            $("#province").append(html);
        }
    });
}

function getCanton() {

    var id_province = $("#province").val();

    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincia/"+id_province+"/cantones.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option data-name="' + data[key] + '"value="' + key + '">' + data[key] + '</option>';
            }
            $("#canton").empty();
            $("#canton").append(html);
        }
    });
}

function getDistrict() {

    var id_province = $("#province").val();
    var id_canton = $("#canton").val();

    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincia/" + id_province + "/canton/" + id_canton + "/distritos.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option data-name="'+data[key]+'"value="'+key+'">'+data[key]+'</option>';
            }
            $("#district").empty();
            $("#district").append(html);
        }
    });
}
/*
function getProvincia() {
    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincias.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option value="' + key + '"value="' + data[key] + '">' + data[key] + '</option>';
            }
            /*$("#province").empty();
            $("#province").append(html);
        }
    });
}

function getCanton() {

    var id_province = $("#province option:selected").data("name");
    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincia/" + id_province + "/cantones.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option value="' + key + '"value="' + data[key] + '">' + data[key] + '</option>';
            }
            $("#canton").empty();
            $("#canton").append(html);
        }
    });
}

function getDistrict() {

    var id_province  $("#province option:selected").data("name");
    var id_canton = $("#canton option:selected").data("name");

    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincia/" + id_province + "/canton/" + id_canton + "/distritos.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += '<option value="' + key + '"value="' + data[key] + '">' + data[key] + '</option>';
            }
            $("#district").empty();
            $("#district").append(html);
        }
    });
}*/