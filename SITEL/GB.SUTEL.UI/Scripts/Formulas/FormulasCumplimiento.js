$(document).ready(function () {
    var ArrayFormula = new Array;
    var confirmacion;
    var criteriosPorcentaje = new Array;
    var Constante = 1;
    /// variables Cumplimiento
    $("#BtnMayor").hide();
    $("#BtnMenor").hide();
    $("#BtnMayorque").hide();
    $("#BtnMenorque").hide();
    $("#BtnSeparador").hide();
    $(".ParamIf").hide("fade");
    $(".formulaPocentaje").hide();
    $(".formulaCumplimiento").hide();
    $("#FP").hide();
    $("#UM").hide();
    $("#PS").hide();
    var ArrayIF = new Array;
    var ArrayFormulaSi = new Array;
    var ArrayFormulaNO = new Array;
    var BanderaSi = 0;
    var stringFormFin = "";
    var CumplimientoFinal = "";
    ///////
    $(".criterios").hide();
    // eventos
    $("#TxtConstante").numeric(".");
    $("#Confirmacion").hide();
    $("#Constante").hide();
    $("#CombServicios").attr("disabled", true)
    $("#Confirmacion2").hide();
    $("#CombIndicadores").attr("readonly", true)

    $("#CobmDireccion").on("change", function () {

        if ($("#CombServicios").val() != 0) {
            var IdServicios = $("#CombServicios").val();
            var IdDireccion = $("#CobmDireccion").val();
            AjaxIdicadoSeric($("#CombServicios").val(), $("#CobmDireccion").val());
            AjaxIdicadorCriterio($("#CombIndicadores").val());
        }
        $("#CombServicios").attr("disabled", false)
    });

    $("#CombServicios").on("change", function () {
        AjaxIdicadoSeric($("#CombServicios").val(), $("#CobmDireccion").val());
        AjaxIdicadorCriterio($("#CombIndicadores").val());
    });

    $("#CombIndicadores").on("change", function () { AjaxIdicadorCriterio($("#CombIndicadores").val()); });

    $(document).on('click', '#BtnConstante', function () { $("#exampleModalCenter").modal(); Constante = 1;});

    $(document).on('click', '#BtnConstanteCumpli', function () { $("#exampleModalCenter").modal(); Constante = 2; });

    $(document).on('click', '#UM', function () {
        if (BanderaSi == 0) {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {
                ArrayFormulaSi.push("UM");
                MostarFormulaSi(ArrayFormulaSi);
            }

            //CombinarArray();
        } else if (BanderaSi == 2) {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {
                ArrayFormulaNO.push("UM");
                MostarFormulaSi(ArrayFormulaNO);
            }
            //CombinarArray();
        } else if (BanderaSi == 1) {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {
                ArrayIF.push("UM");
                MostarFormulaSi(ArrayIF);
            }
            //CombinarArray();
        }
    });

    $(document).on('click', '#FP', function () {
       
        ////console.log(BanderaSi);
        if (BanderaSi == 0) {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {
                ArrayFormulaSi.push("FP");
                MostarFormulaSi(ArrayFormulaSi);
            }

            //CombinarArray();
        } else if (BanderaSi == 2)
        {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {

                ArrayFormulaNO.push("FP");
                MostarFormulaSi(ArrayFormulaNO);
            }
            //CombinarArray();
        } else if (BanderaSi == 1)
        {
            var Respuesta = ItemArrSi();
            ////console.log(Respuesta)
            if (Respuesta == 2) {
                ArrayIF.push("FP");
                MostarFormulaSi(ArrayIF);
            }
            //CombinarArray();
        }
    });

    $(document).on('click', '#PS', function () {

        ////console.log(BanderaSi);
        if (BanderaSi == 0) {
            var Respuesta = ItemArrSi();
            if (Respuesta == 2) {
                ArrayFormulaSi.push("FR");
                MostarFormulaSi(ArrayFormulaSi);
            }

            //CombinarArray();
        } else if (BanderaSi == 2) {
            var Respuesta = ItemArrSi();
            if (Respuesta == 2) {
                ArrayFormulaNO.push("FR");
                MostarFormulaSi(ArrayFormulaNO);
            }
            //CombinarArray();
        } else if (BanderaSi == 1) {
            var Respuesta = ItemArrSi();
            if (Respuesta == 2) {
                ArrayIF.push("FR");
                MostarFormulaSi(ArrayIF);
            }
            //CombinarArray();
        }
    });

    $(document).on('click', '#BtnRemoverItem', function (){ RemoverUitem(); });

    $(document).on('click', '#BtnRemoverItemCumpli', function () {   RemoveritemIf(); });
    
    $(document).on('click', '#BtnReset', function () {
        ArrayFormula =[];
        MostarFormula(ArrayFormula, 0)
        $("input[type=checkbox]").attr("checked", false);
    });

    $(document).on('click', '#BtnGuardar', function () {

        ////console.log("constanet" + Constante)
        if ($("#TxtConstante").val() == "") {
            $("#TxtConstante").css("border-color", "red");
        }

        switch (Constante) {
            case 1:
                $("#exampleModalCenter").modal("hide");
                var resul = ItemArr($("#TxtConstante").val());
                ////console.log(resul)
                if (resul == '2')
                {
                    ArrayFormula.push($("#TxtConstante").val());
                    $("#TxtConstante").val("")
                }
                MostarFormula(ArrayFormula, 0);
                break
            case 2:
                ConstanteFun();
                break
            case 3:
                ConstanteFun();
                break
            case 4:
                ConstanteFun();
                break
            default:

        }
        $("#exampleModalCenter").modal("hide");
    });


    function ConstanteFun() {

        if (BanderaSi == 0) {
            var resul = ItemArrSi();
            if (resul == '2') {
                ArrayFormulaSi.push($("#TxtConstante").val());
                $("#TxtConstante").val("")

            }
            MostarFormulaSi(ArrayFormulaSi);

        } else if (BanderaSi == 2) {
            var resul = ItemArrSi();
            if (resul == '2') {
                ArrayFormulaNO.push($("#TxtConstante").val());
                $("#TxtConstante").val("")
            }
            MostarFormulaSi(ArrayFormulaNO);
        }
        else if (BanderaSi == 1) {
            var resul = ItemArrSi();
            if (resul == '2') {
                ArrayIF.push($("#TxtConstante").val());
                $("#TxtConstante").val("")
            }
            MostarFormulaSi(ArrayIF);

        }
    }
    $(document).on('click', '#BtnSumar', function () {

        if (!ArrayFormula.length == 0) {
            var Respuesta = ValidarSignos();
            ////console.log(Respuesta)
            if (Respuesta == 3) {
                ArrayFormula.push("+");
                MostarFormula(ArrayFormula, 0);
            }

        }

    });

    $(document).on('click', '#BtnRestar', function () {
        if (!ArrayFormula.length == 0) {
            var Respuesta = ValidarSignos();
           // //console.log(Respuesta)
            if (Respuesta == 3) {
                ArrayFormula.push("-");
                MostarFormula(ArrayFormula, 0);
            }

        }
    });

    $(document).on('click', '#BtnMultiplicar', function () {
        if (!ArrayFormula.length == 0) {
            var Respuesta = ValidarSignos();
            ////console.log(Respuesta)
            if (Respuesta == 3) {
                ArrayFormula.push("*");
                MostarFormula(ArrayFormula, 0);
            }

        }
    });

    $(document).on('click', '#BtnDividir', function () {

        if (!ArrayFormula.length == 0) {
            var Respuesta = ValidarSignos();
            ////console.log(Respuesta)
            if (Respuesta == 3) {

                //ArrayFormula.splice(0, 0, "(");
                //ArrayFormula.splice(ArrayFormula.length, 0, ")");
                ArrayFormula.splice(ArrayFormula.length, 0, "/");
                MostarFormula(ArrayFormula, 0);
            }

        }

    });


    $(document).on('click', '#Btndleft', function () {
        
        if (ArrayFormula.length == "0") {
            ArrayFormula.splice(0, 0, "(");
            MostarFormula(ArrayFormula, 0)
        }
        else
        {
            //var Respuesta = ValidarSignos();
            //console.log(Respuesta)
           //if (Respuesta == 3)
            //{
               //console.log("agregar")
                //console.log(ArrayFormula.length);
                ArrayFormula.splice(ArrayFormula.length, 0, "(");
                MostarFormula(ArrayFormula, 0);
            //}
        }
     });
        
    $(document).on('click', '#btnright', function () {
       
        if (!ArrayFormula.length == 0) {
            var Respuesta = ValidarSignos();
            //console.log(Respuesta)
            if (Respuesta == 3) {
            var valor2 = ArrayFormula.length - 1;
            //console.log("valor2-ValidarSignos " + ArrayFormula[valor2]);
            if (ArrayFormula[valor2] != "(")
            {
                ArrayFormula.splice(ArrayFormula.length, 0, ")");
                MostarFormula(ArrayFormula, 0);

            }
          }

        }

    });

    $(document).on('click', '#CkeckData', function () {
       
        if (!$(this).is(':checked')) {
            removeItemArr(ArrayFormula, $(this).val());
            RemoveCriterios(criteriosPorcentaje, $(this).val())
            $(this).attr('checked', false);
        }
        else
        {
            var resul = ItemArr($(this).val());
            ///$('#myCheckbox').attr('checked', false);
            ////console.log(resul);
            if (resul == '2') {
                ArrayFormula.push($(this).val());
                //$(this).attr('checked', true);
                AddCriterios($(this).val());
            }
            else { $(this).attr('checked', false) }
            MostarFormula(ArrayFormula, 0);
        }

    });

    $(document).on('click', '#BtnResetCumpli', function () {
        CumplimientoFinal = "";
        ArrayIF = [];
        ArrayFormulaSi = [];
        ArrayFormulaNO = [];
        MostarFormulaSi(ArrayIF);
        MostarFormulaSi(ArrayFormulaSi);
        MostarFormulaSi(ArrayFormulaNO);
        BanderaSi == 1;
        $("#CheckV").attr("checked", false);
        $("#CheckF").attr("checked", false);
        $("#inputCondicion").select();
        $("#inputCondicion").val("");
        $("#inputVerdadero").val("");
        $("#TxtFormulaCumplimiento").html();
        $("#inputFalso").val("");
        
    });

    //$("#inputCondicion").click(function () {
    //    //console.log(BanderaSi);
    //});

    //$("#inputCondicion")
    //.focusout(function () { //console.log("foco" + BanderaSi); });



    //$("#inputVerdadero")
    //.focusout(function () { //console.log("foco" + BanderaSi); BanderaSi = 2; });

    ////////Eventos Formula cumplimiento

    
    $(document).on('click', '.btn-addSi', function () {

           var resul = ItemArrSi($(this).attr("id"));
           ////console.log(BanderaSi + "BTN-Add") 
           if (BanderaSi == 0) {
              /// //console.log(" agregando Verdadero")
               var resul = ItemArrSi($(this).attr("id"));
               ////console.log(resul);
               if (resul == '2')
               {
                   ArrayFormulaSi.push($(this).attr("id"));
                   MostarFormulaSi(ArrayFormulaSi);
               }
               //$("#inputVerdadero").select();
           }
           else if(BanderaSi == 1)
           {
               ////console.log(" agregando condicion")
               var resul = ItemArrSi($(this).attr("id"));
               
               if (resul == '2')
               {
                   ArrayIF.push($(this).attr("id"));
               }
               //$("#inputCondicion").select();
               MostarFormulaSi(ArrayIF)
               //console.log(ArrayIF)
               $("#inputCondicion").select();
           }
           else if (BanderaSi == 2) {
               //console.log(" agregando Falso")
               var resul = ItemArrSi($(this).attr("id"));
               //console.log(resul)
               if (resul == '2') {
                   ArrayFormulaNO.push($(this).attr("id"));
               }
               $("#inputFalso").select();
               MostarFormulaSi(ArrayFormulaNO)
               //console.log(ArrayFormulaNO)
           }
        
    });

    $(document).on('click', '#BtnVerformula', function () {

        $("#ModalFormula").modal('show');
        MostarFormula(ArrayFormula, 0);
    });

    $(document).on('click', '#BtnVerformulaCumpli', function () {

        //$("#Formula").html("");
        $("#ModalFormula").modal('show');
        CargarFormulasIF()
    });
    
    $(document).on('click', '#BtnSi', function () {
        BanderaSi = 1;
        $("#BtnMayor").show();
        $("#BtnMenor").show();
        $("#BtnMayorque").show();
        $("#BtnMayorque").show();
        $("#TxtFormulaCumplimiento").html("");
        $("#BtnMenorque").show();
        $(".ParamIf").show("fade")
        $("#FP").show();
        $("#UM").show();
        $("#PS").show();
        $("#CheckV").attr("checked", false);
        $("#CheckF").attr("checked", false);
    });

    $(document).on('click', '#BtnMayor', function () {
        //console.log("mas si" + BanderaSi)
        if (BanderaSi == 1) {
            //console.log("mas si")
            if (!ArrayIF.length == 0) {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    ArrayIF.push(">");
                }
                MostarFormulaSi(ArrayIF);
                //console.log(ArrayIF)
            }

        }
    });
    
    $(document).on('click', '#BtnMenor', function () {
        //console.log("mas si" + BanderaSi)
        if (BanderaSi == 1) {
            //console.log("mas si")
            if (!ArrayIF.length == 0) {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    ArrayIF.push("<");
                }
                MostarFormulaSi(ArrayIF);
                //console.log(ArrayIF)
            }

        }
    });
 
    $(document).on('click', '#BtnMayorque', function () {
        //console.log("mas si" + BanderaSi)
        if (BanderaSi == 1) {
            //console.log("mas si")
            if (!ArrayIF.length == 0) {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    ArrayIF.push(">=");
                }
                MostarFormulaSi(ArrayIF);
                //console.log(ArrayIF)
            }

        }
    });

    $(document).on('click', '#BtnMenorque', function () {
        //console.log("mas si" + BanderaSi)
        if (BanderaSi == 1) {
            if (!ArrayIF.length == 0) {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    ArrayIF.push("<=");
                }
                MostarFormulaSi(ArrayIF);
                //console.log(ArrayIF)
            }

        }
    });

    $(document).on('click', '#BtnMenor', function () {
        //console.log("mas si"+BanderaSi)
        if (BanderaSi == 0)
        {
            //console.log("mas si")
            if (!ArrayIF.length == 0) {
            var Respuesta = ValidarSignosSi();
            //console.log(Respuesta)
            if (Respuesta == 3) {
                ArrayIF.push("+");
            }
            //console.log(ArrayIF)
          }

        }
    });

    $(document).on('click', '#BtnSumarCumpli', function () {

        switch (BanderaSi) {
            case 0:
                if (!ArrayFormulaSi.length == 0) {
                    var Respuesta = ValidarSignosSi();
                    //console.log(Respuesta)
                    if (Respuesta == 3) {
                        ArrayFormulaSi.push("+");
                    }
                    MostarFormulaSi(ArrayFormulaSi);
                }
                break
            case 1:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayIF.push("+");

                }
                MostarFormulaSi(ArrayIF);
                break
            case 2:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayFormulaNO.push("+");

                }
                MostarFormulaSi(ArrayFormulaNO);
                break

            default:
        }
    });

    $(document).on('click', '#BtnRestarCumpli', function () {

        switch (BanderaSi) {
            case 0:
                if (!ArrayFormulaSi.length == 0) {
                    var Respuesta = ValidarSignosSi();
                    //console.log(Respuesta)
                    if (Respuesta == 3) {
                        ArrayFormulaSi.push("-");
                    }
                    MostarFormulaSi(ArrayFormulaSi);
                }
                break
            case 1:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayIF.push("-");

                }
                MostarFormulaSi(ArrayIF);
                break
            case 2:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayFormulaNO.push("-");

                }
                MostarFormulaSi(ArrayFormulaNO);
                break

            default:
        }
    });

    $(document).on('click', '#BtnMultiplicarCumpli', function () {
        switch (BanderaSi)
        {
            case 0:
                if (!ArrayFormulaSi.length == 0) {
                    var Respuesta = ValidarSignosSi();
                    //console.log(Respuesta)
                    if (Respuesta == 3) {
                        ArrayFormulaSi.push("*");
                    }
                    MostarFormulaSi(ArrayFormulaSi);
                }
                break
            case 1:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayIF.push("*");

                }
                MostarFormulaSi(ArrayIF);
                break
            case 2:
                var Respuesta = ValidarSignosSi();
                //console.log(BanderaSi)
                if (Respuesta == 3) {
                    ArrayFormulaNO.push("*");

                }
                MostarFormulaSi(ArrayFormulaNO);
                break

            default:
          }
         
    });

    $(document).on('click', '#BtnDividirCumpli', function () {

        if (BanderaSi=="0")
        {
            if (!ArrayFormulaSi.length == 0) {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {

                    //ArrayFormulaSi.splice(0, 0, "(");
                    //ArrayFormulaSi.splice(ArrayFormulaSi.length, 0, ")");
                    ArrayFormulaSi.splice(ArrayFormulaSi.length, 0, "/");


                }

            }
            MostarFormulaSi(ArrayFormulaSi);
        } else if (BanderaSi == 2) {
            var Respuesta = ValidarSignosSi();
            //console.log(BanderaSi)

            if (Respuesta == 3) {
                //ArrayFormulaNO.splice(0, 0, "(");
                //ArrayFormulaNO.splice(ArrayFormulaNO.length, 0, ")");
                ArrayFormulaNO.splice(ArrayFormulaNO.length, 0, "/");

            }
            MostarFormulaSi(ArrayFormulaNO);
        } else if (BanderaSi == 1) {
            var Respuesta = ValidarSignosSi();
            //console.log(BanderaSi)

            if (Respuesta == 3) {
                ArrayIF.splice(ArrayIF.length, 0, "/");

            }
            MostarFormulaSi(ArrayIF);
        }
        

    });

    $(document).on('click', '#btn-cargar', function () { CargarFormulasIF();});

    $(document).on('click', '#CheckV', function () {

        //console.log(ArrayIF.length)
        if (ArrayIF.length == 0)
        {
            //console.log("!=0")
            BanderaSi = 1;
            $("#CheckV").attr("checked", false);
        }
        else
        {
            //console.log("oto")
            BanderaSi = 0;
            $("#CheckV").attr("checked", true);
            $("#CheckF").attr("checked", false);
        }
        
    });

    $(document).on('click', '#CheckF', function () {

        if (ArrayFormulaSi.length == 0)
        {
            BanderaSi = 1;
            $("#CheckF").attr("checked", false);
        } else
        {
            BanderaSi = 2;
            $("#CheckF").attr("checked", true);
            $("#CheckV").attr("checked", false);
        }
        
    });

    $(document).on('click', '#BtndleftCumpli', function () {
        //console.log(BanderaSi);
        if (BanderaSi == "0") {
            if (ArrayFormulaSi.length == "0") {
                ArrayFormulaSi.splice(0, 0, "(");
                MostarFormulaSi(ArrayFormulaSi, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayFormulaSi.length);
                    ArrayFormulaSi.splice(ArrayFormulaSi.length, 0, "(");
                    MostarFormulaSi(ArrayFormulaSi);
                }
            }
        } else if (BanderaSi == 2)
        {
            if (ArrayFormulaNO.length == "0") {
                ArrayFormulaNO.splice(0, 0, "(");
                MostarFormulaSi(ArrayFormulaNO, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayFormulaNO.length);
                    ArrayFormulaNO.splice(ArrayFormulaNO.length, 0, "(");
                    MostarFormulaSi(ArrayFormulaNO);
                }
            }
        
        } else if (BanderaSi == 1) {
            if (ArrayIF.length == "0") {
                ArrayIF.splice(0, 0, "(");
                MostarFormulaSi(ArrayIF, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayIF.length);
                    ArrayIF.splice(ArrayIF.length, 0, "(");
                    MostarFormulaSi(ArrayIF);
                }
            }

        }

    });

    $(document).on('click', '#BtnrightCumpli', function () {
        if (BanderaSi == "0") {
            if (ArrayIF.length == "0") {
                ArrayFormulaSi.splice(0, 0, ")");
                MostarFormulaSi(ArrayFormulaSi, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayFormulaSi.length);
                    ArrayFormulaSi.splice(ArrayFormulaSi.length, 0, ")");
                    MostarFormulaSi(ArrayFormulaSi);
                }
            }
        } else if (BanderaSi == 2) {
            if (ArrayFormulaNO.length == "0") {
                ArrayFormulaNO.splice(ArrayIF.length, 0, ")");
                MostarFormulaSi(ArrayFormulaNO, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayFormulaNO.length);
                    ArrayFormulaNO.splice(ArrayFormulaNO.length, 0, ")");
                    MostarFormulaSi(ArrayFormulaNO);
                }
            }

        } else if (BanderaSi == 1) {
            if (ArrayIF.length == "0") {
                ArrayIF.splice(0, 0, ")");
                MostarFormulaSi(ArrayIF, 0)
            }
            else {
                var Respuesta = ValidarSignosSi();
                //console.log(Respuesta)
                if (Respuesta == 3) {
                    //console.log(ArrayIF.length);
                    ArrayIF.splice(ArrayIF.length, 0, ")");
                    MostarFormulaSi(ArrayIF);
                }
            }

        }
    });

    
    //// funciones 

    function CargarFormulasIF() {
        var strinCondi = "";
        $.each(ArrayIF, function (i, item) {
            strinCondi += ArrayIF[i];
        });

        var stringverdadero = "";
        $.each(ArrayFormulaSi, function (i, item) {
            stringverdadero += ArrayFormulaSi[i];
        });

        var stringfalso = "";
        $.each(ArrayFormulaNO, function (i, item) {
            stringfalso += ArrayFormulaNO[i];
        });

        var Formula = "SI(" + strinCondi + ";" + stringverdadero + ";" + stringfalso + ")";
        $("#TxtFormulaCumplimiento").html(Formula);
        CumplimientoFinal = Formula;
        $("#Formula").html(Formula);
    }


    function LoadF() {
        var strinCondi = "";
        $.each(ArrayIF, function (i, item) {
            strinCondi += ArrayIF[i];
        });
        $("#inputCondicion").val(strinCondi);
        var stringverdadero = "";
        $.each(ArrayFormulaSi, function (i, item) {
            stringverdadero += ArrayFormulaSi[i];
        });
        $("#inputVerdadero").val(stringverdadero);
        var stringfalso = "";
        $.each(ArrayFormulaNO, function (i, item) {
            stringfalso += ArrayFormulaNO[i];
        });
        $("#inputFalso").val(stringfalso);
    }

    function CombinarArray() {
        if (BanderaSi == 0) {
            $.each(ArrayFormula, function (i, item) {
                ArrayFormulaSi.push(ArrayFormula[i]);
            })
        } else if (BanderaSi == 2) {

            $.each(ArrayFormula, function (i, item) {
                ArrayFormulaNO.push(ArrayFormula[i]);
            })
        } else if (BanderaSi == 1) {

            $.each(ArrayFormula, function (i, item) {
                ArrayIF.push(ArrayFormula[i]);
            })
        }
    }


    var modalConfirm = function (callback) {

        $("#btn-Enviar").on("click", function () {
            $("#mi-modal").modal('show');
        });

        $("#modal-btn-si").on("click", function () {
            callback(true);
            $("#mi-modal").modal('hide');
        });

        $("#modal-btn-no").on("click", function () {
            callback(false);
            $("#mi-modal").modal('hide');
        });
    };

    modalConfirm(function (confirm) {
        var dataCriterios = CriteriosPorcentajeString();
        if (confirm)
        {
              var data =
               {
                   "IdServicio": $("#CombServicios").val(),
                   "IdIndicador": $("#CombIndicadores").val(),
                   "FormulaPorcentaje": stringFormFin,
                   "FormulaCumplimiento": CumplimientoFinal,
                   "Criterios": dataCriterios,
                   "FromArray": ArrayFormula,
                   "ArrayIF":ArrayIF,
                   "ArrayVerdadero":ArrayFormulaSi,
                   "ArrayFalso":ArrayFormulaNO
               };
              var Eval = ItemArr();
              var Parantesis = validarParen();
              var ParenIf = validarParenIf();
              var ParenSi = validarParenVerdadero();
              var ParenNo = validarParenFalso();
              var ItemVerdadero = ValidarVerdaderoItem();
              var ItemIf = ValidarIFItem();
              var Itemfalso = ValidarFalso();
              //console.log(ItemVerdadero);
             
              if (Parantesis == '2' || Eval == '2')
              {
                  alert("Por favor valide el formato de la fórmula de porcentajes ingresada");
             
              }
              else if (ParenIf == 2 || ParenSi == 2 || ParenNo == 2)
              {
                  alert("Por favor valide el formato de la fórmula de Cumplimiento ingresada");
              } else if (ItemIf == 2 || ItemVerdadero == 2 || Itemfalso==2)
              {
                  alert("Por favor valide el formato de la fórmula de Cumplimiento ingresada");
              }
              else { EnviarDatos(data); }
              
        } else
        {
            //Acciones si el usuario no confirma
            confirmacion = false;
        }
    });

    function MostarFormula(arrayform, ReserString) {
        var stringForm = "";
        //console.log("MostarFormula" + arrayform.length);

        $.each(arrayform, function (i, item) {
            stringForm += arrayform[i];
        });
        //console.log(stringForm);
        $("#TxtFormulaPorcentajes").val(stringForm);
        stringFormFin = stringForm;
        $("#Formula").html(stringForm);
    }

    function validarParen() {
        var contadoraper = 0;
        var contadorCierre = 0;

        console.log(ArrayFormula.length);
        if (ArrayFormula.length <= 0) {

            return 3;
        }
        else
        {
            $.each(ArrayFormula, function (i, item) {
                if (ArrayFormula[i] == "(") { contadoraper = contadoraper + 1; }
                else if (ArrayFormula[i] == ")") { contadorCierre = contadorCierre + 1; }
            });

            //console.log("apertura" + contadoraper + "" + "cIERREE" + contadorCierre);
            if (contadoraper == contadorCierre) {
                return 3;
            } else { return 2; }

            if (contadorCierre == contadoraper) {
                return 3;
            } else { return 2; }
        }
        
    }

    function validarParenIf() {
        var contadoraper = 0;
        var contadorCierre = 0;
        $.each(ArrayIF, function (i, item) {
            if (ArrayIF[i] == "(") { contadoraper = contadoraper + 1; }
            else if (ArrayIF[i] == ")") { contadorCierre = contadorCierre + 1; }
        });

        //console.log("apertura" + contadoraper + "" + "cIERREE" + contadorCierre);
        if (contadoraper == contadorCierre) {
            return 3;
        } else { return 2; }
    }

    function validarParenVerdadero() {
        var contadoraper = 0;
        var contadorCierre = 0;
        $.each(ArrayFormulaSi, function (i, item) {
            if (ArrayFormulaSi[i] == "(") { contadoraper = contadoraper + 1; }
            else if (ArrayFormulaSi[i] == ")") { contadorCierre = contadorCierre + 1; }
        });

        //console.log("apertura" + contadoraper + "" + "cIERREE" + contadorCierre);
        if (contadoraper == contadorCierre) {
            return 3;
        } else { return 2; }
    }

    function validarParenFalso() {
        var contadoraper = 0;
        var contadorCierre = 0;
        $.each(ArrayFormulaNO, function (i, item) {
            if (ArrayFormulaNO[i] == "(") { contadoraper = contadoraper + 1; }
            else if (ArrayFormulaNO[i] == ")") { contadorCierre = contadorCierre + 1; }
        });

        ////console.log("apertura" + contadoraper + "" + "cIERREE" + contadorCierre);
        if (contadoraper == contadorCierre) {
            return 3;
        } else { return 2; }
    }



    function CriteriosPorcentajeString()
    {
        var Criterios="";
        if (criteriosPorcentaje.length > 0)
        {
            var a = criteriosPorcentaje.length - 1;
            ////console.log(criteriosPorcentaje[a])
            
                $.each(criteriosPorcentaje, function (index, item)
                {
                    Criterios += criteriosPorcentaje[index]+",";
                });
         
        }
        return Criterios;
    }

    function AddCriterios(item)
    {
        criteriosPorcentaje.push(item);
       // //console.log("cri"+criteriosPorcentaje)
    }

    function RemoveCriterios(arraycriterios,item)
    {
        var i = arraycriterios.indexOf(item);

        if (i !== -1 || i != 0)
        {
            arraycriterios.splice(i, 1);
        }
        ////console.log("cri" + criteriosPorcentaje)
    }

    function RemoverUitem() {
        ArrayFormula.pop();
        MostarFormula(ArrayFormula, 0);
        if (ArrayFormula.length == 0)
        {
            $("input[type=checkbox]").attr("checked", false);
        }
    }

    function RemoveritemIf() {
       // //console.log(BanderaSi)
        if (BanderaSi == 0)
        {
            ArrayFormulaSi.pop();
            MostarFormulaSi(ArrayFormulaSi);

        } else if (BanderaSi == 2)
        {
            ArrayFormulaNO.pop();
            MostarFormulaSi(ArrayFormulaNO);
        }
        else if (BanderaSi == 1)
        {
            ArrayIF.pop();
            MostarFormulaSi(ArrayIF);
         
        }
        
    }

    function removeItemArr(arr, item) {

        var i = arr.indexOf(item);
        if (arr[i - 1] == '(') {
            /////console.log("(arr[i - 1] == '(')")
            arr.splice(i, 2);
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }

        if (i !== -1 || i != 0) {
            i = i - 1;
            ////console.log("entro si" + arr[i - 1])
            arr.splice(i, 2);
        }
        if (arr[i - 1] == '/') {
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }
        if (arr[i - 1] == ')') {
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }
        MostarFormula(ArrayFormula, 0);

    }

    function ItemArr() {
        var valor2 = ArrayFormula.length - 1;
        //console.log("ItemAr -ItemArr " +ArrayFormula.length);
        //console.log("ItemAr -ItemArr " + ArrayFormula[valor2]);
        if (valor2 < '0') {
            
            return 2;
        }
        else if (ArrayFormula[valor2] == '+'
            || ArrayFormula[valor2] == '-'
            || ArrayFormula[valor2] == '*'
            || ArrayFormula[valor2] == '/'
            || ArrayFormula[valor2] == '('
            || ArrayFormula[valor2] == ')')
        { return 2; }
        else
        { return 3; }


    }

    function ValidarSignos() {
        var valor2 = ArrayFormula.length - 1;
        ////console.log("valor2-ValidarSignos " + ArrayFormula[valor2]);
        if (
            ArrayFormula[valor2] == '+'
            || ArrayFormula[valor2] == '-'
            || ArrayFormula[valor2] == '*'
            || ArrayFormula[valor2] == '/'
            //|| ArrayFormula[valor2] == ')'
            // ||ArrayFormula[valor2] == '('
            )
        { return 2; } else { return 3; }

    }

    ////Validaiones Si

        function ValidarVerdaderoItem()
         {

            var valor2 = ArrayFormulaSi.length - 1;
                if (ArrayFormulaSi.length == 0) {
                    return 2;
                } else if (ArrayFormulaSi[valor2] == '+' ||
                    ArrayFormulaSi[valor2] == '-'
                    || ArrayFormulaSi[valor2] == '*'
                    || ArrayFormulaSi[valor2] == '/'
                    || ArrayFormulaSi[valor2] == '>'
                    || ArrayFormulaSi[valor2] == '<'
                    || ArrayFormulaSi[valor2] == '<='
                    || ArrayFormulaSi[valor2] == '>='
                    || ArrayFormulaSi[valor2] == ')'
                    || ArrayFormulaSi[valor2] == '('
                    )
                { return 2; }
                else
                { return 3; }
            };

    function ValidarFalso()
            {
                var valorNO = ArrayFormulaNO.length - 1;
                if (ArrayFormulaNO.length == 0) {
                    return 2;
                } else if (ArrayFormulaNO[valorNO] == '+' ||
                    ArrayFormulaNO[valorNO] == '-'
                    || ArrayFormulaNO[valorNO] == '*'
                    || ArrayFormulaNO[valorNO] == '/'
                    || ArrayFormulaNO[valorNO] == '>'
                    || ArrayFormulaNO[valorNO] == '<'
                    || ArrayFormulaNO[valorNO] == '<='
                    || ArrayFormulaNO[valorNO] == '>='
                    || ArrayFormulaNO[valorNO] == ')'
                    || ArrayFormulaNO[valorNO] == '('
                    )
                { return 2; }
                else
                { return 3; }
            };
    function ValidarIFItem()
    {
        var valor = ArrayIF.length - 1
                if (ArrayIF.length == 0) {
                    return 2;
                } else if (ArrayIF[valor] == '+' ||
                ArrayIF[valor] == '-'
                || ArrayIF[valor] == '*'
                || ArrayIF[valor] == '/'
                || ArrayIF[valor] == '>'
                || ArrayIF[valor] == '<'
                || ArrayIF[valor] == '<='
                || ArrayIF[valor] == '>='
                || ArrayIF[valor] == ')'
                || ArrayIF[valor] == '('
                    )
                { return 2; }
                else
                { return 3; }
            };

    function MostarFormulaSi(arrayformSi) {
        var stringFormSi = "";
        
        if (BanderaSi == 0)
        {
            $.each(arrayformSi, function (i, item) {
                stringFormSi += arrayformSi[i];
            });
            $("#TxtFormulaCumplimiento").html(stringFormSi);
            $("#inputVerdadero").val(stringFormSi);
            ////console.log(stringFormSi)
        } else if (BanderaSi == 1)
        {
            var ifstring = "";
            $.each(arrayformSi, function (i, item)
            {
                ifstring += arrayformSi[i];
            });

            $("#TxtFormulaCumplimiento").html(ifstring);
            $("#inputCondicion").val(ifstring);
        } else if (BanderaSi == 2) {
            var NOstring = "";
            $.each(ArrayFormulaNO, function (i, item) {
                NOstring += ArrayFormulaNO[i];
            });

            $("#inputFalso").val(NOstring);
        }
    }

    function removeItemArrSI(arr, item) {

        var i = arr.indexOf(item);
        if (arr[i - 1] == '(') {
            /////console.log("(arr[i - 1] == '(')")
            arr.splice(i, 2);
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }

        if (i !== -1 || i != 0) {
            i = i - 1;
            ////console.log("entro si" + arr[i - 1])
            arr.splice(i, 2);
        }
        if (arr[i - 1] == '/') {
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }
        if (arr[i - 1] == ')') {
            arr.removeItem('/');
            arr.removeItem('(');
            arr.removeItem(')');
        }
        MostarFormula(ArrayFormula, 0);

    }

    function ItemArrSi() {
        
        if (BanderaSi == 0)
        {
            var valor2 = ArrayFormulaSi.length - 1;
            
            if (ArrayFormulaSi.length == 0) {
                return 2;
            } else if (ArrayFormulaSi[valor2] == '+' ||
                ArrayFormulaSi[valor2] == '-'
                || ArrayFormulaSi[valor2] == '*'
                || ArrayFormulaSi[valor2] == '/'
                || ArrayFormulaSi[valor2] == '>'
                || ArrayFormulaSi[valor2] == '<'
                || ArrayFormulaSi[valor2] == '<='
                || ArrayFormulaSi[valor2] == '>='
                || ArrayFormulaSi[valor2] == ')'
                || ArrayFormulaSi[valor2] == '('
                )
            { return 2; }
            else
            { return 3; }
        }else if (BanderaSi == 1)
        {
            var valor = ArrayIF.length - 1
            /////console.log(valor)
            if (ArrayIF.length ==0)
            {
                return 2;
            } else if (ArrayIF[valor] == '+' ||
            ArrayIF[valor] == '-'
            || ArrayIF[valor] == '*'
            || ArrayIF[valor] == '/'
            || ArrayIF[valor] == '>'
            || ArrayIF[valor] == '<'
            || ArrayIF[valor] == '<='
            || ArrayIF[valor] == '>='
            || ArrayIF[valor] == ')'
            || ArrayIF[valor] == '('
                )
        { return 2; }
        else
        { return 3; }
        } else if (BanderaSi == 2)
        {
            var valorNO = ArrayFormulaNO.length - 1;
            if (ArrayFormulaNO.length == 0)
            {
                return 2;
            } else if (ArrayFormulaNO[valorNO] == '+' ||
                ArrayFormulaNO[valorNO] == '-'
                || ArrayFormulaNO[valorNO] == '*'
                || ArrayFormulaNO[valorNO] == '/'
                || ArrayFormulaNO[valorNO] == '>'
                || ArrayFormulaNO[valorNO] == '<'
                || ArrayFormulaNO[valorNO] == '<='
                || ArrayFormulaNO[valorNO] == '>='
                || ArrayFormulaNO[valorNO] == ')'
                || ArrayFormulaNO[valorNO] == '('
                )
            { return 2; }
            else
            { return 3; }
        }


    }

    function ValidarSignosSi() {

        if (BanderaSi == 1)
        {
            var valor2 = ArrayIF.length - 1;
            //console.log(valor2)
            if (ArrayIF[valor2] == '+'
                || ArrayIF[valor2] == '-'
                || ArrayIF[valor2] == '*'
                || ArrayIF[valor2] == '/'
                //|| ArrayIF[valor2] == ')'
                // || ArrayIF[valor2] == '('
                || ArrayIF[valor2] == '>'
                || ArrayIF[valor2] == '<'
                || ArrayIF[valor2] == '<='
                || ArrayIF[valor2] == '>=')
            { return 2; }
            else
            { return 3; }
        } else if (BanderaSi == 0)
        {
            var valor2 = ArrayFormulaSi.length - 1;
            ////console.log(valor2)
            ////console.log("valor2-ValidarSignosSI " + ArrayFormulaSi[valor2]);
            if (
                ArrayFormulaSi[valor2] == '+'
                || ArrayFormulaSi[valor2] == '-'
                || ArrayFormulaSi[valor2] == '*'
                || ArrayFormulaSi[valor2] == '/'
                //|| ArrayFormulaSi[valor2] == ')'
                //|| ArrayFormulaSi[valor2] == '('
                //|| ArrayFormulaSi[valor2] == '>'
                || ArrayFormulaSi[valor2] == '<'
                || ArrayFormulaSi[valor2] == '<='
                || ArrayFormulaSi[valor2] == '>=')
            { return 2; } else { return 3; }
        } else if (BanderaSi == 2) {
            var valorNO = ArrayFormulaNO.length - 1;

            if (ArrayFormulaNO.length == 0) {
                return 2;
            } else if (ArrayFormulaNO[valorNO] == '+' ||
                ArrayFormulaNO[valorNO] == '-'
                || ArrayFormulaNO[valorNO] == '*'
                || ArrayFormulaNO[valorNO] == '/'
                || ArrayFormulaNO[valorNO] == '>'
                || ArrayFormulaNO[valorNO] == '<'
                || ArrayFormulaNO[valorNO] == '<='
                || ArrayFormulaNO[valorNO] == '>='
                )
            { return 2; }
            else
            { return 3; }
        }
        

    }

    function AjaxIdicadoSeric(IdServicios, IdDireccion) {
        $.ajax({
            url: '/FormCumplimientoPorcen/GetDatosIndicadorXServicio',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ IdServicios: IdServicios,IdDireccion:IdDireccion }),
            success: function (entidad) {
                ///var pr = jQuery.parseJSON(entidad)
                ////console.log(JSON.stringify(entidad));
                LoadIndicadores(entidad);
                $("#CombIndicadores").attr("readonly", false)
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });

    }

    function LoadIndicadores(array) {
        $("#CombIndicadores").html("");
        $("#CombIndicadores").append('<option data-subtext="">Seleccione</option>');

        $.each(array, function (index, item) {
            $("#CombIndicadores").append('<option value="' + item.IdIndicador + '">' + item.IdIndicador + '</option>');
        });

    }

    function AjaxIdicadorCriterio(IdIndicador) {
        $.ajax({
            url: '/FormCumplimientoPorcen/GetCriteriosXIndicador',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ IdIndicador: IdIndicador }),
            success: function (entidad) {
                ///var pr = jQuery.parseJSON(entidad)
                ////console.log(JSON.stringify(entidad));
                LoadCriterio(entidad);
                $(".criterios").show();
                
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });

    }

    function LoadCriterio(array) {

        var Usuario = "";
        var FechaMod =""
        if (array.length > 0)
        {
            $(".formulaPocentaje").show("fade");
            $(".formulaCumplimiento").show("fade");
        } else
        {
            $(".formulaPocentaje").hide("fade");
            $(".formulaCumplimiento").hide("fade");
            $("#TxtFormulaPorcentajes").val("");
        }
        var Formula = "";
        var FrmArrayIF = "";
        var FrmArrayVerdadero = "";
        var FrmArrayFalso = "";
        $("#ResultLisCriterio").html("");
        var tabla = '<table class="table table-striped table-bordered table-hover dataTables" id="dexample">';
        tabla += '<thead>';
        tabla += '<tr>';
        tabla += '<th align="right"></th>';
        tabla += '<th align="right" style="width: 10px;"></th>';
        tabla += '<th align="right">Codigo</th>';
        tabla += '<th align="right">Descripcion</th>';
        tabla += '</tr>';
        tabla += '</thead>';
        tabla += '<tbody>';
        $.each(array, function (i, item) {
            Formula = item.FromArray;
            FrmArrayIF = item.ArrayIF;
            FrmArrayVerdadero = item.ArrayVerdadero;
            FrmArrayFalso = item.ArrayFalso;
            Usuario = item.Usuario;
            if (item.FechaUltimaActualizacion == "1/1/1900 12:00:00 AM")
            {
                FechaMod = "";
            } else { FechaMod = item.FechaUltimaActualizacion; }
            
            tabla += '<tr class="" align="left">';
            if (item.CriteriosCkech!='0')
            {
                
                ////console.log(item.CriteriosCkech)
                tabla += '<td><input type="checkbox" value="' + item.IdCriterio + '"  id="CkeckData" checked="checked"></td>';
                tabla += '<td><button type="button" class="btn btn-succes btn-xs btn-addSi" id="' + item.IdCriterio + '" ><span class="glyphicon glyphicon-plus"></span></button></td>';
                //$("input:checkbox[id='CkeckData']").prop('checked', true);
                //$('#CkeckData').prop('checked', true);
                AddCriterios(item.IdCriterio)
            }
            else
            {
                ////console.log("NO"+item.CriteriosCkech)
                tabla += '<td><input type="checkbox" value="' + item.IdCriterio + '"  id="CkeckData"></td>';
                tabla += '<td><button type="button" class="btn btn-succes btn-xs btn-addSi" id="'+item.IdCriterio+'" ><span class="glyphicon glyphicon-plus"></span></button></td>';
            }
            
            tabla += '<td>' + item.IdCriterio + '</td>';
            tabla += '<td>' + item.DescCriterio + '</td>';
            tabla += ' </tr>';
        })
        tabla += '</tbody>';
        tabla += '</table>';
        MostarFormula(Formula, '0')
        ArrayFormula = Formula;
        ArrayIF = FrmArrayIF;
        ArrayFormulaSi = FrmArrayVerdadero;
        ArrayFormulaNO = FrmArrayFalso;
        $("#ResultLisCriterio").append(tabla);
        //$('#dexample').dataTable
        //    ({
        //        "processing": true,
        //    });
        CargarFormulasIF();
        LoadF();
        $("#Usuario").text(Usuario);
        $("#FechaMod").text(FechaMod);
    }

    Array.prototype.removeItem = function (a) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == a) {
                for (var i2 = i; i2 < this.length - 1; i2++) {
                    this[i2] = this[i2 + 1];
                }
                this.length = this.length - 1;
                return;
            }
        }
    };

    function EnviarDatos(Mimodel) {
        var mensaje;
        $("#Confirmacion").html("");
        $.ajax({
            url: '/FormCumplimientoPorcen/CrearParamFormulas',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ Mimodel: Mimodel }),
            success: function (entidad) {
                
                $(".darkScreen").fadeOut(100);
                $("#exampleModalCenter").modal("hide");

                $("#Confirmacion").show("slow");
                $('#Confirmacion').fadeIn();
                setTimeout(function () { $("#Confirmacion").fadeOut() }, 5000);
                if (entidad[1] == "1") {
                    mensaje = "<strong >Mensaje de Confirmación! </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-info alert-dismissible fade in");

                }
                else if (entidad[1] == "2") {
                    mensaje = "<strong >Mensaje de Confirmación! </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-success alert-dismissible fade in");
                }
                else if (entidad[1] == "3") {
                    mensaje = "<strong >Mensaje de Confirmación! </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-danger alert-dismissible fade in");
                }
                $("#Confirmacion").append(mensaje);
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });
    }

});
