# SIF - Sistema de Controle Financeiro

SIF, é um sistema para controle financeiro pessoal

### Telas/Cadastros

1. Cadastros
   - Clientes
   - Caixas
   - Formas de Pagamentos
   - Planos de Contas

2. Financeiro
   - Lançamento de Contas a Pagar
   - Lançamento de Contas a Receber
   - Gerador de Parcelas
   - Baixas de Contas a Pagar
   - Baixas de Contas a Receber
   - Baixa/Consulta Geral

3. Movimentação
   - Lançamentos das Baixas A Receber/Pagar

4. Painel/Configurações
   - Cadastro de Usuários
   - Alteração de Senha
   

### Telas de Exemplos
A telas seguem um padrão para ficar fácil usar todas

- Tela para cadastro de Contas a Receber

![cad contasreceber](https://user-images.githubusercontent.com/2820984/71309091-90f86380-23e2-11ea-8c50-fe0f9e122ca5.jpg)

- Tela para cadastro de Formas de Pagamentos

![cad fpagamentos](https://user-images.githubusercontent.com/2820984/71309111-e0d72a80-23e2-11ea-8246-a2751c5f8de4.jpg)

### Armazenamento dos dados
1. Para armazenar os dados, este projeto necessita do SGBD Microsoft SQL Server instalado na máquina ou em alguma máquina na rede
2. Configuração de acesso aos dados:
   - Arquivo XML de configuração: **SIFXml.xml**
   - **SIFXml.xml** está no mesmo local/pasta em que o .exe se encontra. Para ambiente de desenvolvimento em **'\Debug\'**
   - Para reconfigurar a conexão, basta excluir o arquivo e abrir o sistema novamente que a interface de configuração será exibida. Ou então editar o arquivo e trocar os dados da conexão manualmente
   
     
### Referências
Os estilos de todos os controles tem como base no projeto do repositório do link a seguir, o fonte original não está integrado ao sistema, foi feito uma cópia para fazer as mudanças necessárias em alguns controles para que o sistema tivesse uma aparência única e diferenciada.
https://github.com/firstfloorsoftware/mui
