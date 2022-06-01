#Intrudoção

Este projeto surgiu como uma ideia de fazer um gerenciador de gastos pessoais. Um sistema que poderia usar dados de diversos serviços para compor os gastos, como por exemplo: Uber, iFood, Rappi, etc.

Inicialmente decidi implementar apenas o iFood. Para isso analisei qual era o processo para fazer o login no iFood através do Google Chrome Developer tools.
Após analisar as requisições consegui implementar com sucesso um cliente que consiga fazer o login no ifood apenas usando o método de login via EMAIL. Não implementei os demais métodos de login para economia de tempo nessa POC.



#Tecnologias e conceitos aplicados:

##Injeção de Dependência

##Repositórios

##Domínios ricos
Evitei usar entidades anêmicas para constriuir as entidades do domínio. A manipulação das entidades é feita estritamente através de métodos criados na própria entidade.

##AutoMapper
Foi utilizado o auto mapper para facilitar a criação de objetos diferentes mas com características iguais.

##HangFire
Foi utilizado o hangfire para tarefas em background. Foi utilizado para obter todos os pedidos feitos na sua conta e popular o banco de dados. Como isso é um processo demorado, a chamada da rota não pode esperar até que isso aconteca. Então a rota retorna um sucesso "processo iniciado" enquando o job roda em background sincronizando os pedidos.

##Polly
Foi utilizado Polly para fazer re-tentativas no request que obtém os pedidos da sua conta. Como é um request que precisa de autorização, caso o token já tenha expirado, a policy entra em ação e tenta fazer a autenticação novamente, utilizando o refreshToken obtido no login anterior.

##MemoryCache
Utilizado o cache provider do .Net framework core para salvar em cache o token ao fazer o login e não buscar o token em todo request que é feito a uma rota do iFood que precisa de autorização.

##Rotas e serviços
As rotas devem ser o mais enxuta possível, cuidando apenas de validar o input do usuario, chamar o método do Service responsável e retornar os dados esperados.
Exemplo de fluxo: Controller -> Service -> Banco de dados (se necessário) -> Retorna ao Controller -> Saida mostrada ao usuario
Os inputs são validados através de Attributos. (Required, EmailAddress, etc)

#Estrutura dos Projetos

##Financas.Core
Guarda as definições que podem sem compartilhadas com outros projetos. (Entidade base, repositório base, constantes de erro, etc)

##Financas.Data
Projeto que contém a implementação dos repositórios bem como as migrações geradas através do Entity Framework

##Financas.Domain
Contém os repositórios genéricos e as entidades do domínio

##Financas.Domain.Shared
Código compartilhado entre o domínio e outras camadas, indicado para Enums, constantes, etc.

##Financas.HttpPost
O projeto padrão de API do .Net Core

##Financas.IFood
O projeto que contém o serviço e o cliente que executa as chamadas para o iFood. O service chamado pelo controller esta aqui. Caso num futuro fosse adicionar outro serviço, como Uber, deverá ser criado outro projeto "Financas.Uber" contendo o serviço e o cliente que realiza as chamadas HTTP.