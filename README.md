# Teste para a Vaga de Desenvolvedor .NET

Este repositório contém o projeto desenvolvido como teste para a vaga de Desenvolvedor .NET. 

## Estrutura do Projeto

O projeto foi separado em diferentes serviços e contextos para garantir a independência entre eles. Essa abordagem facilita o trabalho em um contexto específico sem depender dos outros.

## Metodologia

Para o desenvolvimento do projeto, foi utilizada a metodologia TDD (Test-Driven Development). Esta metodologia garante que os testes funcionem 100%, promovendo um código mais robusto e confiável.

## Como Executar

1. Clone o repositório:
    ```
    git clone https://github.com/RodrigoMartinsMoraes-Z/TestePonta
    ```

2. Navegue até o diretório do projeto:
    ```
    cd TestePonta
    ```

3. Restaure as dependências:
    ```
    dotnet restore
    ```

4. Execute os testes:
    ```
    dotnet test
    ```

5. Inicie a aplicação:
5.1 Para cada API, navegue até o diretório do projeto e execute o comando:
    ```
    dotnet run
    ```

URLs das APIs (swagger):
- API de Login: https://localhost:7286/index.html
- API de Usuários: https://localhost:7165/swagger/index.html
- API de Tarefa: https://localhost:7073/swagger/index.html
- API de Tarefas: https://localhost:7239/swagger/index.html

## Utilização
1. Garanta que as API's estão funcionando
2. Tenha um banco de dados Postgres para cada contexto (PONTA.TAREFA e PONTA.USUARIO) e ajuste conforme necessário nos scripts de configuração do contexto.
3. Execute as migrações atravéz do prompt de comando no diretorio de cada contexto utilizando o comando: dotnet ef database update.
4. Acesse a API de usuário e crie um usuário novo.
5. Acesse a API de login e faça login.
6. Utilize o token gerado para utilizar as demais API's. 

## Tecnologias Utilizadas

- .NET Core
- Test-Driven Development (TDD)

## Observaçoes
Estive doente no periodo de desenvolvimento e não consegui fazer a tempo mais coisas, como utilizar models com o AutoMapper, validações de campos com o fluent validation, e testes unitários a prova de mutações no código.

## Contato

Se tiver alguma dúvida, entre em contato pelo whatsapp (44) 988 428 463
