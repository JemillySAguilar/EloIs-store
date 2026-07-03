# Arquitetura - EloÍs Store

## Decisão atual

A EloÍs Store começa com uma arquitetura simples em .NET 10:

- Uma única API ASP.NET Core.
- Organização por `Controllers`, `Services`, `Models`, `Repositories` e `Data`.
- OpenAPI nativo do ASP.NET Core.
- Scalar como interface de documentação da API.
- PostgreSQL como banco de dados local via Docker Compose.

## Serviços

- Auth: cadastro, login e token simples de desenvolvimento.
- Catalog: produtos, categorias, variações, preço e estoque.
- Cart: carrinho por usuário.
- Orders: pedidos e checkout.
- Payments: pagamento mock configurável.
- Notifications: simulação de envio de e-mail.
- Health: endpoints de saúde da aplicação.

## Observação

Esta etapa prioriza uma fundação didática e fácil de evoluir. Regras mais avançadas como JWT real, migrations formais, testes com banco real e checkout transacional ficam para os próximos passos.
