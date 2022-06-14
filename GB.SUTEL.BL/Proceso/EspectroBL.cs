using GB.SUTEL.DAL.Proceso;
using GB.SUTEL.Entities.Espectro;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using GB.SUTEL.BL.Seguridad;

namespace GB.SUTEL.BL.Proceso
{

    /// <summary>
    /// Este controlador maneja las acciones de carga y reemplazo de archivos
    /// </summary>
    /// <createddate>28/02/2016</createddate>
    /// <creator>Kevin Solís</creator>
    /// <lastmodificationdate>28/03/2016</lastmodificationdate

    public class EspectroBL : LocalContextualizer
    {

        #region atributos
        EspectroAD espectroDA;
        List<ArchivoCsvModel> listaArchivosCsv;
        List<ArchivoMifModel> listaArchivosMif;
        List<ArchivoTelevisionDigitalCsv> listaArchivoTvDigital;
        List<ArchivoIMTFijasCsv> listaArchivoIMTFijas;
        List<ArchivoBandaAngostaCsv> listaArchivoBandaA;
        ArchivoCsvModel archivoCsv;
        ArchivoMifModel archivoMif;
        ArchivoTelevisionDigitalCsv archivoTvDigital;
        ArchivoIMTFijasCsv archivoIMTFijas;
        ArchivoBandaAngostaCsv archivoBandaA;
        public List<Respuesta<ArchivoCsvModel>> listaRespuestasCsv { get; set; }
        public List<Respuesta<ArchivoMifModel>> listaRespuestasMif { get; set; }
        public List<Respuesta<ArchivoTelevisionDigitalCsv>> listaRespuestasCsvETL { get; set; }
        public List<Respuesta<ArchivoIMTFijasCsv>> listaRespuestasCsvIMTFijas { get; set; }
        public List<Respuesta<ArchivoBandaAngostaCsv>> listaRespuestasCsvBandaA { get; set; }
        string[] arrayValoresArchivo;
        BitacoraBL refBitacoraBL;

        #endregion


        public EspectroBL(ApplicationContext appContext)
            : base(appContext)
        {
            listaArchivosCsv = new List<ArchivoCsvModel>();
            listaArchivosMif = new List<ArchivoMifModel>();
            listaArchivoTvDigital = new List<ArchivoTelevisionDigitalCsv>();
            listaArchivoIMTFijas = new List<ArchivoIMTFijasCsv>();
            listaArchivoBandaA = new List<ArchivoBandaAngostaCsv>();
            archivoCsv = new ArchivoCsvModel();
            archivoMif = new ArchivoMifModel();
            archivoTvDigital = new ArchivoTelevisionDigitalCsv();
            archivoIMTFijas = new ArchivoIMTFijasCsv();
            archivoBandaA = new ArchivoBandaAngostaCsv();
            //archivoTvDigital.listaDetalle = new List<DetalleArchivoTelevisionDigitalCsv>();
            listaRespuestasCsv = new List<Respuesta<ArchivoCsvModel>>();
            listaRespuestasMif = new List<Respuesta<ArchivoMifModel>>();
            listaRespuestasCsvETL = new List<Respuesta<ArchivoTelevisionDigitalCsv>>();
            listaRespuestasCsvIMTFijas = new List<Respuesta<ArchivoIMTFijasCsv>>();
            listaRespuestasCsvBandaA = new List<Respuesta<ArchivoBandaAngostaCsv>>();
            espectroDA = new EspectroAD(appContext);
            refBitacoraBL = new BitacoraBL(AppContext);
        }



        //public void LeerLineaMif(string line, string nombreArchivo)
        //{
        //    archivoMif.nombreArchivoMif = nombreArchivo.Substring(0, (nombreArchivo.Length - 4));

        //    //Se valida si la línea que se está leyendo está vacía
        //    if (!string.IsNullOrEmpty(line))
        //    {
        //        //Se obtiene la primer letra de la línea que se está leyendo
        //        string letraInicial = line.Substring(0, 1);

        //        if (letraInicial.Equals("R"))
        //            archivoMif.region = Int32.Parse(line.Substring(7, line.Length - 7));

        //        if (letraInicial.Equals("-"))
        //        {
        //            var coordenadas = line.Split(' ');
        //            archivoMif.longitudes.Add(coordenadas[0]);
        //            archivoMif.latitudes.Add(coordenadas[1]);
        //        }
        //    }
        //}

        //public void LeerLineaCsv(string line, string nombreArchivo)
        //{

        //    //Se valida si la línea que se está leyendo está vacía o es la primer línea del archivo, que no interesa.
        //    if (!string.IsNullOrEmpty(line) && !line.Substring(1, 1).Equals("T"))
        //    {
        //        string[] annoMesDia = new string[3];
        //        string[] horaMinutoSegundo = new string[3];
        //        string[] fechaHora = new string[3];
        //        string[] separadorNombre = new string[3];

        //        nombreArchivo = nombreArchivo.Substring(0, (nombreArchivo.Length - 4));

        //        separadorNombre = nombreArchivo.Split(new Char[] { '_' });

        //        // Se limpia la línea de caracteres inútiles
        //        line = line.Replace(@"\", string.Empty);
        //        line = line.Replace("\"", string.Empty);

        //        // Se separan los valores
        //        string[] arrayValores = line.Split(new char[] { ',' });

        //        // Se separa la fechar y hora del campo tiempo  (arrayValores[0])
        //        fechaHora = arrayValores[0].Split(new char[] { ' ' });

        //        // Se limpian las cadenas d espacios en blanco
        //        arrayValores[1] = arrayValores[1].Replace(" ", string.Empty);
        //        arrayValores[2] = arrayValores[2].Replace(" ", string.Empty);

        //        // Se separa año, mes, día, hora, minuto y segundo del array  fechaHora  
        //        annoMesDia = fechaHora[0].Split(new char[] { '/' });
        //        horaMinutoSegundo = fechaHora[2].Split(new char[] { ':' });

        //        // Se eliminan los decimales de el valor segundos
        //        horaMinutoSegundo[2] = horaMinutoSegundo[2].Split(new char[] { '.' })[0];




        //        archivoCsv.CodigoSitio = separadorNombre[0];
        //        archivoCsv.FechaCapturaArchivo = new DateTime(Int32.Parse("20" + separadorNombre[1].Substring(0, 2)),
        //                                                      Int32.Parse(separadorNombre[1].Substring(2, 2)),
        //                                                      Int32.Parse(separadorNombre[1].Substring(4, 2)));
        //        archivoCsv.CodigoConsecutivo = separadorNombre[2];


        //        var detalleArchivo = new DetalleArchivoCsv();   

        //        detalleArchivo.Tiempo = new DateTime(Int32.Parse(annoMesDia[2]), //Año
        //                                            Int32.Parse(annoMesDia[1]), // Mes
        //                                            Int32.Parse(annoMesDia[0]), // Día
        //                                            Int32.Parse(horaMinutoSegundo[0]), //Hora
        //                                            Int32.Parse(horaMinutoSegundo[1]), // Minuto
        //                                            Int32.Parse(horaMinutoSegundo[2])); //Segundo

        //        detalleArchivo.Frecuencia = Int32.Parse(arrayValores[1]);

        //        // Se usa para que, a la hora de parsear, reconozca el punto. Por defecto solo reconoce ,
        //        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        //        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        //        detalleArchivo.Nivel = decimal.Parse(arrayValores[2], NumberStyles.Any, ci);

        //        archivoCsv.listaDetalle.Add(detalleArchivo);

        //        if (!String.IsNullOrEmpty(arrayValores[3]))
        //            archivoCsv.informacionArchivo += arrayValores[3];
        //    }


        //}







        /// <summary>
        /// La función valida que el contenido del archivo Mif sea correcto.
        /// </summary>
        /// <param name="line">line se refiere a una línea de caracteres del archivo que se encuentre en lectura. Proviene de EspectroController</param>
        /// <returns>Si la línea que se está leyendo no posee el formato correcto retornará false, de lo contrario, true.</returns>


        public bool LeerValidarContenidoArchivoMif(string line)
        {

            //Se valida si la línea que se está leyendo está vacía
            if (!string.IsNullOrEmpty(line))
            {

                CoordenadasArchivoMif coordenadasArchivoMif = new CoordenadasArchivoMif();

                //Se obtiene la primer letra de la línea que se está leyendo
                string letraInicial = line.Substring(0, 1);

                //Si la letra inicial es R nos interesa, ya que aquí viene el dato región
                if (letraInicial.Equals("R"))
                {

                    archivoMif.region = Int32.Parse(line.Substring(7, line.Length - 7));
                    if (string.IsNullOrEmpty(archivoMif.region.ToString()))
                        return false;

                }

                //Si el caracter inicial es - nos interesa, ya que aquí vienen las coordenadas
                if (letraInicial.Equals("-"))
                {
                    var coordenadas = line.Split(' ');

                    //Si en la línea vienen dos cadenas de caracteres separadas por un espacio la línea está correcta
                    if (coordenadas.Length == 2 && !string.IsNullOrEmpty(coordenadas[0]) && !string.IsNullOrEmpty(coordenadas[1]))
                    {
                        coordenadasArchivoMif.Region = archivoMif.region;
                        coordenadasArchivoMif.Latitud = coordenadas[1];
                        coordenadasArchivoMif.Longitud = coordenadas[0];

                        archivoMif.listaCoordenadas.Add(coordenadasArchivoMif);

                    }

                    else
                        return false;




                }

                return true;
            }

            return true;
        }

        /// <summary>
        /// La función valida que el contenido del archivo Csv sea correcto.
        /// </summary>
        /// <param name="line">line se refiere a una línea de caracteres del archivo que se encuentre en lectura. Proviene de EspectroController</param>
        /// <returns>Si la línea que se está leyendo no posee el formato correcto retornará false, de lo contrario, true.</returns>


        public bool LeerValidarContenidoArchivoCsv(string line)
        {

            //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
            if (!string.IsNullOrEmpty(line) && !line.Substring(1, 1).Equals("T"))
            {
                string[] annoMesDia = new string[3];
                string[] horaMinutoSegundo = new string[3];
                string[] fechaHora = new string[3];


                // Se limpia la línea de caracteres inútiles

                line = line.Replace(@"\", string.Empty);
                line = line.Replace("\"", string.Empty);


                // Se separan los valores en donde haya una coma (,)

                string[] arrayValores = line.Split(new char[] { ',' });

                // Si el split por coma no genera un array de 4 posiciones el formato no es correcto
                if (arrayValores.Length != 4)
                    return false;

                // Se separa la fecha y hora del campo tiempo  (arrayValores[0])

                // Si el split por especio ' ' no genera un array de 3 posiciones el formato no es correcto
                fechaHora = arrayValores[0].Split(new char[] { ' ' });
                if (fechaHora.Length != 3)
                    return false;

                // Se limpian las cadenas d espacios en blanco
                arrayValores[1] = arrayValores[1].Replace(" ", string.Empty);
                arrayValores[2] = arrayValores[2].Replace(" ", string.Empty);

                // Se separa año, mes, día, hora, minuto y segundo del array fechaHora  
                annoMesDia = fechaHora[0].Split(new char[] { '/' });
                if (annoMesDia.Length != 3)
                    return false;
                horaMinutoSegundo = fechaHora[2].Split(new char[] { ':' });

                if (horaMinutoSegundo.Length != 3)
                    return false;

                var detalleArchivo = new DetalleArchivoCsv();

                // var sec = Int32.Parse(horaMinutoSegundo[2]);

                detalleArchivo.Tiempo = new DateTime(Int32.Parse(annoMesDia[2]), //Año
                                                    Int32.Parse(annoMesDia[1]), // Mes
                                                    Int32.Parse(annoMesDia[0]), // Día
                                                    Int32.Parse(horaMinutoSegundo[0]), //Hora
                                                    Int32.Parse(horaMinutoSegundo[1]), // Minuto
                                                    Int32.Parse(horaMinutoSegundo[2].Split('.')[0])); //Segundo

                detalleArchivo.Frecuencia = Int64.Parse(arrayValores[1]);

                // Se usa para que, a la hora de parsear, reconozca el punto. Por defecto solo reconoce ,
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                detalleArchivo.Nivel = decimal.Parse(arrayValores[2], NumberStyles.Any, ci);

                archivoCsv.listaDetalle.Add(detalleArchivo);

                if (!String.IsNullOrEmpty(arrayValores[3]))
                    archivoCsv.informacionArchivo += arrayValores[3];

                return true;
            }

            return true;
        }


        /// <summary>
        /// La función valida que el nombre del archivo Csv sea correcto.
        /// </summary>
        /// <param name="nombre">nombre proviene de EspectroController y es el nombre del ArchivoCsv que está en lectura</param>
        /// <returns>Si el nombre que se está leyendo posee el formato correcto retornará true, de lo contrario, false. Un formato correcto es 001030_150213_0106 o 001030_150213_ETL, ya que posee dos guiones bajos y aparte de esto los otros caracteres son números</returns>


        public bool ValidarNombreArchivoCsv(string nombreArchivo)
        {

            string[] separadorNombre = new string[3];

            //Se elimina la extensión del nombre
            nombreArchivo = nombreArchivo.Substring(0, (nombreArchivo.Length - 4));

            //Se separa el nombre en donde encuentre guiones bajos
            separadorNombre = nombreArchivo.Split(new Char[] { '_' });

            // Se valida que hayan tres datos en el nombre (CódigoSitio, FechaCaptura, CódigoConsecutivo)
            // Se agrega la validación para que permita cuando es igual a ETL
            if (separadorNombre.Length == 3)
                if (System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[0], "^[0-9]*$") &&
                    System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[1], "^[0-9]*$") &&
                    (System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[2], "^[0-9]*$") ||
                    separadorNombre[2].Equals("ETL")))
                    return true;
                else
                    return false;
            else
                 // Se valida que hayan cuatro datos en el nombre (IMT o BA, FechaCaptura, CódigoConsecutivo, Estación)
                 if (separadorNombre.Length == 4)
                if ((separadorNombre[0].Equals("IMT") || separadorNombre[0].Equals("BA")) &&
                    System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[1], "^[0-9]*$") &&
                    System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[2], "^[0-9]*$") &&
                    System.Text.RegularExpressions.Regex.IsMatch(separadorNombre[3], "^[a-zA-Z0-9]*$"))

                    return true;
                else
                    return false;

            else return false;

        }



        //public void LeerArchivosAReemplazar(List<string> listaNombres, string path, string user, string maquina)
        //{
        //    string extension = string.Empty; 
        //    string[] filePaths = Directory.GetFiles(path);
        //    string s = string.Empty;
        //    string nombreArchivo = string.Empty;
        //    string[] separadorRuta = new string[1];
        //    bool mifLeidoCorrectamente = true, csvLeidoCorrectamente = true;

        //    foreach (var nombreArchivoAReemplazar in listaNombres)
        //    {


        //    foreach (var ruta in filePaths)
        //    {

        //        extension = ruta.Substring((ruta.Length - 3), 3);
        //        separadorRuta = ruta.Split(new char[] { '\\' } );
        //        nombreArchivo =  separadorRuta[(separadorRuta.Length-1)];
        //        if (nombreArchivo.Substring(0, nombreArchivo.Length - 4).Equals(nombreArchivoAReemplazar))
        //        using (StreamReader sr = File.OpenText(ruta))
        //        {
        //           if (extension.ToLower().Equals("csv"))
        //           {
        //               while ((s = sr.ReadLine()) != null && csvLeidoCorrectamente)
        //               {
        //                   csvLeidoCorrectamente = LeerLineaCsv(s);
        //               }
        //               agregarNombreCsv(nombreArchivo);


        //           } else {

        //               while ((s = sr.ReadLine()) != null && mifLeidoCorrectamente)
        //               {
        //                  mifLeidoCorrectamente =  LeerLineaMif(s, nombreArchivo);
        //               }


        //           }   

        //        }
        //    }}



        //}




        /// <summary>
        /// La función elimina los archivos que estén contenidos en la carpeta especificada.
        /// </summary>
        /// <param name="path">path proviene de EspectroController y es la ruta de la carpeta cual se quiera dejar vacía.</param>       

        public void eliminarArchivos(string path)
        {
            string[] filePaths = Directory.GetFiles(path);

            foreach (var ruta in filePaths)
                File.Delete(ruta);
        }



        /// <summary>
        /// El método devuelve true si el archivo ya se encuentra en BD
        /// </summary>
        /// <param name="nombreArchivo">nombreArchivo proviene de EspectroController y es el nombre del archivo que se encuentra en lectura
        /// ..</param>    

        public Respuesta<bool> archivoMifYaCargado(string nombreArchivo)
        {
            return espectroDA.ArchivoMifYaCargado(nombreArchivo);
        }

        public Respuesta<bool> VerificaArchivoCsv(string nombreArchivo, string rutaCarpetaInsertar, string rutaCarpetaReemplazar)
        {
            return espectroDA.VerificaArchivoCsv(nombreArchivo, rutaCarpetaInsertar, rutaCarpetaReemplazar);
        }

        /// <summary>
        /// Verifica si el archivo ya existe
        /// </summary>
        /// <param name="nombreArchivo">nombre del archivo</param>
        /// <param name="archivoETL">True si el archivo es ETL, de lo contrario false</param>
        /// <param name="archivoIMTFija">True si el archivo es IMT, de lo contrario false</param> 
        /// <param name="archivoBandaAn">True si el archivo es Banda Angosta, de lo contrario false</param>
        /// <returns>False si no existe, true de lo contrario</returns>
        public Respuesta<bool> archivoCsvYaCargado(string nombreArchivo, bool archivoETL = false, bool archivoIMTFija = false, bool archivoBandaAn = false)
        {
            return espectroDA.ArchivoCsvYaCargado(nombreArchivo, archivoETL, archivoIMTFija, archivoBandaAn);
        }
        //public Respuesta<bool> validarNullArchivosCSV()
        //{
        //    Respuesta<bool> respuesta = new Respuesta<bool>();
        //}

        public void MoverArchivos(string ruta, string destinationFile, List<string> ListaSeleccionados)
        {

            string[] filePaths = Directory.GetFiles(ruta);

            foreach (var nombre in ListaSeleccionados)
            {
                var nombreSeleccionado = nombre.Substring(0, nombre.Length - 4);
                foreach (var sourceFile in filePaths)
                {

                    var nombreEnCarpeta = sourceFile.Split(new char[] { '\\' })[sourceFile.Split(new char[] { '\\' }).Length - 1];

                    var destino = destinationFile + "\\" + nombreEnCarpeta;


                    //Se elimina La extension
                    nombreEnCarpeta = nombreEnCarpeta.Substring(0, nombreEnCarpeta.Length - 4);
                    if (nombreSeleccionado.Equals(nombreEnCarpeta))
                        System.IO.File.Move(sourceFile, destino);

                    destino = string.Empty;
                }
            }




        }

        public Respuesta<bool> EliminarArchivosMif(string nombre)
        {

            return espectroDA.EliminarMif(nombre);

        }

        public Respuesta<bool> EliminarArchivosCsv(string nombre, bool Etl = false, bool archivoIMTFija = false, bool archivoBandaAn = false, string rutaCarpetaInserta = "", string rutaCarpetaReemplazar = "")
        {
            if (Etl)
            {
                return espectroDA.EliminarCsvETL(nombre);
            }
            else if (archivoIMTFija)
            {
                return espectroDA.EliminarCsvIMT(nombre, rutaCarpetaInserta, rutaCarpetaReemplazar);
            }

            else if (archivoBandaAn)
            {
                    return espectroDA.EliminarCsvBandaAn(nombre, rutaCarpetaInserta, rutaCarpetaReemplazar);
            }
            else
             {
                    return espectroDA.EliminarCsv(nombre);    
            }
        }
        


        //public Respuesta<bool> GuardarArchivosEnBD(string usuario, string maquina, string detalleTransaccion, int AccionExitosa, int Accion)
        //{

        //    Respuesta<bool> respuesta = espectroDA.GuardarArchivosEnBD(usuario, maquina, detalleTransaccion, AccionExitosa, Accion);

        //    return respuesta;


        //}


        /// <summary>
        /// Inserta las listas de archivos en BD, se suma la cantidad de registros para insertar en Bitacora
        /// </summary>
        /// <param name="machine">máquina</param>
        /// <param name="user">Usuario</param>
        public void InsertarArchivos(string machine, string user)
        {
            int cantidad = 0;

            if (listaArchivosCsv.Count() > 0 || listaArchivosMif.Count() > 0 || listaArchivoTvDigital.Count() > 0 
                || listaArchivoIMTFijas.Count() > 0 || listaArchivoBandaA.Count() > 0)
            {
                cantidad = listaArchivosCsv.Count() + listaArchivosMif.Count() + listaArchivoTvDigital.Count() +
                    listaArchivoIMTFijas.Count() + listaArchivoBandaA.Count();

                foreach (var archivo in listaArchivosCsv)
                {
                    listaRespuestasCsv.Add(
                     espectroDA.gAgregarArchivoCsv(archivo, user, machine, false));
                }

                foreach (var archivo in listaArchivosMif)
                {
                    listaRespuestasMif.Add(
                        espectroDA.gAgregarArchivoMif(archivo, user, machine, false));
                }

                foreach (var archivo in listaArchivoTvDigital)
                {
                    listaRespuestasCsvETL.Add(
                     espectroDA.gAgregarArchivoCsvETL(archivo, user, machine, false));
                }

                foreach (var archivo in listaArchivoIMTFijas)
                {
                    listaRespuestasCsvIMTFijas.Add(
                     espectroDA.gAgregarArchivoCsvIMTFija(archivo, user, machine, false));
                }

                foreach (var archivo in listaArchivoBandaA)
                {
                    listaRespuestasCsvBandaA.Add(
                     espectroDA.gAgregarArchivoCsvBandaAngosta(archivo, user, machine, false));
                }

                refBitacoraBL.InsertarBitacora(2, "Espectro", user, "Se cargo un archivo en espectro", "Cantidad de registros: " + cantidad, "");

            }

        }

        public void AgregarMifALista(string nombreArchivo)
        {

            //Se quita la extensión
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            archivoMif.nombreArchivoMif = nombreArchivo;
            ArchivoMifModel nuevoArchivo = archivoMif;
            listaArchivosMif.Add(nuevoArchivo);
            archivoMif = new ArchivoMifModel();
        }

        /// <summary>
        /// Valida que los titulos sean los correctos para documentos CSV 
        /// </summary>
        /// <param name="titulos">máquina</param>
        public bool validaTitulosColumnasArchivoBA_IMT(string titulos)
        {


            titulos = titulos.Replace(@"\", string.Empty);
            titulos = titulos.Replace("\"", string.Empty);
            //arrayValoresArchivo = titulos.Split(new char[] { ',' }, 8);
            string[] Arraytitulos = titulos.Split(',');

            if (Arraytitulos[0] == "Tiempo" && Arraytitulos[1] == "Frecuencia (Hz)" && Arraytitulos[2] == "Level (dB�V/m)" && Arraytitulos[3] == "Arch_ Info")
            {
                return true;
            }
            else {
                return false;
            }


        }

            /// <summary>
            /// Agrega los archivos que estan correctos a la lista
            /// </summary>
            /// <param name="nombreArchivo">nombre del archivo</param>
            /// <param name="archivoETL">true, si es un archivo ETL, false de lo contrario</param>
            /// <param name="archivoIMTFija">true, si es un archivo IMT, false de lo contrario</param>
            /// <param name="archivoBandaAn">true, si es un archivo Banda Angosta, false de lo contrario</param> 
            public void AgregarCsvALista(string nombreArchivo, bool archivoETL = false, bool archivoIMTFija = false, bool archivoBandaAn = false)
        {

            //Se quita la extensión
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            string[] separadorNombre = new string[3];

            separadorNombre = nombreArchivo.Split('_');

            //Si es ETL 
            if (archivoETL)
            {
                archivoTvDigital.CodigoSitio = separadorNombre[0];
                archivoTvDigital.FechaCapturaArchivo = new DateTime(Int32.Parse("20" + separadorNombre[1].Substring(0, 2)),
                                                              Int32.Parse(separadorNombre[1].Substring(2, 2)),
                                                              Int32.Parse(separadorNombre[1].Substring(4, 2)));
                archivoTvDigital.CodigoConsecutivo = separadorNombre[2];

                archivoTvDigital.nombreArchivo = nombreArchivo;


                listaArchivoTvDigital.Add(archivoTvDigital);
                archivoTvDigital = new ArchivoTelevisionDigitalCsv();
            }
            else
            {
                if (archivoIMTFija)
                {
                    archivoIMTFijas.Estacion = separadorNombre[3];
                    archivoIMTFijas.FechaCapturaArchivo = new DateTime(Int32.Parse("20" + separadorNombre[1].Substring(0, 2)),
                                                                  Int32.Parse(separadorNombre[1].Substring(2, 2)),
                                                                  Int32.Parse(separadorNombre[1].Substring(4, 2)));
                    archivoIMTFijas.CodigoConsecutivo = separadorNombre[2];

                    archivoIMTFijas.nombreArchivo = nombreArchivo;


                    listaArchivoIMTFijas.Add(archivoIMTFijas);
                    archivoIMTFijas = new ArchivoIMTFijasCsv();
                }
                else
                {
                    if (archivoBandaAn)
                    {
                        archivoBandaA.Estacion = separadorNombre[3];
                        archivoBandaA.FechaCapturaArchivo = new DateTime(Int32.Parse("20" + separadorNombre[1].Substring(0, 2)),
                                                                      Int32.Parse(separadorNombre[1].Substring(2, 2)),
                                                                      Int32.Parse(separadorNombre[1].Substring(4, 2)));
                        archivoBandaA.CodigoConsecutivo = separadorNombre[2];

                        archivoBandaA.nombreArchivo = nombreArchivo;


                        listaArchivoBandaA.Add(archivoBandaA);
                        archivoBandaA = new ArchivoBandaAngostaCsv();
                    }
                    else
                    {
                        archivoCsv.CodigoSitio = separadorNombre[0];
                        archivoCsv.FechaCapturaArchivo = new DateTime(Int32.Parse("20" + separadorNombre[1].Substring(0, 2)),
                                                                      Int32.Parse(separadorNombre[1].Substring(2, 2)),
                                                                      Int32.Parse(separadorNombre[1].Substring(4, 2)));
                        archivoCsv.CodigoConsecutivo = separadorNombre[2];

                        archivoCsv.nombreArchivo = nombreArchivo;


                        ArchivoCsvModel nuevoArchivo = archivoCsv;
                        listaArchivosCsv.Add(nuevoArchivo);
                        archivoCsv = new ArchivoCsvModel();
                    }
                }

            }
        }

        public void LeerArchivosAReemplazar(List<string> listaNombres, string path, string user, string machine)
        {
            string extension = string.Empty;
            string[] filePaths = Directory.GetFiles(path);
            string s = string.Empty;
            string nombreArchivo = string.Empty;
            string[] separadorRuta = new string[1];

            foreach (var nombreArchivoAReemplazar in listaNombres)
            {
                foreach (var ruta in filePaths)
                {
                    extension = ruta.Substring((ruta.Length - 3), 3);
                    separadorRuta = ruta.Split(new char[] { '\\' });
                    nombreArchivo = separadorRuta[(separadorRuta.Length - 1)];
                    var nombreSinExtension = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

                    if (nombreArchivo.Equals(nombreArchivoAReemplazar))
                        using (StreamReader sr = File.OpenText(ruta))
                        {
                            if (extension.ToLower().Equals("csv"))
                            {
                                //Verifica si el archivo es ETL
                                if (nombreSinExtension.Substring(nombreSinExtension.Length - 3, 3).Equals("ETL"))
                                {
                                    while ((s = sr.ReadLine()) != null)
                                    {
                                        //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                        if (!string.IsNullOrEmpty(s) && !s.Substring(0, 2).Contains("T"))
                                        {
                                            LeerValidarColumnas(s);
                                            LeerValidarContenidoArchivoCsvETL();
                                        }
                                    }

                                    AgregarCsvALista(nombreArchivo, true);
                                }
                                else
                                {
                                    if (nombreSinExtension.Substring(0, 3).Equals("IMT"))
                                    {
                                        while ((s = sr.ReadLine()) != null)
                                        {
                                            //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                            if (!string.IsNullOrEmpty(s) && !s.Substring(0, 2).Contains("T"))
                                            {
                                                LeerValidarColumnas(s, true);
                                                LeerValidarArchivoCsvIMTBandaA();
                                            }
                                        }

                                        AgregarCsvALista(nombreArchivo, false, true);
                                    }
                                    else
                                    {
                                        if (nombreSinExtension.Substring(0, 2).Equals("BA"))
                                        {
                                            while ((s = sr.ReadLine()) != null)
                                            {
                                                //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                                if (!string.IsNullOrEmpty(s) && !s.Substring(0, 2).Contains("T"))
                                                {
                                                    LeerValidarColumnas(s, false, true);
                                                    LeerValidarArchivoCsvIMTBandaA(false);
                                                }
                                            }

                                            AgregarCsvALista(nombreArchivo, false, false, true);
                                        }
                                        else
                                        {
                                            while ((s = sr.ReadLine()) != null)
                                            {
                                                LeerValidarContenidoArchivoCsv(s);
                                            }

                                            AgregarCsvALista(nombreArchivo);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                while ((s = sr.ReadLine()) != null)
                                {
                                    LeerValidarContenidoArchivoMif(s);
                                }
                                AgregarMifALista(nombreArchivo);

                            }

                        }
                }
            }

            ReemplazarArchivos(user, machine);

        }




        public void ReemplazarArchivos(string user, string machine)
        {

            foreach (var archivo in listaArchivosCsv)
            {

                listaRespuestasCsv.Add(espectroDA.gAgregarArchivoCsv(archivo, user, machine, true));
            }

            foreach (var archivo in listaArchivosMif)
            {
                listaRespuestasMif.Add(espectroDA.gAgregarArchivoMif(archivo, user, machine, true));
            }

            foreach (var archivo in listaArchivoTvDigital)
            {
                listaRespuestasCsvETL.Add(
                 espectroDA.gAgregarArchivoCsvETL(archivo, user, machine, true));
            }

            foreach (var archivo in listaArchivoIMTFijas)
            {
                listaRespuestasCsvIMTFijas.Add(
                 espectroDA.gAgregarArchivoCsvIMTFija(archivo, user, machine, true));
            }

            foreach (var archivo in listaArchivoBandaA)
            {
                listaRespuestasCsvBandaA.Add(
                 espectroDA.gAgregarArchivoCsvBandaAngosta(archivo, user, machine, true));
            }

        }


        public void eliminarArchivosYaReemplazados(string path, List<string> listaAEliminar)
        {
            string nombreArchivo = string.Empty;
            string[] separadorRuta = new string[1];

            string[] filePaths = Directory.GetFiles(path);

            foreach (var ruta in filePaths)
            {

                separadorRuta = ruta.Split(new char[] { '\\' });
                nombreArchivo = separadorRuta[(separadorRuta.Length - 1)];
                foreach (var nombreSeleccionado in listaAEliminar)
                {
                    if (nombreSeleccionado.Equals(nombreArchivo.Substring(0, nombreArchivo.Length - 4)))
                    {

                        File.Delete(ruta);

                    }
                }

            }

            //filesNamesNoSelectedToReplace = Directory.GetFiles(path);
        }
        /// <summary>
        /// Valida que cada uno de los datos del titulo tengan el formato que les corresponde
        /// </summary>
        /// <param name="titulo">Se refiere al titulo del archivo. Proviene de EspectroController</param>
        /// <returns>booleano true cuando el resultado es correcto, de lo contrario false</returns>
        public bool validarTituloArchivoIMT_BA(string titulo)
        {
            titulo = titulo.Replace(".csv", "");
            string[] arrayTitulo = titulo.Split('_');

            DateTime validadorFecha;
            int validadorNumero;

            //se resta los campos de mes y año para que poesterioremente se valide si el restante de datos es equiparable a la cantidad de campos de un dia (2)
            var cantidadRestanteAnno = arrayTitulo[1].Length-4;

            var anno="20"+ arrayTitulo[1].Substring(0, 2);
            var mes = arrayTitulo[1].Substring(2, 2);
            var dia = arrayTitulo[1].Substring(4, cantidadRestanteAnno);

            var fecha= dia+"/"+mes+"/"+ anno;
            string formatString = "dd/MM/yyyy";
            CultureInfo enUS = new CultureInfo("en-US");

            if (DateTime.TryParseExact(fecha, formatString, enUS,
                                           DateTimeStyles.None, out validadorFecha))
            {
                if (arrayTitulo[0] == "IMT" || arrayTitulo[0] == "BA") {
                    if (int.TryParse(arrayTitulo[2], out validadorNumero) && arrayTitulo[2].Length<=5)
                    {
                        if (arrayTitulo[3].Length <= 10) {
                            return true;

                        }
                        
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Valida dato por dato que tengan tanto el tipo como el formato correcto
        /// </summary>
        /// <param name="fila">se refiere a una línea de caracteres del archivo que se encuentre en lectura. Proviene de EspectroController</param>
        /// <returns>booleano true cuando el resultado es correcto, de lo contrario false</returns>
        public bool validarDatosArchivoIMT_BA(string fila)
        {
            fila = fila.Replace(@"\", string.Empty);
            fila = fila.Replace("\"", string.Empty);
            string[] arrayFila = fila.Split(',');

            DateTime validadorFecha;
            Int64 validadorNumero;
            float validadorDecimal;
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            if (DateTime.TryParseExact(arrayFila[0], "dd/MM/yyyy  HH:mm:ss.FFF", System.Globalization.CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out validadorFecha))
            {
                if (Int64.TryParse(arrayFila[1], out validadorNumero)) {
                    if (float.TryParse(arrayFila[2], out validadorDecimal))
                    {
                        return true;
                    }
                }
            }
             return false;
        
        }

            /// <summary>
            /// Valida que las columnas que vienen en el archivo sean las correspondientes a cada archivo
            /// </summary>
            /// <param name="line">line se refiere a una línea de caracteres del archivo que se encuentre en lectura. Proviene de EspectroController</param>
            /// <param name="archivoIMTFija">Si es un archivo IMT Estaciones fijas va en true, de lo contrario en false</param>
            /// <param name="archivoBandaAn">Si es un archivo Banda Angosta va en true, de lo contrario en false</param>
            /// <returns>booleano true cuando el resultado es correcto, de lo contrario false</returns>
            public bool LeerValidarColumnas(string line, bool archivoIMTFija = false, bool archivoBandaAn = false)
        {
            // Se limpia la línea de caracteres inútiles
            line = line.Replace(@"\", string.Empty);
            line = line.Replace("\"", string.Empty);

            // Se separan los valores en donde haya una coma (,)
            arrayValoresArchivo = line.Split(new char[] { ',' }, 8);

            if (archivoIMTFija || archivoBandaAn)
            {
                // Si el split por coma no genera un array de 4 posiciones el formato no es correcto
                //********CAMBIAR PARA QUE QUEDE DE 4 COLUMNAS***********
                if (arrayValoresArchivo.Length < 4)
                {
                    return false;
                }
            }
            else
            {
                // Si el split por coma no genera un array de 6 posiciones el formato no es correcto
                if (arrayValoresArchivo.Length < 6)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Valida que el contenido del archivo, sea el correcto
        /// </summary>
        /// <returns>booleano true cuando el resultado es correcto, de lo contrario false</returns>
        public bool LeerValidarContenidoArchivoCsvETL()
        {
            string[] annoMesDia = new string[3];
            string[] horaMinutoSegundo = new string[3];
            string[] fechaHora = new string[3];
            int frecuenciaResult;
            decimal ISDBTResult;
            decimal acimutResult;

            // Se separa la fecha y hora del campo tiempo  (arrayValores[0])
            // Si el split por especio ' ' no genera un array de 3 posiciones el formato no es correcto
            fechaHora = arrayValoresArchivo[0].Split(new char[] { ' ' });
            if (fechaHora.Length != 3)
                return false;

            // Se limpian las cadenas d espacios en blanco
            arrayValoresArchivo[1] = arrayValoresArchivo[1].Replace(" ", string.Empty);
            arrayValoresArchivo[2] = arrayValoresArchivo[2].Replace(" ", string.Empty);
            arrayValoresArchivo[3] = arrayValoresArchivo[3].Replace(" ", string.Empty);
            arrayValoresArchivo[4] = arrayValoresArchivo[4].Replace(" ", string.Empty);
            arrayValoresArchivo[5] = arrayValoresArchivo[5].Replace(" ", string.Empty);
            //arrayValoresArchivo[6] = arrayValoresArchivo[6].Replace(" ", string.Empty);


            // Se separa año, mes, día, hora, minuto y segundo del array fechaHora  
            annoMesDia = fechaHora[0].Split(new char[] { '/' });
            if (annoMesDia.Length != 3)
                return false;

            horaMinutoSegundo = fechaHora[2].Split(new char[] { ':' });

            if (horaMinutoSegundo.Length != 3)
                return false;


            var detalleArchivoTvDigital = new DetalleArchivoTelevisionDigitalCsv();

            detalleArchivoTvDigital.Tiempo = new DateTime(Int32.Parse(annoMesDia[2]), //Año
                                                Int32.Parse(annoMesDia[1]), // Mes
                                                Int32.Parse(annoMesDia[0]), // Día
                                                Int32.Parse(horaMinutoSegundo[0]), //Hora
                                                Int32.Parse(horaMinutoSegundo[1]), // Minuto
                                                Int32.Parse(horaMinutoSegundo[2].Split('.')[0])); //Segundo


            bool frecuenciaValidar = int.TryParse(arrayValoresArchivo[1], out frecuenciaResult);
            if (frecuenciaValidar)
            {
                detalleArchivoTvDigital.Frecuencia = frecuenciaResult;
            }
            else
            {
                return false;
            }

            detalleArchivoTvDigital.NombreSistema = arrayValoresArchivo[2].Length > 100 ? arrayValoresArchivo[2].Substring(0, 100) : arrayValoresArchivo[2];

            // Se usa para que, a la hora de parsear, reconozca el punto. Por defecto solo reconoce ,
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            bool ISDBTValidar = decimal.TryParse(arrayValoresArchivo[3], out ISDBTResult);
            if (ISDBTValidar)
            {
                detalleArchivoTvDigital.ISDBT = decimal.Parse(arrayValoresArchivo[3], NumberStyles.Any, ci);
            }
            else
            {
                return false;
            }

            bool acimutValidar = decimal.TryParse(arrayValoresArchivo[4], out acimutResult);
            if (acimutValidar)
            {
                detalleArchivoTvDigital.Acimut = int.Parse(arrayValoresArchivo[4], NumberStyles.Any, ci);
            }
            else
            {
                return false;
            }

            // detalleArchivoTvDigital.Logitud = arrayValoresArchivo[5].Length > 50 ? arrayValoresArchivo[5].Substring(0, 50) : arrayValoresArchivo[5];

            // detalleArchivoTvDigital.Latitude = arrayValoresArchivo[6].Length > 50 ? arrayValoresArchivo[6].Substring(0, 50) : arrayValoresArchivo[6];

            detalleArchivoTvDigital.ArchInfo = arrayValoresArchivo[5].Length > 150 ? arrayValoresArchivo[5].Substring(0, 150) : arrayValoresArchivo[5];

            archivoTvDigital.listaDetalle.Add(detalleArchivoTvDigital);

            return true;
        }


            /// <summary>
            /// Valida que el contenido del archivo, sea el correcto
            /// </summary>
            /// <param name="archivoIMTFija">Si es archivo IMT de lo contario es Banda Angosta</param>
            /// <returns>booleano true cuando el resultado es correcto, de lo contrario false</returns>
            public bool LeerValidarArchivoCsvIMTBandaA(bool archivoIMTFija = true)
        {
            string[] annoMesDia = new string[3];
            string[] horaMinutoSegundo = new string[3];
            string[] fechaHora = new string[3];
            Int64 frecuenciaResult;
            decimal levelResult;

            // Se usa para que, a la hora de parsear, reconozca el punto. Por defecto solo reconoce ,
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            // Se separa la fecha y hora del campo tiempo  (arrayValores[0])
            // Si el split por espacio ' ' no genera un array de 3 posiciones el formato no es correcto
            fechaHora = arrayValoresArchivo[0].Split(new char[] { ' ' });
            if (fechaHora.Length != 3)
                return false;

            // Se limpian las cadenas de espacios en blanco
            arrayValoresArchivo[1] = arrayValoresArchivo[1].Replace(" ", string.Empty);
            arrayValoresArchivo[2] = arrayValoresArchivo[2].Replace(" ", string.Empty);

            //*******CAMBIAR CUANDO ESTE EL ARCHIVO CORRECTO DEBE TERMINAR EN 4 QUE ES ArchivoInfo*******
            arrayValoresArchivo[3] = arrayValoresArchivo[3].Replace(" ", string.Empty);
            //arrayValoresArchivo[4] = arrayValoresArchivo[4].Replace(" ", string.Empty);
            //arrayValoresArchivo[5] = arrayValoresArchivo[5].Replace(" ", string.Empty);


            // Se separa año, mes, día, hora, minuto y segundo del array fechaHora  
            annoMesDia = fechaHora[0].Split(new char[] { '/' });
            if (annoMesDia.Length != 3)
                return false;

            horaMinutoSegundo = fechaHora[2].Split(new char[] { ':' });

            if (horaMinutoSegundo.Length != 3)
                return false;


            if (archivoIMTFija)
            {
                var detalleArchivoIMTFijas = new DetalleArchivoIMTFijasCsv();

                int[] arrayFecha = new int[3];
                for (int i = 0; i < annoMesDia.Length; i++)
                {
                    if (!int.TryParse(annoMesDia[i], out arrayFecha[i]))
                    {
                        return false;
                    }
                }

                int[] arraytiempo = new int[3];
                horaMinutoSegundo[2] = horaMinutoSegundo[2].Split('.')[0];
                for (int i = 0; i < horaMinutoSegundo.Length; i++)
                {
                    if (!int.TryParse(horaMinutoSegundo[i], out arraytiempo[i]))
                    {
                        return false;
                    }
                }


                detalleArchivoIMTFijas.Tiempo = new DateTime(arrayFecha[2], //Año
                                                    arrayFecha[1], // Mes
                                                    arrayFecha[0], // Día
                                                    arraytiempo[0], //Hora
                                                    arraytiempo[1], // Minuto
                                                    arraytiempo[2]); //Segundo


                bool frecuenciaValidar = Int64.TryParse(arrayValoresArchivo[1], out frecuenciaResult);
                if (frecuenciaValidar)
                {
                    detalleArchivoIMTFijas.Frecuencia = frecuenciaResult;
                }
                else
                {
                    return false;
                }

                bool levelValidar = decimal.TryParse(arrayValoresArchivo[2], out levelResult);
                if (levelValidar)
                {
                    detalleArchivoIMTFijas.Level = decimal.Parse(arrayValoresArchivo[2], NumberStyles.Any, ci);
                }
                else
                {
                    return false;
                }

                //*****CAMBIAR LUEGO A 3 QUE SERIA ArchivoInfo****
                detalleArchivoIMTFijas.ArchivoInfo = arrayValoresArchivo[3].Length > 100 ? arrayValoresArchivo[3].Substring(0, 100) : arrayValoresArchivo[3];

                archivoIMTFijas.listaDetalle.Add(detalleArchivoIMTFijas);
            }
            else
            {
                var detalleArchivoBandaA = new DetalleBandaAngostaCsv();

                int[] arrayFecha = new int[3];
                for (int i = 0; i < annoMesDia.Length; i++)
                {
                    if (!int.TryParse(annoMesDia[i], out arrayFecha[i]))
                    {
                        return false;
                    }
                }

                int[] arraytiempo = new int[3];
                horaMinutoSegundo[2] = horaMinutoSegundo[2].Split('.')[0];
                for (int i = 0; i < horaMinutoSegundo.Length; i++)
                {
                    if (!int.TryParse(horaMinutoSegundo[i], out arraytiempo[i]))
                    {
                        return false;
                    }
                }


                detalleArchivoBandaA.Tiempo = new DateTime(arrayFecha[2], //Año
                                                    arrayFecha[1], // Mes
                                                    arrayFecha[0], // Día
                                                    arraytiempo[0], //Hora
                                                    arraytiempo[1], // Minuto
                                                    arraytiempo[2]); //Segundo


                bool frecuenciaValidar = Int64.TryParse(arrayValoresArchivo[1], out frecuenciaResult);
                if (frecuenciaValidar)
                {
                    detalleArchivoBandaA.Frecuencia = frecuenciaResult;
                }
                else
                {
                    return false;
                }

                bool levelValidar = decimal.TryParse(arrayValoresArchivo[2], out levelResult);
                if (levelValidar)
                {
                    detalleArchivoBandaA.Level = decimal.Parse(arrayValoresArchivo[2], NumberStyles.Any, ci);
                }
                else
                {
                    return false;
                }

                //*****CAMBIAR LUEGO A 3 QUE SERIA ArchivoInfo****
                detalleArchivoBandaA.ArchivoInfo = arrayValoresArchivo[3].Length > 100 ? arrayValoresArchivo[3].Substring(0, 100) : arrayValoresArchivo[3];

                archivoBandaA.listaDetalle.Add(detalleArchivoBandaA);
            }

            return true;
        }
    }



}

