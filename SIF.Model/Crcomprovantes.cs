using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Crcomprovantes : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Crcomprovantes"; }
        }

        private string _Crc_sequencial;
        [CamposSqlAttibutes("Crc_sequencial", IsPrimaryKey = true, IsIdentity = true)]
        public string Crc_sequencial
        {
            get { return _Crc_sequencial; }
            set
            {
                if (_Crc_sequencial != value)
                {
                    _Crc_sequencial = value;
                    NotifyPropertyChanged("Crc_sequencial");
                }
            }
        }

        private int _Crf_sequencial;
        [CamposSqlAttibutes("Crf_sequencial")]
        public int Crf_sequencial
        {
            get { return _Crf_sequencial; }
            set
            {
                if (_Crf_sequencial != value)
                {
                    _Crf_sequencial = value;
                    NotifyPropertyChanged("Crf_sequencial");
                }
            }
        }

        private int _Crc_tipo;
        [CamposSqlAttibutes("Crc_tipo")]
        public int Crc_tipo
        {
            get { return _Crc_tipo; }
            set
            {
                if (_Crc_tipo != value)
                {
                    _Crc_tipo = value;
                    NotifyPropertyChanged("Crc_tipo");
                }
            }
        }

        private int _Crc_observacao;
        [CamposSqlAttibutes("Crc_observacao")]
        public int Crc_observacao
        {
            get { return _Crc_tipo; }
            set
            {
                if (_Crc_observacao != value)
                {
                    _Crc_observacao = value;
                    NotifyPropertyChanged("Crc_observacao");
                }
            }
        }

        private Byte[] _Crc_imagem;
        [CamposSqlAttibutes("Crc_imagem")]
        public Byte[] Crc_imagem
        {
            get { return _Crc_imagem; }
            set
            {
                if (_Crc_imagem != value)
                {
                    _Crc_imagem = value;
                    NotifyPropertyChanged("Cli_foto");
                }
            }
        }
    }
}
