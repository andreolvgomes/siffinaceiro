using SIF.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao.Providers
{
    public class ProviderCadCaixas : ProviderInterfacesCadastros2<Caixas>//: INotifyPropertyChanged, IDisposable
    {
        private DataView _ViewMovimentacao;

        public DataView ViewMovimentacao
        {
            get { return _ViewMovimentacao; }
            set
            {
                if (_ViewMovimentacao != value)
                {
                    _ViewMovimentacao = value;
                    OnPropertyChanged("ViewMovimentacao");
                }
            }
        }

        public ProviderCadCaixas(Window janela, UserControls.Buttons buttons, ConnectionDb conexao)
            : base(janela, buttons, conexao)
        {
            this.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Caixas>(PreparaDadosInterface);
            this.Event_UpdateBemSucedidoEventHandler += new ExecucaoCommandsCadastrosEventHandler(UpdateSucesso);
        }

        private void UpdateSucesso()
        {
            PreecheListMovimentacao(Entidade.Cai_movdomes);
        }

        private void PreparaDadosInterface(Caixas model)
        {
            PreecheListMovimentacao(model.Cai_movdomes);
        }

        private void PreecheListMovimentacao(bool cai_movdomes)
        {
            try
            {
                string commandtext = string.Format(@"SELECT 
	                      Clientes.Cli_nome
                          ,Camovimentos.Cam_valorlancado
                          ,CASE WHEN Camovimentos.Cam_tipomovimento = 'D' THEN 'Débito' ELSE 'Crédito' END AS Cam_tipomovimento
                          ,Camovimentos.Cam_datalancamento
                          ,Camovimentos.Cam_observacao
                    FROM dbo.Camovimentos
                    INNER JOIN Clientes ON Camovimentos.Cli_codigo = Clientes.Cli_codigo 
                    WHERE Camovimentos.Cai_codigo = '{0}'", Entidade.Cai_codigo);
                if (cai_movdomes)
                {
                    commandtext += string.Format(" AND CONVERT(VARCHAR, Camovimentos.Cam_datalancamento, 103) LIKE CONVERT(VARCHAR, '%{0:MM/yyyy}', 103)", DateTime.Now);
                }

                commandtext += " ORDER BY Camovimentos.Cam_datalancamento DESC";

                this.ViewMovimentacao = SistemaGlobal.Sis.Connection.GetDataView(commandtext);
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex);
            }
        }
    }
}
