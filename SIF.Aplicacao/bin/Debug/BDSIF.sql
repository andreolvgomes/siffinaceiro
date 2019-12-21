--CREATE DATABASE BDSIF
--GO

--USE BDSCP
--GO

CREATE TABLE Clientes
(
	Cli_codigo		INT IDENTITY(1, 1) NOT NULL,
	Cli_nome		VARCHAR(50) DEFAULT '' NOT NULL,
	Cli_nomerazao	VARCHAR(50)	DEFAULT '' NOT NULL,
	Cli_endereco	VARCHAR(50)	DEFAULT '' NOT NULL,
	Cli_numero		INT DEFAULT 0 NOT NULL,
	Cli_bairro		VARCHAR(30)	DEFAULT '' NOT NULL,
	Cli_cidade		VARCHAR(30)	DEFAULT '' NOT NULL,
	Cli_uf			VARCHAR(2)	DEFAULT '' NOT NULL,
	Cli_cep			VARCHAR(9)	DEFAULT '' NOT NULL,
	Cli_complemento	VARCHAR(50)	DEFAULT '' NOT NULL,
	Cli_tipopessoa	INT DEFAULT 0 NOT NULL,
	Cli_cpfcnpj		VARCHAR(18) DEFAULT '' NOT NULL,
	Cli_extra1		VARCHAR(50)	DEFAULT '' NOT NULL,
	Cli_extra2		VARCHAR(50)	DEFAULT '' NOT NULL,
	Cli_apelido		VARCHAR(30)	DEFAULT '' NOT NULL,
	Cli_celular		VARCHAR(15) DEFAULT '' NOT NULL,
	Cli_fone1		VARCHAR(15)	DEFAULT '' NOT NULL,
	Cli_fone2		VARCHAR(15)	DEFAULT '' NOT NULL,
	Cli_datanascimento	SMALLDATETIME NULL,
	Cli_naturalidade	VARCHAR(30)	DEFAULT '' NOT NULL,
	Cli_estadocivil		VARCHAR(20)	DEFAULT '' NOT NULL,
	Cli_sexo			VARCHAR(9)	DEFAULT '' NOT NULL,
	Cli_observacao		VARCHAR(200)	DEFAULT '' NOT NULL,
	Cli_foto			IMAGE NULL,
	
CONSTRAINT PK_Clientes PRIMARY KEY (Cli_codigo)
)
GO

CREATE TABLE Caixas
(
	Cai_codigo		VARCHAR(3)		DEFAULT '' NOT NULL,
	Cai_descricao	VARCHAR(30)		DEFAULT	'' NOT NULL,
	Cai_observacao	VARCHAR(200)		DEFAULT '' NOT NULL,
	Cai_saldo		DECIMAL(10,2)	DEFAULT 0 NOT NULL,
	Cai_movdomes	BIT DEFAULT(0) NOT NULL
CONSTRAINT PK_Caixas PRIMARY KEY (Cai_codigo)
)
GO

CREATE TABLE Planocontas
(
	Pla_numeroconta		VARCHAR(20)	DEFAULT '' NOT NULL,
	Pla_descricao	VARCHAR(50) DEFAULT '' NOT NULL,

CONSTRAINT PK_Planocontas PRIMARY KEY (Pla_numeroconta)
)
GO

CREATE TABLE Fpagamentos
(
	Fpa_codigo		VARCHAR(2)	DEFAULT '' NOT NULL,
	Fpa_descricao	VARCHAR(30)	DEFAULT '' NOT NULL,
	Fpa_observacao	VARCHAR(200)	DEFAULT '' NOT NULL
	
CONSTRAINT PK_Fpagamentos PRIMARY KEY (Fpa_codigo)
)
GO


CREATE TABLE Camovimentos
(
	Cam_sequencial	INT IDENTITY(1, 1) NOT NULL,
	Cli_codigo		INT NOT NULL,
	Fpa_codigo		VARCHAR(2)	NOT NULL,
	Pla_numeroconta	VARCHAR(20)	NOT NULL,
	Cai_codigo		VARCHAR(3)	NOT NULL,
	Cam_valorlancado	DECIMAL(10, 2)	DEFAULT 0 NOT NULL,
	Cam_tipomovimento	VARCHAR(1)	DEFAULT '' NOT NULL,
	Cam_datalancamento	DATETIME NOT NULL,
	Cam_observacao		VARCHAR(500)		DEFAULT '' NOT NULL,
	Crf_sequencial	INT NOT NULL
CONSTRAINT PK_Camovimentos PRIMARY KEY (Cam_sequencial),
CONSTRAINT FK_Camovimentos_Clientes	FOREIGN KEY (Cli_codigo) REFERENCES Clientes(Cli_codigo),
CONSTRAINT FK_Camovimentos_Fpagamentos	FOREIGN KEY (Fpa_codigo) REFERENCES Fpagamentos(Fpa_codigo) ON UPDATE CASCADE,
CONSTRAINT FK_Camovimentos_Planocontas	FOREIGN KEY (Pla_numeroconta) REFERENCES Planocontas(Pla_numeroconta) ON UPDATE CASCADE,
CONSTRAINT FK_Camovimentos_Caixas	FOREIGN KEY (Cai_codigo) REFERENCES Caixas(Cai_codigo) ON UPDATE CASCADE
)
GO

CREATE TABLE Crfinanceiro
(
	Crf_sequencial		INT IDENTITY(1, 1) NOT NULL,
	Cli_codigo			INT NOT NULL,
	Fpa_codigo			VARCHAR(2)	NOT NULL,
	Pla_numeroconta		VARCHAR(20)	NOT NULL,
	Crf_ndocumento		VARCHAR(20)	DEFAULT '' NOT NULL,
	Crf_tipoconta		VARCHAR(2)	DEFAULT '' NOT NULL,
	Crf_parcela			VARCHAR(2)	DEFAULT ''	NOT NULL,
	Crf_valorparcela	DECIMAL(10, 2)	DEFAULT 0	NOT NULL,
	Crf_valorareceber	DECIMAL(10, 2)	DEFAULT 0	NOT NULL,		
	Crf_valordocumento	DECIMAL(10, 2) DEFAULT 0	NOT NULL,
	Crf_valorrecebido	DECIMAL(10, 2) DEFAULT 0	NOT NULL,
	Crf_datalancamento	DATETIME	NULL,
	Crf_datavencimento	DATETIME	NULL,
	Crf_databaixa		DATETIME	NULL,
	Crf_observacao		VARCHAR(500)	DEFAULT '' NOT NULL,
	Crf_empagamento		BIT DEFAULT(0) NOT NULL
	
CONSTRAINT PK_Crfinanceiro PRIMARY KEY (Crf_sequencial),
CONSTRAINT FK_Crfinanceiro_Clientes	FOREIGN KEY (Cli_codigo) REFERENCES Clientes(Cli_codigo),
CONSTRAINT FK_Crfinanceiro_Fpagamentos	FOREIGN KEY (Fpa_codigo) REFERENCES Fpagamentos(Fpa_codigo) ON UPDATE CASCADE,
CONSTRAINT FK_Crfinanceiro_Planocontas	FOREIGN KEY (Pla_numeroconta) REFERENCES Planocontas(Pla_numeroconta) ON UPDATE CASCADE
)
GO

CREATE TABLE Crcomprovantes
(
	 Crc_sequencial	INT NOT NULL IDENTITY(1, 1)
	,Crf_sequencial	INT NOT NULL
	,Crc_tipo	VARCHAR(2) NOT NULL
	,Crc_imagem	IMAGE NOT NULL
	,Crc_observacao	VARCHAR(500)

CONSTRAINT PK_Crcomprovantes PRIMARY KEY (Crc_sequencial),
CONSTRAINT FK_Crcomprovantes_Crfinanceiro FOREIGN KEY (Crf_sequencial) REFERENCES Crfinanceiro(Crf_sequencial)
)
GO

CREATE TABLE Crbaixas
(
	Crb_sequencial		INT	IDENTITY(1, 1)		NOT NULL,
	Crf_sequencial		INT						NOT NULL,
	Fpa_codigo			VARCHAR(2)				NOT NULL,
	Pla_numeroconta		VARCHAR(20)				NOT NULL,
	Cai_codigo			VARCHAR(3)				NOT NULL,
	Crb_tipodoconta		VARCHAR(2)	DEFAULT ''	NOT NULL,
	Crb_valorecebido			DECIMAL(10, 2)	DEFAULT 0		NOT NULL,
	Crb_databaixa		DATETIME				NOT NULL,
	Crb_observacao		VARCHAR(500)		DEFAULT ''		NOT NULL
	
CONSTRAINT PK_Crbaixas PRIMARY KEY (Crb_sequencial),
CONSTRAINT FK_Crbaixas_Crfinanceiro FOREIGN KEY (Crf_sequencial) REFERENCES Crfinanceiro(Crf_sequencial),
CONSTRAINT FK_Crbaixas_Fpagamentos FOREIGN KEY (Fpa_codigo) REFERENCES Fpagamentos(Fpa_codigo) ON UPDATE CASCADE,
CONSTRAINT FK_Crbaixas_Planocontas FOREIGN KEY (Pla_numeroconta) REFERENCES Planocontas(Pla_numeroconta) ON UPDATE CASCADE,
CONSTRAINT FK_Crbaixas_Caixas FOREIGN KEY (Cai_codigo) REFERENCES Caixas(Cai_codigo) ON UPDATE CASCADE
)
GO

CREATE TABLE Usmenu
(
	Usm_sequencial INT IDENTITY(1, 1) NOT NULL,
	Usm_descricao	VARCHAR(30) NOT NULL

CONSTRAINT PK_Usmenu PRIMARY KEY (Usm_sequencial)
)

CREATE TABLE Usuarios
(
	Usu_codigo	INT IDENTITY(1, 1)	NOT NULL,
	Usu_nome	VARCHAR(50)		NOT NULL,
	Usu_senha	VARCHAR(30)		NOT NULL,
	Usu_perfil	VARCHAR(30)	DEFAULT 'ADMINISTRADOR'	NOT NULL,
	Usu_observacao VARCHAR(200) DEFAULT '' NOT NULL,
	--Usu_cor		VARCHAR(20)	DEFAULT '#FF00ABA9' NOT NULL,
	--Usu_font	VARCHAR(10)	DEFAULT 'large' NOT NULL
	
CONSTRAINT PK_Usuarios	PRIMARY KEY (Usu_codigo)
)
GO

CREATE TABLE Ussecao
(
	Uss_descricao	VARCHAR(50)		NOT NULL,
	Usm_sequencial	INT NOT NULL

CONSTRAINT PK_Ussecao PRIMARY KEY (Uss_descricao),
CONSTRAINT FK_Ussecao_Usmenu FOREIGN KEY (Usm_sequencial) REFERENCES Usmenu(Usm_sequencial)
)
GO

CREATE TABLE Uscontrolesecao
(
	Usu_codigo	INT		NOT NULL,
	Uss_descricao	VARCHAR(50)	DEFAULT '' NOT NULL,
	Usc_disponivel	BIT	DEFAULT 0		NOT NULL,
	Usc_incluir		BIT	DEFAULT 0		NOT NULL,
	Usc_excluir		BIT DEFAULT 0		NOT NULL,
	Usc_editar		BIT DEFAULT 0		NOT NULL,
	
CONSTRAINT PK_Uscontrolesecao PRIMARY KEY (Usu_codigo, Uss_descricao),
CONSTRAINT FK_Uscontrolesecao_Usuarios FOREIGN KEY (Usu_codigo) REFERENCES Usuarios(Usu_codigo),
CONSTRAINT FK_Uscontrolesecao_Ussecao FOREIGN KEY (Uss_descricao) REFERENCES Ussecao(Uss_descricao) ON UPDATE CASCADE
)
GO

INSERT INTO Usmenu (Usm_descricao) VALUES ('Cadastros')
INSERT INTO Usmenu (Usm_descricao) VALUES ('Financeiro')
INSERT INTO Usmenu (Usm_descricao) VALUES ('Movimentação')
INSERT INTO Usmenu (Usm_descricao) VALUES ('Painel')
GO

--USUÁRIO PADRÃO
INSERT INTO Usuarios (Usu_nome, Usu_senha) VALUES ('ADM', 'adm')

--SESSÃO PADRÃO
INSERT INTO Ussecao VALUES('Cadastro Clientes', 1)
INSERT INTO Ussecao VALUES('Cadastro Produtos', 1)
INSERT INTO Ussecao VALUES('Cadastro Caixas', 1)
INSERT INTO Ussecao VALUES('Cadastro F.Pagamentos', 1)
INSERT INTO Ussecao VALUES('Cadastro Plano Contas', 1)
INSERT INTO Ussecao VALUES('Cadastro Usuários', 4)
INSERT INTO Ussecao VALUES('Lançamento Contas a Receber', 2)
INSERT INTO Ussecao VALUES('Lançamento Contas a Pagar', 2)
INSERT INTO Ussecao VALUES('Baixa Contas a Pagar', 2)
INSERT INTO Ussecao VALUES('Baixa Contas a Receber', 2)
INSERT INTO Ussecao VALUES('Lançamento de Movimentação', 3)

--INSERT PERMISSÕES
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro Clientes', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro Produtos', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro Caixas', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro F.Pagamentos', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro Plano Contas', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Cadastro Usuários', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Lançamento Contas a Receber', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Lançamento Contas a Pagar', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Baixa Contas a Pagar', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Baixa Contas a Receber', 1, 1, 1, 1)
INSERT INTO Uscontrolesecao VALUES (1, 'Lançamento de Movimentação', 1, 1, 1, 1)
GO

INSERT INTO Caixas
           (Cai_codigo
           ,Cai_descricao
           ,Cai_observacao
           ,Cai_saldo)
     VALUES
           ('001'
           ,'CAIXA PADRÃO'
           ,''
           ,0)
GO

INSERT INTO Planocontas
           (Pla_numeroconta
           ,Pla_descricao)
     VALUES
           ('0.0.0.0.000.000.0001'
           ,'PLANO DE CONTAS PADRÃO')
GO

INSERT INTO Fpagamentos
           (Fpa_codigo
           ,Fpa_descricao
           ,Fpa_observacao)
     VALUES
           ('01'
           ,'DINHEIRO'
           ,'')
GO

INSERT INTO Clientes
           (Cli_nome
           ,Cli_nomerazao
           ,Cli_endereco
           ,Cli_numero
           ,Cli_bairro
           ,Cli_cidade
           ,Cli_uf
           ,Cli_cep
           ,Cli_complemento
           ,Cli_tipopessoa
           ,Cli_cpfcnpj
           ,Cli_extra1
           ,Cli_extra2
           ,Cli_apelido
           ,Cli_celular
           ,Cli_fone1
           ,Cli_fone2
           ,Cli_datanascimento
           ,Cli_naturalidade
           ,Cli_estadocivil
           ,Cli_sexo
           ,Cli_observacao
           ,Cli_foto)
     VALUES
           ('CLIENTE PADRÃO'
           ,'CLIENTE PADRÃO'
           ,'ENDEREÇO PADRÇAO'
           ,0
           ,'PADRÃO'
           ,'PADRÃO'
           ,'GO'
           ,''
           ,''
           ,0
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,NULL
           ,''
           ,''
           ,''
           ,''
           ,NULL)
GO


