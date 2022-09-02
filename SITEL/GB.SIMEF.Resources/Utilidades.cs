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


        public static Regex rx_alfanumerico_v2 = new Regex(@"^[A-Za-z0-9ÁÉÍÓÚáéíóúñÑ]+([ ][A-Za-z0-9ÁÉÍÓÚáéíóúñÑ]+)*$", RegexOptions.Compiled);


        public static bool ValidarEmail(string email)
        {
            return email != null && Regex.IsMatch(email, "^(([\\w-]+\\.)+[\\w -]+|([a-zA-Z]{1}|[\\w -]{2,}))@(([a-zA -Z]+[\\w-]+\\.){1,2}[a-zA-Z]{2,4})$");
        }




        public static string ConcatenadoCombos(string  codigo, string nombre)
        {
            return string.Format("{0} / {1}",codigo,nombre) ;
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
