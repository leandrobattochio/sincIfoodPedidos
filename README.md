# Introdução

Este projeto surgiu como uma ideia de fazer um gerenciador de gastos pessoais. Um sistema que poderia usar dados de diversos serviços para compor os gastos, como por exemplo: Uber, iFood, Rappi, etc.

Inicialmente decidi implementar apenas o iFood. Para isso analisei qual era o processo para fazer o login no iFood através do Google Chrome Developer tools.
Após analisar as requisições consegui implementar com sucesso um cliente que consiga fazer o login no ifood apenas usando o método de login via EMAIL. Não implementei os demais métodos de login para economia de tempo nessa POC.



# Tecnologias e conceitos aplicados

## Injeção de Dependência

## Repositórios

## Domínios ricos
Evitei usar entidades anêmicas para constriuir as entidades do domínio. A manipulação das entidades é feita estritamente através de métodos criados na própria entidade. Todas as entidades devem ser construidas obrigatoriamente através do construtor onde pode ser feita toda a lógica para setar os dados de forma segura.

## AutoMapper
Foi utilizado o auto mapper para facilitar a criação de objetos diferentes mas com características iguais.

## HangFire
Foi utilizado o hangfire para tarefas em background. Foi utilizado para obter todos os pedidos feitos na sua conta e popular o banco de dados. Como isso é um processo demorado, a chamada da rota não pode esperar até que isso aconteca. Então a rota retorna um sucesso "processo iniciado" enquando o job roda em background sincronizando os pedidos.

## Polly
Foi utilizado Polly para fazer re-tentativas no request que obtém os pedidos da sua conta. Como é um request que precisa de autorização, caso o token já tenha expirado, a policy entra em ação e tenta fazer a autenticação novamente, utilizando o refreshToken obtido no login anterior.

## MemoryCache
Utilizado o cache provider do .Net framework core para salvar em cache o token ao fazer o login e não buscar o token em todo request que é feito a uma rota do iFood que precisa de autorização.

## Rotas e serviços
As rotas devem ser o mais enxuta possível, cuidando apenas de validar o input do usuario, chamar o método do Service responsável e retornar os dados esperados.
Exemplo de fluxo: Controller -> Service -> Banco de dados (se necessário) -> Retorna ao Controller -> Saida mostrada ao usuario
Os inputs são validados através de Attributos. (Required, EmailAddress, etc)

# Estrutura dos Projetos

## Financas.Core
Guarda as definições que podem sem compartilhadas com outros projetos. (Entidade base, repositório base, constantes de erro, etc)

## Financas.Data
Projeto que contém a implementação dos repositórios bem como as migrações geradas através do Entity Framework

## Financas.Domain
Contém os repositórios genéricos e as entidades do domínio

## Financas.Domain.Shared
Código compartilhado entre o domínio e outras camadas, indicado para Enums, constantes, etc.

## Financas.HttpPost
O projeto padrão de API do .Net Core

## Financas.IFood
O projeto que contém o serviço e o cliente que executa as chamadas para o iFood. O service chamado pelo controller esta aqui. Caso num futuro fosse adicionar outro serviço, como Uber, deverá ser criado outro projeto "Financas.Uber" contendo o serviço e o cliente que realiza as chamadas HTTP.


# Como utilizar

Altere no appsettings a conexão com seu banco de dados, após, crie uma migration e atualize seu banco de dados.

Utilize a primeira rota, "enviar-codigo-email" para receber no seu e-mail um código de verificação diretamente do iFood.

A rota deve retornar uma mensagem de sucesso e uma "key", que deve ser guardada para o segundo request.

Verifique seu email e pegue o código recebido.

Na segunda rota. "informar-codigo-recebido-email" passe no input a key recebida do método anterior junto com o código recebido via e-mail.

Caso o código informado esteja correto e válido, você receberá uma mensagem de sucesso e um "token". Guarde para o próximo request.

Na rota "completar-login", passe no input o email utilizado na primeira rota, junto com o token recebido da rota anterior. Após isso
você já deverá estar autenticado no iFood e um novo registro na tabela "AcessosIfood" deve ter sido criado, com seu email, token e refreshToken.

Após, inicie a sincronização dos seus pedidos, através da rota "sincronizar-pedidos". Use o email que você usou para completar o processo de autenticação.

Você pode ver o status do processo através do menu do hangfire.

Após a sincronização dos seus pedidos, chame a rota "obter-total-gasto" informando seu e-mail para saber qual foi o valor total que você já gastou no iFood.