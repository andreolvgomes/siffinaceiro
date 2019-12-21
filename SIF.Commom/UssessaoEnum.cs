using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Commom
{
    public enum UssessaoEnum
    {
        [Description("Baixa Contas a Pagar")]
        BAIXA_CONTAS_PAGAR,
        [Description("Baixa Contas a Receber")]
        BAIXA_CONTAS_RECEBER,
        [Description("Cadastro Caixas")]
        CADASTRO_CAIXA,
        [Description("Cadastro Clientes")]
        CADADASTRO_CLIENTES,
        [Description("Cadastro F.Pagamentos")]
        CADASTRO_FPAGAMENTO,
        [Description("Cadastro Plano Contas")]
        CADASTRO_PLANO_CONTAS,
        [Description("Cadastro Produtos")]
        CADASTRO_PRODUTOS,
        [Description("Cadastro Usuários")]
        CADASTRO_USUARIOS,
        [Description("Lançamento Contas a Pagar")]
        LANCAMENTO_CONTAS_PAGAR,
        [Description("Lançamento Contas a Receber")]
        LANCAMENTO_CONTAS_RECEBER,
        [Description("Lançamento de Movimentação")]
        LANCAMENTO_MOVIMENTACAO,
    }
}
