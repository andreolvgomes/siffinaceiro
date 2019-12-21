using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL
{
    public enum PASSOS_CONFIGURACAO
    {
        PASSO_ESCOLHA_ARMAZENAMENTO,
        PASSO_ARMAZENAMENTO_EM_ARQUIVO,
        PASSO_ARMAZENAMENTO_EM_ISGBD,
        PASSO_CADASTRO_USUARIO,
        PASSO_CONCLUISAO,
    }

    public enum CONFIGURACAO_ARQUIVO
    {
        NOVO_ARQUIVO,
        ARQUIVO_EXISTENTE,
    }

    public enum TIPO_ARMAZENAMENTO
    {
        ARMAZENAMENTO_EM_ARQUIVO,
        ARMAZENAMENTO_BANCO_DE_DADOS,
    }
}
