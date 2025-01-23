# Teste para a Vaga de Desenvolvedor .NET

Este reposit�rio cont�m o projeto desenvolvido como teste para a vaga de Desenvolvedor .NET. 

## Estrutura do Projeto

O projeto foi separado em diferentes servi�os e contextos para garantir a independ�ncia entre eles. Essa abordagem facilita o trabalho em um contexto espec�fico sem depender dos outros.

## Metodologia

Para o desenvolvimento do projeto, foi utilizada a metodologia TDD (Test-Driven Development). Esta metodologia garante que os testes funcionem 100%, promovendo um c�digo mais robusto e confi�vel.

## Como Executar

1. Clone o reposit�rio:
    ```
    git clone https://github.com/RodrigoMartinsMoraes-Z/TestePonta
    ```

2. Navegue at� o diret�rio do projeto:
    ```
    cd TestePonta
    ```

3. Restaure as depend�ncias:
    ```
    dotnet restore
    ```

4. Execute os testes:
    ```
    dotnet test
    ```

5. Inicie a aplica��o:
5.1 Para cada API, navegue at� o diret�rio do projeto e execute o comando:
    ```
    dotnet run
    ```

URLs das APIs (swagger):
- API de Login: https://localhost:7286/index.html
- API de Usu�rios: https://localhost:7165/swagger/index.html
- API de Tarefa: https://localhost:7073/swagger/index.html
- API de Tarefas: https://localhost:7239/swagger/index.html

## Utiliza��o
1. Garanta que as API's est�o funcionando
2. Tenha um banco de dados Postgres para cada contexto (PONTA.TAREFA e PONTA.USUARIO) e ajuste conforme necess�rio nos scripts de configura��o do contexto.
3. Execute as migra��es atrav�z do prompt de comando no diretorio de cada contexto utilizando o comando: dotnet ef database update.
4. Acesse a API de usu�rio e crie um usu�rio novo.
5. Acesse a API de login e fa�a login.
6. Utilize o token gerado para utilizar as demais API's. 

## Tecnologias Utilizadas

- .NET Core
- Test-Driven Development (TDD)

## Observa�oes
Estive doente no periodo de desenvolvimento e n�o consegui fazer a tempo mais coisas, como utilizar models com o AutoMapper, valida��es de campos com o fluent validation, e testes unit�rios a prova de muta��es no c�digo.

## Contato

Se tiver alguma d�vida, entre em contato pelo whatsapp (44) 988 428 463
