/*
 * Se esta usando un webservice para proveer a las perosnas de la P-C-D
 * El problema es que en la base de datos solo se puede guardar un campo y se esta guardando el ID
 * Abría que modificar o incluso crear una tabla con la información de ubicación y eso rompería el esquema y
 * estructura de nuestro proyecto
 */

function getProvincia(id) { 
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

            if (id != null) {
                $("#province").val(id);
            }
        }
    });
}


function getCanton(id_province, id_canton) {

    id_province = (id_province == null) ? $("#province").val() : id_province;

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

            if (id_canton != null) {
                $("#canton").val(id_canton);
            }
        }
    });
}

function getDistrict(id_province, id_canton, id_district) {

    id_province = (id_province == null) ? $("#province").val() : id_province;
    id_canton = (id_canton == null) ? $("#canton").val() : id_canton;

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
            if (id_district != null) {
                $("district").val(id_district);
            }
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