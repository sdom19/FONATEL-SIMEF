const TipoContenidoDetalle = {
    TituloPrincipal: 1,
    Subtitulo: 2,
    Descripcion: 3,
    Imagen: 4,
    CarpetaInformes: 5
}

const Etiquetas = {
    Visualizar: "Visualizar",
    DescargarPPTX: "Descargar presentación",
    DescargarPDF: "Descargar informe"
}

function MostrarTextoImagen(control, idPantalla){
    try{
        let url = jsconstantes.variables.direccionApi + "TextoPantalla/ObtenerTextoPantalla/" + idPantalla;
        fetch(url)
            .then(res => res.json())
            .then(datos => {
                if(datos.length > 0){
                    let html = "";
                    datos = datos.filter(d=> d.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Descripcion || d.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Imagen).sort(function(a,b){return a.orden - b.orden})
                    for(let i =0; i < datos.length; i++){
                        let item = datos[i];
                        html = html + "<div style='margin-bottom:25px;'>";
                        if(item.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Imagen){
                            html = html + "<div><img style='max-height:300px;' src='"+ jsconstantes.variables.direccionSITEL + item.rutaImagen + "' /></div>";
                        }else{
                            html = html + item.texto;
                        }
                        html = html + "</div>"
                    }
                    jQuery(control).html(html);
                }
            })
    }catch(ex){
        console.log(ex);
    }
}

function MostrarTituloPantalla(control, idPantalla){
    try{
        let url =  jsconstantes.variables.direccionApi + "TextoPantalla/ObtenerTextoPantalla/" + idPantalla;
        fetch(url)
            .then(res => res.json())
            .then(datos => {
                if(datos.length > 0){
                    let html = "";
                    datos = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.TituloPrincipal).sort(function(a,b){return a.orden - b.orden});
                    for(let i =0; i < datos.length; i++){
                        let item = datos[i];
                        html = html + item.texto + "<br>";
                    }
                    jQuery(control).html(html);
                }
            })
    }catch(ex){
        console.log(ex)
    }
}

function MostrarSubTituloPantalla(control, idPantalla){
    try{
        let url =  jsconstantes.variables.direccionApi + "TextoPantalla/ObtenerTextoPantalla/" + idPantalla;
        fetch(url)
            .then(res => res.json())
            .then(datos => {
                if(datos.length > 0){
                    let html = "";
                    datos = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Subtitulo).sort(function(a,b){return a.orden - b.orden});
                    for(let i =0; i < datos.length; i++){
                        let item = datos[i];
                        html = html + item.texto + "<br>";
                    }
                    jQuery(control).html(html);
                }
            })
    }catch(ex){
        console.log(ex)
    }
}

function MostrarBlogGraficosInteractivos(control, idPantalla){
    try{
        let url =  jsconstantes.variables.direccionApi + "TextoPantalla/ObtenerTextoPantalla/" + idPantalla;
        fetch(url)
            .then(res => res.json())
            .then(datos => {
                let imagenes = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Imagen).sort(function(a,b){return a.orden - b.orden});
                let titulos = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Subtitulo).sort(function(a,b){return a.orden - b.orden});
                let descripcion = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.Descripcion).sort(function(a,b){return a.orden - b.orden});
                let carpetaInformes = datos.filter(i => i.idTipoContenidoTextoSIGITEL == TipoContenidoDetalle.CarpetaInformes).sort(function(a,b){return a.orden - b.orden});
                let html = "";

                let cantidadFilas = Math.max(imagenes.length, titulos.length, descripcion.length, carpetaInformes.length);

                for (let i=0; i < cantidadFilas; i++){
                    let srcImagen = "";
                    let desc = "";
                    let subtitulo = ""
                    let nombreCarpeta = "";

                    if(imagenes.length > i){
                        srcImagen = jsconstantes.variables.direccionSITEL + imagenes[i].rutaImagen;
                    }

                    if(descripcion.length > i){
                        desc = descripcion[i].texto;
                    }

                    if(titulos.length > i){
                        subtitulo = titulos[i].texto;
                    }

                    if (carpetaInformes.length > i) {
                        nombreCarpeta = carpetaInformes[i].texto;
                    }

                    html = html + `
                        <div class="row">
                            <div class="col-lg-6">   
                                    <img src="${srcImagen}" alt="img"style="max-width: 400px; max-height:300px;" >   
                                </div>
                                <div class="col-lg-6 align-self-center">
                                <h3>${subtitulo}</h3>
                                <h5 style="color:#555!important; text-align: left;">${desc}</h5>
                                <hr>
                                    <a class="btn btn-blue" href="descargagrafico.html" style="color: #ffffffff;">${Etiquetas.Visualizar}</a>
                                    <a class="btn btn-blue" onclick="descargarPPTX('${nombreCarpeta}')"  style="color: #ffffffff;">${Etiquetas.DescargarPPTX}</a>
                                    <a class="btn btn-blue" onclick="descargarPDF('${nombreCarpeta}')" style="color: #ffffffff;">${Etiquetas.DescargarPDF}</a>
                            </div>        
                        </div> 
                        <br/>
                    `;
                }

                jQuery(control).html(html);
            })
    }catch(ex){
        console.log(ex)
    }
}