using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Gravação de log de exceções
    /// </summary>
    public class LogException
    {
        /// <summary>
        /// Grava log da exceção com opção de mostrar msg de error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="displayMsg"></param>
        public bool LogError(Exception ex, bool displayMsg)
        {
            return this.LogError(ex, displayMsg, null);
        }

        /// <summary>
        /// Grava log da exceção com opção de mostrar msg de erro e set a window pai
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="display"></param>
        /// <param name="owner"></param>
        public bool LogError(Exception ex, bool display, Window owner)
        {
            this.LogError(ex);
            if (display)
                SistemaGlobal.Sis.Msg.MostraMensagem(ex.Message, "Error Exception", owner, MessageBoxImage.Error);
            return false;
        }

        /// <summary>
        /// Grava log da exceção
        /// </summary>
        /// <param name="ex"></param>
        public bool LogError(Exception ex)
        {
            string pathDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Logs", "Erros");
            string pathFile = string.Format("Log_{0:dd-MM-yyyy}.xml", DateTime.Now);
            try
            {
                if (!System.IO.Directory.Exists(pathDirectory)) System.IO.Directory.CreateDirectory(pathDirectory);
                pathFile = System.IO.Path.Combine(pathDirectory, pathFile);

                if (!System.IO.File.Exists(pathFile))
                {
                    XDocument xml = new XDocument(new XDeclaration("1.0", "UTF-8", "Yes"), new XElement("LogErros"));
                    xml.Save(pathFile);
                }

                //System.Reflection.MethodBase.GetCurrentMethod();

                XElement doc = XElement.Load(pathFile);
                XElement e = new XElement("Error"
                    , new XElement("DataHora", string.Format("Data: {0:dd/MM/yyyy} Hora: {1:HH:mm:ss}", DateTime.Now, DateTime.Now))
                    , new XElement(ex.Source
                        , new XElement("Message", ex.Message)
                        , new XElement("TargetSite", ex.TargetSite)
                        , new XElement("StackTrace", ex.StackTrace)
                    ));
                doc.Add(e);

                doc.Save(pathFile);
            }
            catch
            {
            }
            return false;
        }
    }
}
