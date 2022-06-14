
using GB.SUTEL.Entities.Espectro;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using GB.SUTEL.Shared;
using System.Collections.Generic;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;
using GB.SUTEL.Entities;

namespace GB.SUTEL.DAL.Proceso
{
    public class EspectroAD : LocalContextualizer
    {

        #region atributos

        private
        Respuesta<ArchivoCsvModel> objRespuestaCsv;
        Respuesta<ArchivoMifModel> objRespuestaMif;
        #endregion

        #region metodos
        public EspectroAD(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuestaMif = new Respuesta<ArchivoMifModel>();
            objRespuestaCsv = new Respuesta<ArchivoCsvModel>();
        }

        /// <summary>
        /// Agrega un archivo Csv a la base de datos
        /// </summary>
        /// <param name="archivoCsv"></param>
        /// <returns></returns>


        #endregion


            public Respuesta<bool> ArchivoMifYaCargado(string nombreArchivo)
        {

            //Se elimina la extension
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {

                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    if (context.ArchivoMif.Where(t => t.NombreArchivoMif == nombreArchivo).FirstOrDefault() != null)

                        respuesta.objObjeto = true;
                    else
                        respuesta.objObjeto = false;
                    return respuesta;
                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }


        }
        public Respuesta<bool> VerificaArchivoCsv(string nombreArchivo, string rutaCarpetaInsertar, string rutaCarpetaReemplazar)
        {

            //Se elimina la extension
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            rutaCarpetaInsertar += "\\"+nombreArchivo + ".csv";
            rutaCarpetaReemplazar += "\\" + nombreArchivo+ ".csv";

            Respuesta<bool> respuesta = new Respuesta<bool>();


            try
            {

                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {
                    var arrayNombreArchivo = nombreArchivo.Split('_');

                    string codigoConsecutivo = arrayNombreArchivo[2];
                    string codigoSitio = arrayNombreArchivo[0];
                    string estacion = arrayNombreArchivo[3];


                    var fecha = new DateTime(Int32.Parse("20" + arrayNombreArchivo[1].Substring(0, 2)),
                         Int32.Parse(arrayNombreArchivo[1].Substring(2, 2)),
                         Int32.Parse(arrayNombreArchivo[1].Substring(4, 2)));

                    if (context.ArchivoCsv.Where(t => t.CodigoConsecutivo == codigoConsecutivo && t.CodigoSitio ==codigoSitio && t.FechaCapturaArchivo== fecha).FirstOrDefault() != null)
                    { 

                        if (System.IO.File.Exists(rutaCarpetaInsertar) || System.IO.File.Exists(rutaCarpetaReemplazar))
                        {
                            respuesta.objObjeto = true;
                        }
                        else {
                            respuesta.objObjeto = false;
                        }
                    }
                    else if (arrayNombreArchivo[0] == "IMT")
                    {
                        if (context.ArchivoIMTFijasCsv.Where(t => t.CodigoConsecutivo == codigoConsecutivo && t.FechaCapturaArchivo == fecha && t.Estacion == estacion).FirstOrDefault() != null)
                        {

                            if (System.IO.File.Exists(rutaCarpetaInsertar) || System.IO.File.Exists(rutaCarpetaReemplazar))
                            {
                                respuesta.objObjeto = true;
                            }
                            else
                            {
                                respuesta.objObjeto = false;
                            }
                        }

                    }else if (arrayNombreArchivo[0] == "BA")
                    {
                        if (context.ArchivoBandaAngostaCsv.Where(t => t.CodigoConsecutivo == codigoConsecutivo && t.FechaCapturaArchivo == fecha && t.Estacion==estacion).FirstOrDefault() != null)
                        {

                            if (System.IO.File.Exists(rutaCarpetaInsertar) || System.IO.File.Exists(rutaCarpetaReemplazar))
                            {
                                respuesta.objObjeto = true;
                            }
                            else
                            {
                                respuesta.objObjeto = false;
                            }
                        }

                    }
                    return respuesta;
                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }


        }

        /// <summary>
        /// Verifica si el archivo ya existe
        /// </summary>
        /// <param name="nombreArchivo">nombre del archivo</param>
        /// <param name="archivoETL">True si el archivo es ETL, de lo contrario false</param>
        /// <param name="archivoIMTFija">True si el archivo es IMT, de lo contrario false</param>
        /// <param name="archivoBandaAn">True si el archivo es Banda Angosta, de lo contrario false</param>
        /// <returns>False si no existe, true de lo contrario</returns>
        public Respuesta<bool> ArchivoCsvYaCargado(string nombreArchivo, bool archivoETL = false, bool archivoIMTFija = false, bool archivoBandaAn = false)
        {
            //Se elimina la extension
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            //Se extraen los identificadores del nombre
            string[] divisorNombre = nombreArchivo.Split('_');
            var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(2, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(4, 2)));

            var CodSitio = "";
            var Estacion = "";
            var CodConsecutivo = divisorNombre[2];

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {
                    //Si el archivo es ETL
                    if (archivoETL)
                    {
                        CodSitio = divisorNombre[0];

                        if (context.ArchivoTelevisionDigitalCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.CodigoSitio == CodSitio &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault() != null)
                            respuesta.objObjeto = true;
                        else
                            respuesta.objObjeto = false;
                    }
                    else
                    {
                        if (archivoIMTFija)
                        {
                            Estacion = divisorNombre[3];

                            if (context.ArchivoIMTFijasCsv.Where(t =>
                                                     t.CodigoConsecutivo == CodConsecutivo &&
                                                     t.Estacion == Estacion &&
                                                     t.FechaCapturaArchivo == fecha).FirstOrDefault() != null)
                                respuesta.objObjeto = true;
                            else
                                respuesta.objObjeto = false;
                        }
                        else
                        {
                            if (archivoBandaAn)
                            {
                                Estacion = divisorNombre[3];

                                if (context.ArchivoBandaAngostaCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.Estacion == Estacion &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault() != null)
                                    respuesta.objObjeto = true;
                                else
                                    respuesta.objObjeto = false;
                            }
                            else
                            {
                                CodSitio = divisorNombre[0];

                                if (context.ArchivoCsv.Where(t =>
                                                                 t.CodigoConsecutivo == CodConsecutivo &&
                                                                 t.CodigoSitio == CodSitio &&
                                                                 t.FechaCapturaArchivo == fecha).FirstOrDefault() != null)
                                    respuesta.objObjeto = true;
                                else
                                    respuesta.objObjeto = false;
                            }
                        }
                    }

                    return respuesta;
                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }

        //#region

        //public Respuesta<ArchivoIMTFijasCsv> gArchivoIMTFijasCsv(ArchivoIMTFijasCsv archivoIMTFijas, string user, string machine, bool esReemplazo)
        //{

        //    int IdUltimoArchivoInsertado = new int();
        //    Respuesta<ArchivoTelevisionDigitalCsv> respuesta = new Respuesta<ArchivoTelevisionDigitalCsv>();
        //    ArchivoTelevisionDigitalCsv csvInsertar = new ArchivoTelevisionDigitalCsv();
        //    GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

        //    try
        //    {
        //        using (var context = new SUTEL_IndicadoresEntities())
        //        {

        //            using (var trans = new TransactionScope())
        //            {
        //                csvInsertar.CodigoConsecutivo = archivoIMTFijas.CodigoConsecutivo;
        //                csvInsertar.CodigoSitio = archivoIMTFijas.CodigoSitio;
        //                csvInsertar.FechaCapturaArchivo = archivoIMTFijas.FechaCapturaArchivo;

        //                context.ArchivoTelevisionDigitalCsv.Add(csvInsertar);
        //                context.SaveChanges();

        //                var ultimoArchivoInsertado = context.ArchivoTelevisionDigitalCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();

        //                IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;


        //                if (esReemplazo)
        //                    bitacoraMifCsv.Accion = 3; // Reemplazar  
        //                else
        //                    bitacoraMifCsv.Accion = 2;


        //                for (int i = 0; i < archivoIMTFijas.DetalleArchivoIMTFijasCsv.Count; i++)
        //                {
        //                    archivoIMTFijas.DetalleArchivoIMTFijasCsv[i].IdArchivoCsv = IdUltimoArchivoInsertado;
        //                }

        //                //Inserta todos los registros en base de datos
        //                context.BulkInsert(archivoCsvETL.listaDetalle);
        //                context.SaveChanges();

        //                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
        //                bitacoraMifCsv.FechaModificacion = DateTime.Now;
        //                bitacoraMifCsv.Maquina = machine;
        //                bitacoraMifCsv.Usuario = user;
        //                bitacoraMifCsv.TipoArchivo = string.Format(Logica.BitacoraTipoArchivo);

        //                if (esReemplazo)
        //                    bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraReemplazar);
        //                else
        //                    bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraInsertar);

        //                bitacoraMifCsv.AccionExitosa = true;


        //                context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
        //                context.SaveChanges();

        //                respuesta.objObjeto = archivoCsvETL;
        //                respuesta.blnIndicadorTransaccion = true;
        //                respuesta.strMensaje = string.Format(Logica.MensajeArchivoInsertado);


        //                trans.Complete();

        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        respuesta.objObjeto = archivoCsvETL;
        //        respuesta.blnIndicadorTransaccion = false;
        //        respuesta.strMensaje = string.Format(Logica.ErrorAgregarArchivo, ex.Message);
        //    }

        //    return respuesta;
        //}

        //public Respuesta<bool> EliminarCsvETL(string nombreArchivo)
        //{
        //    nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
        //    string[] divisorNombre = nombreArchivo.Split('_');

        //    var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
        //                             Int32.Parse(divisorNombre[1].Substring(2, 2)),
        //                             Int32.Parse(divisorNombre[1].Substring(4, 2)));

        //    var CodSitio = divisorNombre[0];
        //    var CodConsecutivo = divisorNombre[2];

        //    Respuesta<bool> respuesta = new Respuesta<bool>();

        //    try
        //    {
        //        using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
        //        {

        //            var result = context.ArchivoTelevisionDigitalCsv.Where(t =>
        //                                                 t.CodigoConsecutivo == CodConsecutivo &&
        //                                                 t.CodigoSitio == CodSitio &&
        //                                                 t.FechaCapturaArchivo == fecha).FirstOrDefault();

        //            if (result != null)
        //            {

        //                context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarDetalleArchivoTelevisionDigitalCsv, result.IdArchivoCsv));
        //                context.SaveChanges();
        //                context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarArchivoTelevisionDigitalCsv, result.IdArchivoCsv));
        //                context.SaveChanges();
        //            }

        //            respuesta.objObjeto = true;
        //            return respuesta;

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        respuesta.objObjeto = false;
        //        respuesta.strMensaje = ex.Message;
        //        return respuesta;
        //    }
        //}



        //#endregion



        #region Archivo TV Digital

        /// <summary>
        /// Agrega un archivo ETL a base de datos
        /// </summary>
        /// <param name="archivoCsvETL">Archivo ETL</param>
        /// <param name="user">usuario</param>
        /// <param name="machine">máquina</param>
        /// <param name="esReemplazo">booleano si es reemplazo</param>
        /// <returns> Respuesta de Archivo de Television Digital</returns>
        public Respuesta<ArchivoTelevisionDigitalCsv> gAgregarArchivoCsvETL(ArchivoTelevisionDigitalCsv archivoCsvETL, string user, string machine, bool esReemplazo)
        {

            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoTelevisionDigitalCsv> respuesta = new Respuesta<ArchivoTelevisionDigitalCsv>();
            ArchivoTelevisionDigitalCsv csvInsertar = new ArchivoTelevisionDigitalCsv();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

            try
            {
                using (var context = new SUTEL_IndicadoresEntities())
                {

                    using (var trans = new TransactionScope())
                    {
                        csvInsertar.CodigoConsecutivo = archivoCsvETL.CodigoConsecutivo;
                        csvInsertar.CodigoSitio = archivoCsvETL.CodigoSitio;
                        csvInsertar.FechaCapturaArchivo = archivoCsvETL.FechaCapturaArchivo;

                        context.ArchivoTelevisionDigitalCsv.Add(csvInsertar);
                        context.SaveChanges();

                        var ultimoArchivoInsertado = context.ArchivoTelevisionDigitalCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();

                        IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;


                        if (esReemplazo)
                            bitacoraMifCsv.Accion = 3; // Reemplazar  
                        else
                            bitacoraMifCsv.Accion = 2;


                        for (int i = 0; i < archivoCsvETL.listaDetalle.Count; i++)
                        {
                            archivoCsvETL.listaDetalle[i].IdArchivoCsv = IdUltimoArchivoInsertado;
                        }

                        //Inserta todos los registros en base de datos
                        context.BulkInsert(archivoCsvETL.listaDetalle);
                        context.SaveChanges();

                        bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                        bitacoraMifCsv.FechaModificacion = DateTime.Now;
                        bitacoraMifCsv.Maquina = machine;
                        bitacoraMifCsv.Usuario = user;
                        bitacoraMifCsv.TipoArchivo = string.Format(Logica.BitacoraTipoArchivo);

                        if (esReemplazo)
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraReemplazar);
                        else
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraInsertar);

                        bitacoraMifCsv.AccionExitosa = true;


                        context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                        context.SaveChanges();

                        respuesta.objObjeto = archivoCsvETL;
                        respuesta.blnIndicadorTransaccion = true;
                        respuesta.strMensaje = string.Format(Logica.MensajeArchivoInsertado);


                        trans.Complete();

                    }

                }
            }

            catch (Exception ex)
            {
                respuesta.objObjeto = archivoCsvETL;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = string.Format(Logica.ErrorAgregarArchivo, ex.Message);
            }

            return respuesta;
        }

        public Respuesta<bool> EliminarCsvETL(string nombreArchivo)
        {
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
            string[] divisorNombre = nombreArchivo.Split('_');

            var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(2, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(4, 2)));

            var CodSitio = divisorNombre[0];
            var CodConsecutivo = divisorNombre[2];

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    var result = context.ArchivoTelevisionDigitalCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.CodigoSitio == CodSitio &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault();

                    if (result != null)
                    {

                        context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarDetalleArchivoTelevisionDigitalCsv, result.IdArchivoCsv));
                        context.SaveChanges();
                        context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarArchivoTelevisionDigitalCsv, result.IdArchivoCsv));
                        context.SaveChanges();
                    }

                    respuesta.objObjeto = true;
                    return respuesta;

                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }

        #endregion

        #region Archivo IMT Fijas

        /// <summary>
        /// Agrega un archivo IMT Fijas a base de datos
        /// </summary>
        /// <param name="archivoCsvIMTFija">Archivo IMT Fija</param>
        /// <param name="user">usuario</param>
        /// <param name="machine">máquina</param>
        /// <param name="esReemplazo">booleano si es reemplazo</param>
        /// <returns> Respuesta de Archivo de IMT Fija</returns>
        public Respuesta<ArchivoIMTFijasCsv> gAgregarArchivoCsvIMTFija(ArchivoIMTFijasCsv archivoCsvIMTFija, string user, string machine, bool esReemplazo)
        {

            
            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoIMTFijasCsv> respuesta = new Respuesta<ArchivoIMTFijasCsv>();
            ArchivoIMTFijasCsv csvInsertar = new ArchivoIMTFijasCsv();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

            try
            {
                using (var context = new SUTEL_IndicadoresEntities())
                {

                    using (var trans = new TransactionScope())
                    {
                        csvInsertar.CodigoConsecutivo = archivoCsvIMTFija.CodigoConsecutivo;
                        csvInsertar.Estacion = archivoCsvIMTFija.Estacion;
                        csvInsertar.FechaCapturaArchivo = archivoCsvIMTFija.FechaCapturaArchivo;
                        csvInsertar.nombreArchivo = archivoCsvIMTFija.nombreArchivo;
                        context.ArchivoIMTFijasCsv.Add(csvInsertar);
                        context.SaveChanges();

                        var ultimoArchivoInsertado = context.ArchivoIMTFijasCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();

                        IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;


                        if (esReemplazo)
                            bitacoraMifCsv.Accion = 3; // Reemplazar  
                        else
                            bitacoraMifCsv.Accion = 2;


                        for (int i = 0; i < archivoCsvIMTFija.listaDetalle.Count; i++)
                        {
                            archivoCsvIMTFija.listaDetalle[i].IdArchivoCsv = IdUltimoArchivoInsertado;
                        }

                        //Inserta todos los registros en base de datos
                        context.BulkInsert(archivoCsvIMTFija.listaDetalle);
                        context.SaveChanges();

                        bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                        bitacoraMifCsv.FechaModificacion = DateTime.Now;
                        bitacoraMifCsv.Maquina = machine;
                        bitacoraMifCsv.Usuario = user;
                        bitacoraMifCsv.TipoArchivo = string.Format(Logica.BitacoraTipoArchivo);

                        if (esReemplazo)
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraReemplazar);
                            //System.IO.File.Delete(rutaRemplazar);

                        else
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraInsertar);

                        bitacoraMifCsv.AccionExitosa = true;


                        context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                        context.SaveChanges();

                        respuesta.objObjeto = archivoCsvIMTFija;
                        respuesta.blnIndicadorTransaccion = true;
                        respuesta.strMensaje = string.Format(Logica.MensajeArchivoInsertado);


                        trans.Complete();

                    }

                }
            }

            catch (Exception ex)
            {
                respuesta.objObjeto = archivoCsvIMTFija;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = string.Format(Logica.ErrorAgregarArchivo, ex.Message);
            }

            return respuesta;
        }
        public Respuesta<bool> EliminarCsvIMT(string nombreArchivo, string rutaCarpetaInsertar, string rutaCarpetaReemplazar)
        {
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
            string[] divisorNombre = nombreArchivo.Split('_');

            var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(2, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(4, 2)));

            var Estacion = divisorNombre[3];
            var CodConsecutivo = divisorNombre[2];

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    var resultArchivCSV = context.ArchivoIMTFijasCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.Estacion == Estacion &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault();



                    if (resultArchivCSV != null)
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM DetalleArchivoIMTFijasCsv WHERE IdArchivoCsv =" + resultArchivCSV.IdArchivoCsv);
                        context.SaveChanges();

                        context.ArchivoIMTFijasCsv.Remove(resultArchivCSV);
                        context.SaveChanges();

                        System.IO.File.Delete(rutaCarpetaInsertar + "\\" + nombreArchivo + ".csv");
                        System.IO.File.Delete(rutaCarpetaReemplazar + "\\" + nombreArchivo + ".csv");

                        respuesta.objObjeto = true;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.objObjeto = false;
                        return respuesta;

                    }
                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }
        //public Respuesta<bool> EliminarCsvIMT(string nombreArchivo)
        //{
        //    nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
        //    string[] divisorNombre = nombreArchivo.Split('_');

        //    var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
        //                             Int32.Parse(divisorNombre[1].Substring(2, 2)),
        //                             Int32.Parse(divisorNombre[1].Substring(4, 2)));

        //    var Estacion = divisorNombre[3];
        //    var CodConsecutivo = divisorNombre[2];

        //    Respuesta<bool> respuesta = new Respuesta<bool>();

        //    try
        //    {
        //        using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
        //        {

        //            var result = context.ArchivoIMTFijasCsv.Where(t =>
        //                                                 t.CodigoConsecutivo == CodConsecutivo &&
        //                                                 t.Estacion == Estacion &&
        //                                                 t.FechaCapturaArchivo == fecha).FirstOrDefault();

        //            if (result != null)
        //            {

        //                context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarDetalleArchivoIMTEstacionesFijas, result.IdArchivoCsv));
        //                context.SaveChanges();
        //                context.Database.ExecuteSqlCommand(string.Format(Logica.EliminarArchivoIMTEstacionesFijas, result.IdArchivoCsv));
        //                context.SaveChanges();
        //            }

        //            respuesta.objObjeto = true;
        //            return respuesta;

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        respuesta.objObjeto = false;
        //        respuesta.strMensaje = ex.Message;
        //        return respuesta;
        //    }
        //}

        #endregion

        #region Archivo Banda Angosta

        /// <summary>
        /// Agrega un archivo Banda Angosta a base de datos
        /// </summary>
        /// <param name="archivoCsvBandaAn">Archivo Banda Angosta</param>
        /// <param name="user">usuario</param>
        /// <param name="machine">máquina</param>
        /// <param name="esReemplazo">booleano si es reemplazo</param>
        /// <returns> Respuesta de Archivo de Banda Angosta</returns>
        public Respuesta<ArchivoBandaAngostaCsv> gAgregarArchivoCsvBandaAngosta(ArchivoBandaAngostaCsv archivoCsvBandaAn, string user, string machine, bool esReemplazo)
        {

            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoBandaAngostaCsv> respuesta = new Respuesta<ArchivoBandaAngostaCsv>();
            ArchivoBandaAngostaCsv csvInsertar = new ArchivoBandaAngostaCsv();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

            try
            {
                using (var context = new SUTEL_IndicadoresEntities())
                {

                    using (var trans = new TransactionScope())
                    {
                        csvInsertar.CodigoConsecutivo = archivoCsvBandaAn.CodigoConsecutivo;
                        csvInsertar.Estacion = archivoCsvBandaAn.Estacion;
                        csvInsertar.FechaCapturaArchivo = archivoCsvBandaAn.FechaCapturaArchivo;

                        context.ArchivoBandaAngostaCsv.Add(csvInsertar);
                        context.SaveChanges();

                        var ultimoArchivoInsertado = context.ArchivoBandaAngostaCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();

                        IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;


                        if (esReemplazo)
                            bitacoraMifCsv.Accion = 3; // Reemplazar  
                        else
                            bitacoraMifCsv.Accion = 2;


                        for (int i = 0; i < archivoCsvBandaAn.listaDetalle.Count; i++)
                        {
                            archivoCsvBandaAn.listaDetalle[i].IdArchivoCsv = IdUltimoArchivoInsertado;
                        }

                        //Inserta todos los registros en base de datos
                        context.BulkInsert(archivoCsvBandaAn.listaDetalle);
                        context.SaveChanges();

                        bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                        bitacoraMifCsv.FechaModificacion = DateTime.Now;
                        bitacoraMifCsv.Maquina = machine;
                        bitacoraMifCsv.Usuario = user;
                        bitacoraMifCsv.TipoArchivo = string.Format(Logica.BitacoraTipoArchivo);

                        if (esReemplazo)
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraReemplazar);
                        else
                            bitacoraMifCsv.DetalleTransaccion = string.Format(Logica.MensajeBitacoraInsertar);

                        bitacoraMifCsv.AccionExitosa = true;


                        context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                        context.SaveChanges();

                        respuesta.objObjeto = archivoCsvBandaAn;
                        respuesta.blnIndicadorTransaccion = true;
                        respuesta.strMensaje = string.Format(Logica.MensajeArchivoInsertado);


                        trans.Complete();

                    }

                }
            }

            catch (Exception ex)
            {
                respuesta.objObjeto = archivoCsvBandaAn;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = string.Format(Logica.ErrorAgregarArchivo, ex.Message);
            }

            return respuesta;
        }

        public Respuesta<bool> EliminarCsvBandaAn(string nombreArchivo, string rutaCarpetaInsertar="", string rutaCarpetaReemplazar="")
        {
            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
            string[] divisorNombre = nombreArchivo.Split('_');

            var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(2, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(4, 2)));

            var Estacion = divisorNombre[3];
            var CodConsecutivo = divisorNombre[2];

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    var resultArchivCSV = context.ArchivoBandaAngostaCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.Estacion == Estacion &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault();



                    if (resultArchivCSV != null)
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM DetalleBandaAngostaCsv WHERE IdArchivoCsv =" + resultArchivCSV.IdArchivoCsv);
                        context.SaveChanges();

                        context.ArchivoBandaAngostaCsv.Remove(resultArchivCSV);
                        context.SaveChanges();

                        if (rutaCarpetaInsertar!="") {
                            System.IO.File.Delete(rutaCarpetaInsertar + "\\" + nombreArchivo + ".csv");

                        }if (rutaCarpetaReemplazar != "") {
                            System.IO.File.Delete(rutaCarpetaReemplazar + "\\" + nombreArchivo + ".csv");

                        }


                        respuesta.objObjeto = true;
                        return respuesta;
                    }
                    else {
                        respuesta.objObjeto = false;
                        return respuesta;

                    }
                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }

        #endregion


        public Respuesta<bool> EliminarCsv(string nombreArchivo)
        {

            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            //Se extraen los identificadores del nombre

            string[] divisorNombre = nombreArchivo.Split('_');

            var fecha = new DateTime(Int32.Parse("20" + divisorNombre[1].Substring(0, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(2, 2)),
                                     Int32.Parse(divisorNombre[1].Substring(4, 2)));

            var CodSitio = divisorNombre[0];
            var CodConsecutivo = divisorNombre[2];

            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    var result = context.ArchivoCsv.Where(t =>
                                                         t.CodigoConsecutivo == CodConsecutivo &&
                                                         t.CodigoSitio == CodSitio &&
                                                         t.FechaCapturaArchivo == fecha).FirstOrDefault();

                    if (result != null)
                    {

                        context.Database.ExecuteSqlCommand("DELETE FROM DetalleArchivoCsv WHERE IdArchivoCsv =" + result.IdArchivoCsv);

                        context.SaveChanges();

                        context.Database.ExecuteSqlCommand("DELETE FROM InformacionArchivoCsv WHERE IdArchivoCsv =" + result.IdArchivoCsv);

                        context.SaveChanges();

                        context.ArchivoCsv.Remove(context.ArchivoCsv.Where(t =>
                                                             t.CodigoConsecutivo == CodConsecutivo &&
                                                             t.CodigoSitio == CodSitio &&
                                                             t.FechaCapturaArchivo == fecha).FirstOrDefault());
                        context.SaveChanges();

                    }

                    respuesta.objObjeto = true;
                    return respuesta;

                }

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }


        public Respuesta<bool> EliminarMif(string nombreArchivo)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();

            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.Length - 4);

            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    var result = context.ArchivoMif.Where(t => t.NombreArchivoMif == nombreArchivo).FirstOrDefault();

                    if (result != null)
                    {

                        var coordenadasToDelete = context.CoordenadasArchivoMif.Where(t => t.IdArchivoMif == result.IdArchivoMif).ToList();

                        context.CoordenadasArchivoMif.RemoveRange(coordenadasToDelete);

                        context.SaveChanges();

                        context.ArchivoMif.Remove(context.ArchivoMif.Where(t => t.IdArchivoMif == result.IdArchivoMif).FirstOrDefault());
                        context.SaveChanges();


                    }

                }
                respuesta.objObjeto = true;
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }
        }


        public Respuesta<bool> GuardarArchivosEnBD(string usuario, string maquina, string detalleTransaccion, int AccionExitosa, int Accion)
        {

            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {

                    context.ExecJobCsv(Accion, AccionExitosa, detalleTransaccion, maquina, usuario);
                    context.ExecJobMif(Accion, AccionExitosa, detalleTransaccion, maquina, usuario);

                }

                respuesta.objObjeto = true;
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta.objObjeto = false;
                return respuesta;
            }

        }

        // OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION  OLD VERSION 




        #region metodos     

        /// <summary>
        /// Agrega un archivo Csv a la base de datos
        /// </summary>
        /// <param name="archivoCsv"></param>
        /// <returns></returns>
        public Respuesta<ArchivoCsvModel> gAgregarArchivoCsv(ArchivoCsvModel archivoCsv, string user, string machine, bool esReemplazo)
        {

            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoCsvModel> respuesta = new Respuesta<ArchivoCsvModel>();
            GB.SUTEL.Entities.ArchivoCsv csvInsertar = new Entities.ArchivoCsv();
            GB.SUTEL.Entities.DetalleArchivoCsv detalleArchivoCsv = new Entities.DetalleArchivoCsv();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();
            GB.SUTEL.Entities.InformacionArchivoCsv informacionArchivoCsv = new Entities.InformacionArchivoCsv();

            try
            {
                csvInsertar.CodigoConsecutivo = archivoCsv.CodigoConsecutivo;
                csvInsertar.CodigoSitio = archivoCsv.CodigoSitio;
                csvInsertar.FechaCapturaArchivo = archivoCsv.FechaCapturaArchivo;

                using (var context = new SUTEL_IndicadoresEntities())
                {

                    context.ArchivoCsv.Add(csvInsertar);

                    context.SaveChanges();

                    var ultimoArchivoInsertado = context.ArchivoCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();

                    IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;


                    if (esReemplazo)
                        bitacoraMifCsv.Accion = 3; // Reemplazar  
                    else
                        bitacoraMifCsv.Accion = 2;


                }

                //var stackSize = 100000000;

                //Thread t = new Thread(() => this.InsertarQuinceMilDetalleArchivoCsv(archivoCsv, IdUltimoArchivoInsertado), stackSize);

                ////Inicia el hilo
                //t.Start();

                //// Se continúa hasta que el hilo finalice
                //t.Join();



                for (int i = 0; i < archivoCsv.listaDetalle.Count; i++)
                    archivoCsv.listaDetalle[i].IdArchivoCsv = IdUltimoArchivoInsertado;

                using (var trans = new TransactionScope())
                {
                    using (var context = new SUTEL_IndicadoresEntities())
                    {
                        context.BulkInsert(archivoCsv.listaDetalle);

                        context.SaveChanges();

                        trans.Complete();
                    }
                }

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.Database.ExecuteSqlCommand("INSERT INTO InformacionArchivoCsv(IdArchivoCsv,InformacionArchivoCsv)VALUES(" + IdUltimoArchivoInsertado + ",'" + archivoCsv.informacionArchivo.Replace("'", " ") + "')");

                    context.SaveChanges();

                    //    informacionArchivoCsv.IdArchivoCsv = IdUltimoArchivoInsertado;
                    //    informacionArchivoCsv.InformacionArchivoCsv = archivoCsv.informacionArchivo;
                    //    context.InformacionArchivoCsv.Add(informacionArchivoCsv);
                    //    context.SaveChanges();
                }


                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                bitacoraMifCsv.FechaModificacion = DateTime.Now;
                bitacoraMifCsv.Maquina = machine;
                bitacoraMifCsv.Usuario = user;
                bitacoraMifCsv.TipoArchivo = "Csv";

                if (esReemplazo)
                    bitacoraMifCsv.DetalleTransaccion = "Reemplazado exitosamente";
                else
                    bitacoraMifCsv.DetalleTransaccion = "Insertado exitosamente";

                bitacoraMifCsv.AccionExitosa = true;


                using (var context = new SUTEL_IndicadoresEntities())
                {

                    context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                    context.SaveChanges();
                }

                respuesta.objObjeto = archivoCsv;
                respuesta.blnIndicadorTransaccion = true;
                respuesta.strMensaje = "El archivo fue insertado exitosamente";
            }

            catch (Exception ex)
            {
                //BORRAR REGISTROS
                this.BorrarRegistrosArchivoCsv(IdUltimoArchivoInsertado);

                respuesta.objObjeto = archivoCsv;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = "Error: " + ex.Message;
            }




            return respuesta;
        }


        public Respuesta<ArchivoMifModel> gAgregarArchivoMif(ArchivoMifModel archivoMif, string user, string machine, bool esReemplazo)
        {

            int IdUltimoArchivoInsertado = new int();

            Respuesta<ArchivoMifModel> respuesta = new Respuesta<ArchivoMifModel>();
            GB.SUTEL.Entities.ArchivoMif mifInsertar = new Entities.ArchivoMif();
            GB.SUTEL.Entities.CoordenadasArchivoMif coordenadasArchivoMif = new Entities.CoordenadasArchivoMif();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

            try
            {
                mifInsertar.NombreArchivoMif = archivoMif.nombreArchivoMif;
                mifInsertar.Region = archivoMif.region;

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.ArchivoMif.Add(mifInsertar);
                    context.SaveChanges();

                    if (esReemplazo)
                        bitacoraMifCsv.Accion = 3; // Reemplazar  
                    else
                        bitacoraMifCsv.Accion = 2;

                    var ultimoArchivoInsertado = context.ArchivoMif.OrderByDescending(u => u.IdArchivoMif).FirstOrDefault();

                    IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoMif;

                }


                for (int i = 0; i < archivoMif.listaCoordenadas.Count; i++)

                    archivoMif.listaCoordenadas[i].IdArchivoMif = IdUltimoArchivoInsertado;


                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.BulkInsert(archivoMif.listaCoordenadas);
                    context.SaveChanges();

                }

                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                bitacoraMifCsv.FechaModificacion = DateTime.Now;
                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                bitacoraMifCsv.Usuario = user;
                bitacoraMifCsv.Maquina = machine;
                bitacoraMifCsv.TipoArchivo = "Mif";

                if (esReemplazo)
                    bitacoraMifCsv.DetalleTransaccion = "Reemplazado exitosamente";
                else
                    bitacoraMifCsv.DetalleTransaccion = "Insertado exitosamente";

                bitacoraMifCsv.AccionExitosa = true;

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                    context.SaveChanges();
                }

                respuesta.objObjeto = archivoMif;
                respuesta.blnIndicadorTransaccion = true;
                respuesta.strMensaje = "El archivo fue insertado exitosamente";
            }

            catch (Exception ex)
            {

                this.BorrarRegistrosArchivoMif(IdUltimoArchivoInsertado);
                respuesta.objObjeto = archivoMif;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = "Error: " + ex.Message;
            }


            return respuesta;
        }




        public void BorrarRegistrosArchivoCsv(int IdUltimoArchivoInsertado)
        {

            using (var context = new SUTEL_IndicadoresEntities())
            {

                var detalle = context.DetalleArchivoCsv.Where(c => c.IdArchivoCsv == IdUltimoArchivoInsertado);
                if (detalle != null)
                {
                    context.DetalleArchivoCsv.RemoveRange(detalle);
                    context.SaveChanges();
                }
            }


            using (var context = new SUTEL_IndicadoresEntities())
            {
                var csv = context.ArchivoCsv.Where(c => c.IdArchivoCsv == IdUltimoArchivoInsertado).FirstOrDefault();
                if (csv != null)
                {
                    context.ArchivoCsv.Remove(csv);
                    context.SaveChanges();
                }
            }
        }


        public void BorrarRegistrosArchivoMif(int IdUltimoArchivoInsertado)
        {


            using (var context = new SUTEL_IndicadoresEntities())
            {

                var coordenadas = context.CoordenadasArchivoMif.Where(c => c.IdArchivoMif == IdUltimoArchivoInsertado);
                if (coordenadas != null)
                {
                    context.CoordenadasArchivoMif.RemoveRange(coordenadas);
                    context.SaveChanges();
                }

            }

            using (var context = new SUTEL_IndicadoresEntities())
            {
                var mif = context.ArchivoMif.Where(c => c.IdArchivoMif == IdUltimoArchivoInsertado).FirstOrDefault();
                if (mif != null)
                {
                    context.ArchivoMif.Remove(mif);
                    context.SaveChanges();
                }

            }


        }


        public Respuesta<ArchivoMifModel> gReemplazarArchivoMif(ArchivoMifModel archivoMif)
        {
            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoMifModel> respuesta = new Respuesta<ArchivoMifModel>();
            GB.SUTEL.Entities.ArchivoMif mifInsertar = new Entities.ArchivoMif();
            GB.SUTEL.Entities.CoordenadasArchivoMif coordenadasArchivoMif = new Entities.CoordenadasArchivoMif();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();

            try
            {


                mifInsertar.NombreArchivoMif = archivoMif.nombreArchivoMif;
                mifInsertar.Region = archivoMif.region;

                using (var context = new SUTEL_IndicadoresEntities())
                {

                    var resultado = context.ArchivoMif.Where(c => c.NombreArchivoMif == archivoMif.nombreArchivoMif)
                                                                  .FirstOrDefault();
                    if (null == resultado)
                    {
                        // algo raro
                        return new Respuesta<ArchivoMifModel>()
                        {
                            objObjeto = archivoMif,
                            strMensaje = "No se ha encontrado el archivo para reemplazarlo",
                            blnIndicadorTransaccion = false,
                            blnIndicadorState = -3

                        };
                    }
                    else
                    {

                        BorrarRegistrosArchivoMif(resultado.IdArchivoMif);

                        context.ArchivoMif.Add(mifInsertar);
                        context.SaveChanges();
                        bitacoraMifCsv.Accion = 3; // Crear

                    }

                }



                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                bitacoraMifCsv.FechaModificacion = DateTime.Now;
                bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                bitacoraMifCsv.Usuario = "Usuario";
                bitacoraMifCsv.Maquina = "Máquina";
                bitacoraMifCsv.TipoArchivo = "Mif";

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                    context.SaveChanges();
                }

                respuesta.objObjeto = archivoMif;
                respuesta.blnIndicadorState = -2;
                respuesta.blnIndicadorTransaccion = true;
                respuesta.strMensaje = "El archivo fue insertado exitosamente";
            }

            catch (Exception ex)
            {

                this.BorrarRegistrosArchivoMif(IdUltimoArchivoInsertado);
                respuesta.objObjeto = archivoMif;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = "Error: " + ex.Message;
            }


            return respuesta;
        }


        public Respuesta<ArchivoCsvModel> gReemplazarArchivoCsv(ArchivoCsvModel archivoCsv)
        {

            int IdUltimoArchivoInsertado = new int();
            Respuesta<ArchivoCsvModel> respuesta = new Respuesta<ArchivoCsvModel>();
            GB.SUTEL.Entities.ArchivoCsv csvInsertar = new Entities.ArchivoCsv();
            GB.SUTEL.Entities.DetalleArchivoCsv detalleArchivoCsv = new Entities.DetalleArchivoCsv();
            GB.SUTEL.Entities.BitacoraArchivosMifCsv bitacoraMifCsv = new Entities.BitacoraArchivosMifCsv();
            GB.SUTEL.Entities.InformacionArchivoCsv informacionArchivoCsv = new Entities.InformacionArchivoCsv();

            try
            {
                csvInsertar.CodigoConsecutivo = archivoCsv.CodigoConsecutivo;
                csvInsertar.CodigoSitio = archivoCsv.CodigoSitio;
                csvInsertar.FechaCapturaArchivo = archivoCsv.FechaCapturaArchivo;

                using (var context = new SUTEL_IndicadoresEntities())
                {

                    var resultado = context.ArchivoCsv.Where(c => c.CodigoConsecutivo == archivoCsv.CodigoConsecutivo &&
                                                                  c.CodigoSitio == archivoCsv.CodigoSitio &&
                                                                  c.FechaCapturaArchivo == archivoCsv.FechaCapturaArchivo.Date)
                                                                  .FirstOrDefault();
                    if (null == resultado)
                    {
                        // algo raro
                        return new Respuesta<ArchivoCsvModel>()
                        {
                            objObjeto = archivoCsv,
                            strMensaje = "No se ha encontrado el archivo para reemplazarlo",
                            blnIndicadorTransaccion = false,
                            blnIndicadorState = -3
                        };
                    }
                    else
                    {
                        context.ArchivoCsv.Add(csvInsertar);
                        context.SaveChanges();

                        var ultimoArchivoInsertado = context.ArchivoCsv.OrderByDescending(u => u.IdArchivoCsv).FirstOrDefault();
                        IdUltimoArchivoInsertado = ultimoArchivoInsertado.IdArchivoCsv;

                        bitacoraMifCsv.Accion = 3;
                    }


                }

                //var stackSize = 100000000;

                //Thread t = new Thread(() => this.InsertarQuinceMilDetalleArchivoCsv(archivoCsv, IdUltimoArchivoInsertado), stackSize);

                ////Inicia el hilo
                //t.Start();

                //// Se continúa hasta que el hilo finalice
                //t.Join();

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    informacionArchivoCsv.IdArchivoCsv = IdUltimoArchivoInsertado;
                    informacionArchivoCsv.InformacionArchivoCsv1 = archivoCsv.informacionArchivo;
                    context.InformacionArchivoCsv.Add(informacionArchivoCsv);
                    context.SaveChanges();
                }

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    bitacoraMifCsv.IdArchivo = IdUltimoArchivoInsertado;
                    bitacoraMifCsv.FechaModificacion = DateTime.Now;

                    bitacoraMifCsv.Maquina = "una maquina";
                    bitacoraMifCsv.Usuario = "usuario";
                    bitacoraMifCsv.TipoArchivo = "Csv";
                    context.BitacoraArchivosMifCsv.Add(bitacoraMifCsv);
                    context.SaveChanges();
                }

                respuesta.objObjeto = archivoCsv;
                respuesta.blnIndicadorState = -2;
                respuesta.blnIndicadorTransaccion = true;
                respuesta.strMensaje = "El archivo fue insertado exitosamente";
            }

            catch (Exception ex)
            {
                //BORRAR REGISTROS
                this.BorrarRegistrosArchivoCsv(IdUltimoArchivoInsertado);

                respuesta.objObjeto = archivoCsv;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = "Error: " + ex.Message;
            }




            return respuesta;
        }

        #endregion

    }
}
