using JsonDiffPatchDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GB.SIMEF.Resources
{
    public static class Utilidades
    {

        /// <summary>
        /// Michael Hernandez Cordero
        /// 21-04-2023
        /// Metodo para generar el JSON enviado a bitacora
        /// </summary>
        /// <returns></returns>
        /// 
        public static string GeneradorUrlJson(string indicadores)
        {
            return string.Format("{{\"Indicadores\":\"{0}\"}}", indicadores);
        }



        public static string RutaCarpeta(string rutaCapeta, string opcion = "")
        {
            if (opcion == EtiquetasViewCategorias.Categorias)
            {
                return string.Format(@"{0}\Categorias", rutaCapeta);
            }
            else if (opcion == EtiquetasViewRelacionCategoria.RelacionCategoria)
            {
                return string.Format(@"{0}\Relaciones", rutaCapeta);
            }
            else
            {
                return string.Format(@"{0}\RegistroIndicador", rutaCapeta);
            }
            
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Valida si la cadena es alfanumérica: Letras del alfabeto, números, tildes (utilizadas en español) y la eñe (ñ). Acepta espacios
        /// </summary>
        public static Regex rx_alfanumerico = new Regex(@"^[0-9A-Za-zÁÉÍÓÚáéíóúñÑ,;.\- ]+$", RegexOptions.Compiled);

        /// <summary>
        /// 22/08/2022
        /// Michael Hernández Cordero
        /// Yerlin ordeno quitar los numericos
        /// Valida si la cadena es alfanumérica: Letras del alfabeto, números, tildes (utilizadas en español) y la eñe (ñ). Acepta espacios
        /// </summary>
        public static Regex rx_alfanumerico2 = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúñÑ,;. ]+$", RegexOptions.Compiled);

        /// <summary>
        /// 13/08/2022
        /// José Navarro Acuña
        /// Valida si la cadena es únicamente texto: Letras del alfabeto, tildes (utilizadas en español) y la eñe (ñ). Acepta espacios
        /// </summary>
        public static Regex rx_soloTexto = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúñÑ,. ]+$", RegexOptions.Compiled);

        public static bool ValidarEmail(string email)
        {
            return email != null && Regex.IsMatch(email, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@(([a-zA-Z]+[\\w-]+\\.){1,2}[a-zA-Z]{2,4})$");
        }

        public static string fechaColumna(DateTime fecha)
        {
            return fecha.ToString("MMM") +"-"+fecha.ToString("yyyy");
        }

        public static string fechaSinHora(DateTime fecha)
        {
            return fecha.ToString("dd-MM-yy");
        }

        public static string jsonDiff(string json1, string json2)
        {
            string result = "";
            if (!String.IsNullOrEmpty(json1))
            {
                var opts = new JsonDiffPatchDotNet.Options
                {
                    ArrayDiff = ArrayDiffMode.Simple,
                    TextDiff = TextDiffMode.Simple
                };
                var diffObj = new JsonDiffPatch(opts);
                result = diffObj.Diff(json1, json2);
                
            }
            return result;

        }

        public static string ConcatenadoCombos(string  codigo, string nombre)
        {
            return string.Format("{0} / {1}",codigo,nombre) ;
        }

        /// <summary>
        /// 29/08/2022
        /// José Navarro Acuña
        /// Obtener el valor encriptado por defecto de los componentes dropdown
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultDropDownValue()
        {
            return Encriptar(Constantes.defaultDropDownValue.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cadenaAencriptar"></param>
        /// <returns></returns>
        public static string Encriptar(string _cadenaAencriptar)
        {
            string objRespuesta = null;
            byte[] btEncriptacion = Encoding.Unicode.GetBytes(_cadenaAencriptar);
            btEncriptacion = EncriptarByte(btEncriptacion);
            objRespuesta = Convert.ToBase64String(btEncriptacion);
            return objRespuesta;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string Desencriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar.Replace(' ', '+'));
            decryted = DesencriptarByte(decryted);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public static string DesencriptarArray(string _cadenaAdesencriptar)
        {
            string result = string.Empty;

            string[] cadenaArray = _cadenaAdesencriptar.Split(',');

            for (int i = 0; i<cadenaArray.Length; i++)
            {
                _cadenaAdesencriptar = cadenaArray[i];

                byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar.Replace(' ', '+'));
                decryted = DesencriptarByte(decryted);
                result =result+","+ System.Text.Encoding.Unicode.GetString(decryted);
            }

            return result.Trim(',');
        }

        private static byte[] EncriptarByte(byte[] btValor)
        {
            byte[] valorEncriptado = null; 
            PasswordDeriveBytes pdbPassBytes =
            new PasswordDeriveBytes(Constantes.CifradoDatos.strPermutacion,
                new byte[] { Constantes.CifradoDatos.intBytePermutacionUno,
                         Constantes.CifradoDatos.intBytePermutacionDos,
                         Constantes.CifradoDatos.intBytePermutacionTres,
                         Constantes.CifradoDatos.intBytePermutacionCuatro
                });

                MemoryStream msStream = new MemoryStream();
                Aes aes = new AesManaged();
                aes.Key = pdbPassBytes.GetBytes(aes.KeySize / Constantes.CifradoDatos.intDivisionPassword);
                aes.IV = pdbPassBytes.GetBytes(aes.BlockSize / Constantes.CifradoDatos.intDivisionPassword);

                CryptoStream csCryptoStream = new CryptoStream(msStream,
                aes.CreateEncryptor(), CryptoStreamMode.Write);
                csCryptoStream.Write(btValor,0, btValor.Length);
                csCryptoStream.Close();
                valorEncriptado = msStream.ToArray();
      
            return valorEncriptado;
        }

        private static byte[] DesencriptarByte(byte[] btValor)
        {
            byte[] objRespuesta = null;

                PasswordDeriveBytes pdbPassBytes =
                    new PasswordDeriveBytes(Constantes.CifradoDatos.strPermutacion,
                    new byte[] { Constantes.CifradoDatos.intBytePermutacionUno,
                         Constantes.CifradoDatos.intBytePermutacionDos,
                         Constantes.CifradoDatos.intBytePermutacionTres,
                         Constantes.CifradoDatos.intBytePermutacionCuatro
                    });

                MemoryStream msStream = new MemoryStream();
                Aes aes = new AesManaged();
                aes.Key = pdbPassBytes.GetBytes(aes.KeySize / Constantes.CifradoDatos.intDivisionPassword);
                aes.IV = pdbPassBytes.GetBytes(aes.BlockSize / Constantes.CifradoDatos.intDivisionPassword);

                CryptoStream csCryptoStream = new CryptoStream(msStream,
                aes.CreateDecryptor(), CryptoStreamMode.Write);
                csCryptoStream.Write(btValor, 0, btValor.Length);
                csCryptoStream.Close();
                objRespuesta = msStream.ToArray();
             return objRespuesta;
        }
    }
}
