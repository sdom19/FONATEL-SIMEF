
var initDataGrid = function () {
    debugger

    $("tbody tr[data-select='true']").click(setItemSelected);
    $("tbody td[data-select='true']").click(setItemSelected);
    $("button[data-select='true']").click(setItemSelected);

        $('table[data-table-grid="true"]').each(function (index) {

            var tableTem = this;
            tableTem.id = "dataTable_" + index;
                   
            var idTable = "#" + tableTem.id;
            var iDisplayLength = $(idTable).attr("data-table-grid-display-rows");            
            if (iDisplayLength === null || iDisplayLength === undefined) {
                iDisplayLength = 10;
            }
            else {
                iDisplayLength = parseInt(iDisplayLength);
            }
   
            var table = $(idTable).DataTable({            
                "bLengthChange": false,
                "responsive": true,            
                "searching": false,
                "destroy" : true,
                "iDisplayLength": iDisplayLength,
                language: {                
                    lengthMenu: "Mostrando _MENU_ datos",
                    info: " _START_ de _END_ de _TOTAL_ datos",
                    paginate: {
                        first: "primero",
                        previous: "anterior",
                        next: "siguiente",
                        last: "anterior"
                    }
                },
                "aoColumnDefs": [{
                    "bSortable": false,
                    "aTargets": ["no-sort"]
                }]
            });

            /*$(idTable + " tfoot th").each(function () {
                var title = $(idTable + ' thead th').eq($(this).index()).text();

                if (title != '') {
                    $(this).html('<input type="text" placeholder=" ' + $.trim(title) + '" />');               
                }
            });   */    

            /*$(idTable + " tfoot th input").each(function () {
                $(this).keyup(function (e) {                
                    if (e.keyCode === 13) {
                        var targetParent = $(this).parent().parent().parent().parent();                    
                        searchingInputFooterTable(targetParent.attr("id"), targetParent.attr("data-tabla-ajax-url"));
                    }
                });
            });*/
           
            // Apply the search
            table.columns().eq(0).each(function (colIdx) {
                $('input', table.column(colIdx).footer()).on('keyup change', function () {                
                    table
                        .column(colIdx)
                        .search(this.value)
                        .draw();
                });            
            });

            //$(idTable + ' tbody').on('click', 'tr', function () {
            //    debugger;
            //    if ($(this).attr('name') == "trOperador") {
            //        var operadorId = $(this).children().attr('id');
            //        var operadorCheck = "#check_" + operadorId;

            //        if ($(operadorCheck).is(":checked")) {
            //            $(operadorCheck).prop("checked", false);
            //            AgregarValoresListaUnCheck(operadorId)
            //        }
            //        else {
            //            $(operadorCheck).prop("checked", true);
            //            AgregarValoresListaCheck(operadorId);
            //        }

                 
            //    }
               
            //    //Funcionalidad del ajuste de indicadores
            //    debugger;
            //    if ($(this).attr('name') == "trIndicador") {
            //        var IdicadorId = $(this).children().attr('id');
            //        var IndicadorCheck = "#checkIndicador_" + IdicadorId;
            //        var valueOrden = $("#ordenIndicador_" + IdicadorId).val();

            //        //if (valueOrden == null || valueOrden == "") {
            //        //    $(IndicadorCheck).prop("checked", false);
            //        //}
            //        //else {
            //        //    if ($(IndicadorCheck).is(":checked"))   {
            //        //        $(IndicadorCheck).prop("checked", false);
            //        //       // AgregarNuevoIndicador(IdicadorId)
            //        //    }
            //        //    else {
            //        //        $(IndicadorCheck).prop("checked", true);
            //        //        AgregarNuevoIndicador(IdicadorId);
            //        //    }
            //        //}
            //    }
               
            //    if ($(this).hasClass('selected')) {
            //      $(this).removeClass('selected');
                    
            //   }
            //   else {
            //    table.$('tr.selected').removeClass('selected');
            //   $(this).addClass('selected');
            //        //var element = $("#" + this.id);
            //      //var valueChecked = element.prop('checked', true);
            //      // alert(this.id);
            //    }
            //});
            $(idTable + ' tbody').on('click', 'tr', function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                }
                else {
                    table.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }
            });

            //Set colors by grid
            $(idTable + " tfoot").css("background", "#0671AE");
            $(idTable + " thead").css("background", "#0671AE");
            $(idTable + " thead tr").css("color", "#FFFFFF");        
            $(".paginate_button.current").css('cssText', 'color: #FFFFFF !important');
            $(".paginate_button.current").css("background", "#0671AE");
        });
    };        
       

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-tooltip="true"]').tooltip();
    
    $('table[data-table-grid="true"]').ready(initDataGrid);

    $('.modal.fade').on('hidden.bs.modal', function (e) {
        $('.modal.fade div .alert.alert-danger').hide();        
    });


    //Session
    var idSessionleTime = 0;

    $(document).ready(function () {
        //Increment the idle time counter every minute.
        var url = location.href;
        
        var local = location.protocol + "//" + location.host + "/";


        console.log(local);

        if (local == url || local + "Login?" == url) {
             console.log("No hace nada");
        } else {

 //            debugger;

            var idleInterval = setInterval(timerIncrement, 60000); // 1 minute

   //        debugger;
           // Zero the idle timer on mouse movement.
              $(this).mousemove(function (e) {
                  idSessionleTime = 0;
                $("#modalSessionOut").modal('hide');

              });
            $(this).keypress(function (e) {
                idSessionleTime = 0;
                $("#modalSessionOut").modal('hide');
            });
        }
    });

    var counter = 0;

    function timerIncrement() {
     
        idSessionleTime = idSessionleTime + 1;

        if (idSessionleTime == (globalTimeSessionExpire - 1)) {

           
            $('#modalSessionOut').modal('show');
            $("#tituloMensajeS").html("Alerta!!");
            counter = setInterval(met, 1000);
          
        }

        if (idSessionleTime >= globalTimeSessionExpire) { // 30 minutes
            window.location.href = "/Login/LogOffByFrontEnd";
           
        }
    };

    
    var count = 60;
    function met() {
        count = count - 1;
        if (count <= 0) {
            clearInterval(counter);
            count = 60;
            return;
        }
        $("#contenidoMensajeS").html(count + " secs");
}

//pruebas de MOlina
(function alertjose() {
    alert("jose sos grande");
  });