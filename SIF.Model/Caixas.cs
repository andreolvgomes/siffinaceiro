using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Caixas : IModelProperty, IDataErrorInfo
    {
        public override string NomeTabela
        {
            get { return "Caixas"; }
        }

        private string _Cai_codigo;
        [CamposSqlAttibutes("Cai_codigo", IsPrimaryKey = true)]
        public string Cai_codigo
        {
            get { return _Cai_codigo; }
            set
            {
                if (_Cai_codigo != value)
                {
                    _Cai_codigo = value;
                    NotifyPropertyChanged("Cai_codigo");
                }
            }
        }

        private bool _Cai_movdomes;
        [CamposSqlAttibutes("Cai_movdomes")]
        public bool Cai_movdomes
        {
            get { return _Cai_movdomes; }
            set
            {
                if (_Cai_movdomes != value)
                {
                    _Cai_movdomes = value;
                    NotifyPropertyChanged("Cai_movdomes");
                }
            }
        }

        private string _Cai_descricao;
        [CamposSqlAttibutes("Cai_descricao")]
        public string Cai_descricao
        {
            get { return _Cai_descricao; }
            set
            {
                if (_Cai_descricao != value)
                {
                    _Cai_descricao = value;
                    NotifyPropertyChanged("Cai_descricao");
                }
            }
        }

        private string _Cai_observacao;
        [CamposSqlAttibutes("Cai_observacao")]
        public string Cai_observacao
        {
            get { return _Cai_observacao; }
            set
            {
                if (_Cai_observacao != value)
                {
                    _Cai_observacao = value;
                    NotifyPropertyChanged("Cai_observacao");
                }
            }
        }

        public static Caixas GetNewCaixa()
        {
            Caixas caixas = new Caixas();
            caixas.Cai_codigo = "";
            caixas.Cai_descricao = "";
            caixas.Cai_observacao = "";
            return caixas;
        }

        private decimal _Cai_saldo;
        [CamposSqlAttibutes("Cai_saldo")]
        public decimal Cai_saldo
        {
            get { return _Cai_saldo; }
            set
            {
                if (_Cai_saldo != value)
                {
                    _Cai_saldo = value;
                    NotifyPropertyChanged("Cai_saldo");
                }
            }
        }

        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Cai_codigo":
                        return string.IsNullOrEmpty(Cai_codigo) ? "Informe o código do caixa" : null;
                    case "Cai_descricao":
                        return string.IsNullOrEmpty(Cai_descricao) ? "Informe a descrição do caixa" : null;
                }
                return null;
            }
        }
    }
}
