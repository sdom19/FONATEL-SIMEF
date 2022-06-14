using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Communication
{
    public class SutelAD : LocalContextualizer
    {
        public SutelAD(ApplicationContext appContext) : base (appContext)
        {

        }
        public Respuesta<List<UsuarioAD>> ConsultarTodos()
        {
            Respuesta<List<UsuarioAD>> objRespuesta = new Respuesta<List<UsuarioAD>>();
            List<UsuarioAD> oUseres = new List<UsuarioAD>();
            try
            {
                var AD = ConfigurationManager.AppSettings["SutelActiveDirectory"].ToString();
                var ADuser = ConfigurationManager.AppSettings["SutelADuser"].ToString();
                var ADpassword = ConfigurationManager.AppSettings["SutelADpassword"].ToString();
                ADuser = ADuser.Replace("/", @"\");
                using (var context = new PrincipalContext(ContextType.Domain, AD, ADuser, ADpassword))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        foreach (var result in searcher.FindAll())
                        {
                            //DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                            UsuarioAD newUser = new UsuarioAD();
                            newUser.Nombre = result.Name;
                            newUser.SAM = result.SamAccountName;



                            oUseres.Add(newUser);
                        }
                    }
                }                
                objRespuesta.objObjeto = oUseres.OrderBy(x => x.SAM).ToList();                
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(String.Format(msj,((CustomException)ex).Id), null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        public bool LogIn(string username, string password)
        {
            List<UsuarioAD> oUseres = new List<UsuarioAD>();
            try
            {
                var AD = ConfigurationManager.AppSettings["SutelActiveDirectory"].ToString();
                var ADuser = ConfigurationManager.AppSettings["SutelADuser"].ToString();
                var ADpassword = ConfigurationManager.AppSettings["SutelADpassword"].ToString();
                ADuser = ADuser.Replace("/", @"\");
                using (var context = new PrincipalContext(ContextType.Domain, AD, ADuser, ADpassword))
                {

                    string searchForThis = "\\";
                    int Inicio = ADuser.IndexOf(searchForThis);
                    string sub = ADuser.Substring(0, Inicio + 1);
                    string User = sub + username;

                    //using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    //{
                    //    foreach (var result in searcher.FindAll())
                    //    {
                    //        //DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                    //        UsuarioAD newUser = new UsuarioAD();
                    //        newUser.Nombre = result.Name;
                    //        newUser.SAM = result.SamAccountName;
                    //        oUseres.Add(newUser);
                    //    }
                    //}
                    return context.ValidateCredentials(username, password,ContextOptions.Sealing); 
                    
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(String.Format(msj), ex);
            }
        }




    }
}
