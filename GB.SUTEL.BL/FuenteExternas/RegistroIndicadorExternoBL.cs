using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using System.Globalization;
using OfficeOpenXml;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using GB.SUTEL.DAL.FuenteExternas;


namespace GB.SUTEL.BL.FuenteExternas
{
    public class RegistroIndicadorExternoBL : LocalContextualizer
    {
        private Respuesta<RegistroIndicadorExterno> objRespuesta;
        private RegistroIndicadorExternoDA objRegistroIndicadorExternoDA;
        public RegistroIndicadorExternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<RegistroIndicadorExterno>();
            objRegistroIndicadorExternoDA = new RegistroIndicadorExternoDA(appContext);
        }

        #region Agregar
        public Respuesta<RegistroIndicadorExterno> Agregar(RegistroIndicadorExterno objRegistroIndicadorExterno, string ValorIndicador)
        {
            try
            {
                var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                objRegistroIndicadorExterno.ValorIndicador = Convert.ToDouble(ValorIndicador.Replace(",", decSep).Replace(".", decSep));
                IndicadorExternoBL refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                IndicadorExterno IndicadorExterno = refIndicadorExternoBL.ConsultarPorExpresion(x => x.IdIndicadorExterno == objRegistroIndicadorExterno.IdIndicadorExterno).objObjeto;
                if (objRegistroIndicadorExternoDA.Single(x =>  !x.Borrado &&
                    x.IndicadorExterno.FuenteExterna.IdFuenteExterna == IndicadorExterno.IdFuenteExterna &&
                    x.IdIndicadorExterno == objRegistroIndicadorExterno.IdIndicadorExterno &&
                    x.ValorIndicador == objRegistroIndicadorExterno.ValorIndicador&&
                    x.IdPeridiocidad == objRegistroIndicadorExterno.IdPeridiocidad &&
                    x.IdZonaIndicadorExterno == objRegistroIndicadorExterno.IdZonaIndicadorExterno &&
                    x.IdCanton == objRegistroIndicadorExterno.IdCanton &&
                    x.IdGenero == objRegistroIndicadorExterno.IdGenero&&
                    x.IdTrimestre == objRegistroIndicadorExterno.IdTrimestre&&
                    x.Anno == objRegistroIndicadorExterno.Anno).objObjeto == null)
                {
                    objRegistroIndicadorExterno.IdRegistroIndicadorExterno = Guid.NewGuid();
                    objRespuesta = objRegistroIndicadorExternoDA.Agregar(objRegistroIndicadorExterno);
                    objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DuplicatedRegistry;
                    objRespuesta.blnIndicadorState = 300;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<RegistroIndicadorExterno> Agregar(List<RegistroIndicadorExterno> objRegistroIndicadorExterno)
        {
            try
            {
                IndicadorExternoBL refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                IndicadorExterno IndicadorExterno;
                int cont = 0;
                foreach (var item in objRegistroIndicadorExterno)
                {
                    IndicadorExterno = refIndicadorExternoBL.ConsultarPorExpresion(x => x.IdIndicadorExterno == item.IdIndicadorExterno).objObjeto;
                    if (objRegistroIndicadorExternoDA.Single(x => !x.Borrado &&
                        x.IndicadorExterno.FuenteExterna.IdFuenteExterna == IndicadorExterno.IdFuenteExterna &&
                        x.IdIndicadorExterno == item.IdIndicadorExterno &&
                        x.ValorIndicador == item.ValorIndicador &&
                        x.IdPeridiocidad == item.IdPeridiocidad &&
                        x.IdZonaIndicadorExterno == item.IdZonaIndicadorExterno &&
                        x.IdCanton == item.IdCanton &&
                        x.IdGenero == item.IdGenero &&
                        x.IdTrimestre == item.IdTrimestre &&
                        x.Anno == item.Anno).objObjeto == null)
                    {
                        cont++;
                        item.IdRegistroIndicadorExterno = Guid.NewGuid();
                        objRespuesta = objRegistroIndicadorExternoDA.Agregar(item);
                    }
                }
                if(cont != objRegistroIndicadorExterno.Count())
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.NombreDuplicado;
                    objRespuesta.blnIndicadorState = 300;                    
                }                
                objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }



        #endregion

        #region Editar
        public Respuesta<RegistroIndicadorExterno[]> Editar(RegistroIndicadorExterno objRegistroIndicadorExterno, string ValorIndicador)
        {
            Respuesta<RegistroIndicadorExterno[]> objRespuesta = new Respuesta<RegistroIndicadorExterno[]>();
            try
            {
                var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                objRegistroIndicadorExterno.ValorIndicador = Convert.ToDouble(ValorIndicador.Replace(",",decSep).Replace(".",decSep));
                IndicadorExternoBL refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                IndicadorExterno IndicadorExterno = refIndicadorExternoBL.ConsultarPorExpresion(x => x.IdIndicadorExterno == objRegistroIndicadorExterno.IdIndicadorExterno).objObjeto;
                var RegistroIndicadorExterno = objRegistroIndicadorExternoDA.Single(x => !x.Borrado &&
                    x.IndicadorExterno.FuenteExterna.IdFuenteExterna == IndicadorExterno.IdFuenteExterna &&
                    x.IdIndicadorExterno == objRegistroIndicadorExterno.IdIndicadorExterno &&
                    x.ValorIndicador == objRegistroIndicadorExterno.ValorIndicador &&
                    x.IdPeridiocidad == objRegistroIndicadorExterno.IdPeridiocidad &&
                    x.IdZonaIndicadorExterno == objRegistroIndicadorExterno.IdZonaIndicadorExterno &&
                    x.IdCanton == objRegistroIndicadorExterno.IdCanton &&
                    x.IdGenero == objRegistroIndicadorExterno.IdGenero &&
                    x.IdTrimestre == objRegistroIndicadorExterno.IdTrimestre &&
                    x.Anno == objRegistroIndicadorExterno.Anno).objObjeto;

                if (RegistroIndicadorExterno == null || RegistroIndicadorExterno.IdRegistroIndicadorExterno==objRegistroIndicadorExterno.IdRegistroIndicadorExterno)
                {
                    if (RegistroIndicadorExterno == null)
                    {
                        objRespuesta = objRegistroIndicadorExternoDA.Editar(objRegistroIndicadorExterno);
                        objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                    }
                    else
                    {
                        RegistroIndicadorExterno.IdIndicadorExterno = objRegistroIndicadorExterno.IdIndicadorExterno;
                        RegistroIndicadorExterno.IdPeridiocidad = objRegistroIndicadorExterno.IdPeridiocidad;
                        RegistroIndicadorExterno.IdZonaIndicadorExterno = objRegistroIndicadorExterno.IdZonaIndicadorExterno;
                        RegistroIndicadorExterno.IdRegionIndicadorExterno = objRegistroIndicadorExterno.IdRegionIndicadorExterno;
                        RegistroIndicadorExterno.Anno = objRegistroIndicadorExterno.Anno;
                        RegistroIndicadorExterno.IdTrimestre = objRegistroIndicadorExterno.IdTrimestre;
                        RegistroIndicadorExterno.IdCanton = objRegistroIndicadorExterno.IdCanton;
                        RegistroIndicadorExterno.IdGenero = objRegistroIndicadorExterno.IdGenero;
                        RegistroIndicadorExterno.ValorIndicador = objRegistroIndicadorExterno.ValorIndicador;

                        objRespuesta = objRegistroIndicadorExternoDA.Editar(RegistroIndicadorExterno);
                        objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DuplicatedRegistry;
                    objRespuesta.blnIndicadorState = 300;
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        public Respuesta<List<RegistroIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<RegistroIndicadorExterno>> objRespuesta = new Respuesta<List<RegistroIndicadorExterno>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<Trimestre>> ConsultarTrimestres()
        {
            Respuesta<List<Trimestre>> objRespuesta = new Respuesta<List<Trimestre>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.ConsultarTrimestres();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        /// <summary>
        /// consulta de provincias
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Provincia>> gConsultarProvincias()
        {

            Respuesta<List<Provincia>> objRespuesta = new Respuesta<List<Provincia>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.gConsultarProvincias();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<Canton>> ConsultarCantones()
        {
            Respuesta<List<Canton>> objRespuesta = new Respuesta<List<Canton>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.ConsultarCantones();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<Canton>> ConsultarCantonesXprovincia(int idProvincia)
        {
            Respuesta<List<Canton>> objRespuesta = new Respuesta<List<Canton>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.ConsultarCantonesXProvincia(idProvincia);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<Genero>> ConsultarGeneros()
        {
            Respuesta<List<Genero>> objRespuesta = new Respuesta<List<Genero>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.ConsultarGeneros();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Filtro
        public Respuesta<List<RegistroIndicadorExterno>> gFiltrarRegistroIndicadorExterno(string fuente, string indicador, double valor,
            string anno, string zona, string region)
        {
            Respuesta<List<RegistroIndicadorExterno>> objRespuesta = new Respuesta<List<RegistroIndicadorExterno>>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.gFiltrarRegistroIndicadorExterno(fuente,indicador,valor,anno,zona,region);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarPorExpresion
        public Respuesta<RegistroIndicadorExterno> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<RegistroIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.Single(expression);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        public Respuesta<RegistroIndicadorExterno> Eliminar(Guid id)
        {
            Respuesta<RegistroIndicadorExterno> objRespuesta = new Respuesta<RegistroIndicadorExterno>();
            try
            {
                objRespuesta = objRegistroIndicadorExternoDA.Eliminar(id);
                objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion


        #region LeerArchivos

        public Respuesta<List<RegistroIndicadorExterno>> leerXlsx(ExcelWorksheet ws, ExcelWorksheet ws2, int startRow) 
        {
            List<RegistroIndicadorExterno> respuestaBitacora = new List<RegistroIndicadorExterno>();
            if ( ws != null)
                {
                    RegistroIndicadorExterno newRegistroIndicadorExterno = new RegistroIndicadorExterno();
                    int lastRow = ws.Dimension.End.Row;
                    int lastRow2 = ws2.Dimension.End.Row;
                    for (int i = startRow; i <= lastRow;i++ )
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 2].Value != null && ws.Cells[i, 3].Value != null &&
                            ws.Cells[i, 4].Value != null && ws.Cells[i, 5].Value != null && ws.Cells[i, 6].Value != null &&
                            ws.Cells[i, 7].Value != null && ws.Cells[i, 8].Value != null && ws.Cells[i, 9].Value != null &&
                            ws.Cells[i, 10].Value != null)
                        {
                            newRegistroIndicadorExterno = new RegistroIndicadorExterno();
                            //fuente externa
                            var indicador = ws2.Cells[2, 3, lastRow2, 3].Where(x => x.Value == ws.Cells[i, 2].Value).FirstOrDefault();
                            string idindicador = ws2.Cells[indicador.Start.Row, 4].Value.ToString();
                            //perdiodicidad
                            var periodicidad = ws2.Cells[2, 5, lastRow2, 5].Where(x => x.Value == ws.Cells[i, 3].Value).FirstOrDefault();
                            int idperiodicidad;
                            int.TryParse(ws2.Cells[periodicidad.Start.Row, 6].Value.ToString(), out idperiodicidad);
                            //zona
                            var zona = ws2.Cells[2, 7, lastRow2, 7].Where(x => x.Value == ws.Cells[i, 4].Value).FirstOrDefault();
                            int idzona;
                            int.TryParse(ws2.Cells[zona.Start.Row, 8].Value.ToString(), out idzona);
                            //region
                            var region = ws2.Cells[2, 9, lastRow2, 9].Where(x => x.Value == ws.Cells[i, 5].Value).FirstOrDefault();
                            int idregion;
                            int.TryParse(ws2.Cells[region.Start.Row, 10].Value.ToString(), out idregion);
                            //Año                        
                            int anio;
                            int.TryParse(ws.Cells[i, 6].Value.ToString(), out anio);
                            //trimestre
                            var trimestre = ws2.Cells[2, 13, lastRow2, 13].Where(x => x.Value == ws.Cells[i, 7].Value).FirstOrDefault();
                            int idtrimestre;
                            int.TryParse(ws2.Cells[trimestre.Start.Row, 14].Value.ToString(), out idtrimestre);
                            //canton
                            var canton = ws2.Cells[2, 15, lastRow2, 15].Where(x => x.Value == ws.Cells[i, 8].Value).FirstOrDefault();
                            int idcanton;
                            int.TryParse(ws2.Cells[canton.Start.Row, 16].Value.ToString(), out idcanton);
                            //genero
                            var genero = ws2.Cells[2, 17, lastRow2, 17].Where(x => x.Value == ws.Cells[i, 9].Value).FirstOrDefault();
                            int idgenero;
                            int.TryParse(ws2.Cells[genero.Start.Row, 18].Value.ToString(), out idgenero);
                            //valor
                            //int valor;
                            //int.TryParse(ws.Cells[i, 10].Value.ToString(), out valor);
                            string valor = ws.Cells[i, 10].Value.ToString();

                            newRegistroIndicadorExterno.IdIndicadorExterno = idindicador;
                            newRegistroIndicadorExterno.IdPeridiocidad = idperiodicidad;
                            newRegistroIndicadorExterno.IdZonaIndicadorExterno = idzona;
                            newRegistroIndicadorExterno.IdRegionIndicadorExterno = idregion;
                            newRegistroIndicadorExterno.Anno = anio;
                            newRegistroIndicadorExterno.IdTrimestre = idtrimestre;
                            newRegistroIndicadorExterno.IdCanton = idcanton;
                            newRegistroIndicadorExterno.IdGenero = idgenero;
                            //newRegistroIndicadorExterno.ValorIndicador = valor;                            
                            Agregar(newRegistroIndicadorExterno, valor);
                            respuestaBitacora.Add(newRegistroIndicadorExterno);
                        }
                    }
                }

            Respuesta<List<RegistroIndicadorExterno>> respuesta = new Respuesta<List<RegistroIndicadorExterno>>();
            respuesta.objObjeto = respuestaBitacora;

            return respuesta;
        }

        public Respuesta<RegistroIndicadorExterno> leerXls(ISheet ws, ISheet ws2, int startRow, List<IndicadorExterno> IndicadorExterno,
            List<Periodicidad> Periodicidades, List<ZonaIndicadorExterno> Zona, List<RegionIndicadorExterno> Region, List<Trimestre> Trimestre,
            List<Canton> Canton, List<Genero> Genero)
        {
            Respuesta<RegistroIndicadorExterno> respuesta = new Respuesta<RegistroIndicadorExterno>();
            startRow = startRow - 1;
            if (ws != null)
            {
                
                RegistroIndicadorExterno newRegistroIndicadorExterno = new RegistroIndicadorExterno();
                int lastRow = ws.LastRowNum;
               int lastRow2 = ws2.LastRowNum;
                for (int i = startRow; i <= lastRow; i++)
                {
                    if (ws.GetRow(i).GetCell(0)  != null && ws.GetRow(i).GetCell(1) != null && ws.GetRow(i).GetCell(2) != null &&
                        ws.GetRow(i).GetCell(3) != null && ws.GetRow(i).GetCell(4) != null && ws.GetRow(i).GetCell(5) != null &&
                        ws.GetRow(i).GetCell(6) != null && ws.GetRow(i).GetCell(7) != null && ws.GetRow(i).GetCell(8) != null &&
                        ws.GetRow(i).GetCell(9) != null)
                    {
                        newRegistroIndicadorExterno = new RegistroIndicadorExterno();
                        
                
                       // //fuente externa
                       var indicador = IndicadorExterno.Where(x => x.Nombre == ws.GetRow(i).GetCell(1).ToString()).FirstOrDefault();
                       string idIndicador = indicador.IdIndicadorExterno.ToString();
                       //perdiodicidad
                       var periodicidad = Periodicidades.Where(y =>y.DescPeriodicidad == ws.GetRow(i).GetCell(2).ToString()).FirstOrDefault();
                       int idperiodicidad = Convert.ToInt32(periodicidad.IdPeridiocidad);
                       //zona
                       var zona = Zona.Where(x => x.DescZonaIndicadorExterno == ws.GetRow(i).GetCell(3).ToString()).FirstOrDefault();
                       int idzona = Convert.ToInt32(zona.IdZonaIndicadorExterno);
                       //region
                       int idregion = Region.Where(x => x.DescRegionIndicadorExterno == ws.GetRow(i).GetCell(4).ToString()).FirstOrDefault().IdRegionIndicadorExterno;
                       //Año                        
                       int anio = Convert.ToInt32(ws.GetRow(i).GetCell(5).ToString());
                       //trimestre
                       int idtrimestre = Trimestre.Where(x => x.Nombre == ws.GetRow(i).GetCell(6).ToString()).FirstOrDefault().IdTrimestre;
                       //canton
                       int idcanton = Canton.Where(x => x.Nombre == ws.GetRow(i).GetCell(7).ToString()).FirstOrDefault().IdCanton;
                       //genero
                       int idgenero = Genero.Where(x => x.Descripcion == ws.GetRow(i).GetCell(8).ToString()).FirstOrDefault().IdGenero;
                       //valor
                       string valor = ws.GetRow(i).GetCell(9).ToString();

                       newRegistroIndicadorExterno.IdIndicadorExterno = idIndicador;
                       newRegistroIndicadorExterno.IdPeridiocidad = idperiodicidad;
                       newRegistroIndicadorExterno.IdZonaIndicadorExterno = idzona;
                       newRegistroIndicadorExterno.IdRegionIndicadorExterno = idregion;
                       newRegistroIndicadorExterno.Anno = anio;
                       newRegistroIndicadorExterno.IdTrimestre = idtrimestre;
                       newRegistroIndicadorExterno.IdCanton = idcanton;
                       newRegistroIndicadorExterno.IdGenero = idgenero;
                       //newRegistroIndicadorExterno.ValorIndicador = valor;
                       respuesta = Agregar(newRegistroIndicadorExterno, valor);
                    }
                }
            }


        

            return respuesta;
        
        }
        #endregion

       
    }
}
