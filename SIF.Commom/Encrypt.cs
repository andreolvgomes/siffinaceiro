using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    /// <summary>
    /// Encripta/Decripta informações
    /// </summary>
    public static class Encrypt
    {
        private const char CH = '♀';
        private const int LENGHT = 30;

        /// <summary>
        /// Encripta/Decripta informações
        /// </summary>
        /// <param name="informacao">informação a ser encriptada ou decriptada</param>
        /// <returns></returns>
        private static string EncryptString(string informacao)
        {
            return EncryptString(informacao.PadRight(LENGHT, CH), "SIF_OLIVER").Replace(CH.ToString(), "");
        }

        /// <summary>
        /// Encripta/Decripta informações
        /// </summary>
        /// <param name="informacao">informação a ser encriptada ou decriptada</param>
        /// <param name="pw_descricao">parâmetro da criptografia</param>
        /// <returns></returns>
        private static string EncryptString(string informacao, string pw_descricao)
        {
            int x = 0, n = 0;
            string descr = "";
            foreach (char c in informacao)
            {
                n = GetValue(c) ^ GetValue(pw_descricao[x]);
                descr += GetString(n);
                x++;
                if (x == pw_descricao.Length) x = 0;
            }
            return descr;
        }

        /// <summary>
        /// Pega a String de um binário
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        private static string GetString(int bit)
        {
            return Convert.ToChar(bit).ToString();
        }

        /// <summary>
        /// Pega o binário de uma string
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        private static int GetValue(char digit)
        {
            return Convert.ToUInt16(digit);
        }
    }
}
