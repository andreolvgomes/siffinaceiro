USE BDSIF
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

INSERT INTO Caixas(Cai_codigo,Cai_descricao,Cai_observacao,Cai_saldo)
     VALUES('001','CAIXA PADRÃO','',0)
GO

INSERT INTO Planocontas(Pla_numeroconta,Pla_descricao)
     VALUES('0.0.0.0.000.000.0001','PLANO DE CONTAS PADRÃO')
GO

INSERT INTO Fpagamentos(Fpa_codigo,Fpa_descricao,Fpa_observacao)     
VALUES('01','DINHEIRO','')
GO

INSERT INTO Clientes(Cli_nome,Cli_nomerazao,Cli_endereco,Cli_numero,Cli_bairro,Cli_cidade,Cli_uf,Cli_cep,Cli_complemento,Cli_tipopessoa,Cli_cpfcnpj,Cli_extra1,Cli_extra2,Cli_apelido,Cli_celular,Cli_fone1,Cli_fone2,Cli_datanascimento,Cli_naturalidade,Cli_estadocivil,Cli_sexo,Cli_observacao,Cli_foto)
     VALUES('CLIENTE PADRÃO','CLIENTE PADRÃO','ENDEREÇO PADRÇAO',0,'PADRÃO','PADRÃO','GO','','',0,'','','','','','','',NULL,'','','','',NULL)
GO
