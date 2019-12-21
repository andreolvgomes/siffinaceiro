using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Cotrole dos botões na interface
    /// </summary>
    public enum ControleButtons
    {
        /// <summary>
        /// Registro em edição
        /// </summary>
        Edit,
        /// <summary>
        /// Navegando entre os registros
        /// </summary>
        Navigate,
        /// <summary>
        /// Novo registro
        /// </summary>
        New,
    }

    /// <summary>
    /// Botões da tela de cadatro e outras
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// Nenhum
        /// </summary>
        None,
        /// <summary>
        /// Novo
        /// </summary>
        New,
        /// <summary>
        /// Save
        /// </summary>
        Save,
        /// <summary>
        /// Cancel
        /// </summary>
        Cancel,
        /// <summary>
        /// Delete
        /// </summary>
        Delete,
        /// <summary>
        /// Próximo registro
        /// </summary>
        Next,
        /// <summary>
        /// Registro anterior
        /// </summary>
        Previous,
        /// <summary>
        /// Primeiro registro
        /// </summary>
        First,
        /// <summary>
        /// Último registro
        /// </summary>
        Last,
        /// <summary>
        /// Em edição
        /// </summary>
        Alter,
        /// <summary>
        /// Em pesquisa
        /// </summary>
        Pesquisa,
    }
}
