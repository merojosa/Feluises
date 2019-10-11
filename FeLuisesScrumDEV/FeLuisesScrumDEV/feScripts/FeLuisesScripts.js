

function getProvincia(){ 
    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincias.json",
        data: {},
        success: function (data) {
            var html = "";
            for (key in data) {
                html += "<option value='" + key + "'>" + data[key] + "</option>";
            }
            /*$("#province").empty();*/
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
                html += "<option value='" + key + "'>" + data[key] + "</option>";
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