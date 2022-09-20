



var fileList = new Array();
var filesToChooseOnModalReplace = new Array();
var filesToReplaceChecked = new Array();


setTimeout(function () {
    $('#alertExito').fadeOut('fast');
}, 6000); // <-- time in milliseconds


function CargarDuplicadosEnModal(item) {

    for (var i = 0 ; i < fileList.length ; i++) {
        var nombreSinExtension = fileList[i].name.substring(0, (fileList[i].name - 4));
        if (item == nombreSinExtension)
            filesToChooseOnModalReplace = nombreSinExtension;
    }
}

function EnviarArchivosParaReemplazar() {

    alert("EnviarArchivosParaReemplazar");

    for (var i = 0 ; i < fileList.length ; i++) {

    }
}


