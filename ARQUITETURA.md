# Decisões de Arquitetura e Implementação

Este documento resume as principais escolhas feitas no desenvolvimento da aplicação (frontend + backend) e o porquê de cada decisão.

Objetivos
- Entregar um CRUD de heróis consistente, responsivo e acessível.
- Código simples de manter, com camadas claras e testes automatizados.

Frontend (React + Vite + TypeScript)
- Vite + TS: build rápido, DX excelente e tipagem de ponta a ponta.
- React 18: componentes funcionais com hooks; sem state manager global por simplicidade (estado local e navegação bastam para este escopo).
- Estilização com Tailwind: classes utilitárias garantem consistência visual (gradientes, blur, cores) e velocidade de entrega. Componentes base (Button, Card, Badge, Select, Label, Input) seguem o padrão do projeto.
- Roteamento (react-router): rotas declarativas para Home, Lista, Formulário (novo/edição) e NotFound.
- Formulários (react-hook-form): validação leve no cliente; conversão de vírgula para ponto em altura/peso (ex.: “1,80” → 1.8) e normalização de data para ISO.
- Camada de serviços (axios): `@services/*` concentra chamadas HTTP e tipos (`ApiResponse`, `HeroResponse`…), isolando o fetch da UI e facilitando testes.
- UX/Erros: toast simples para feedback imediato (erros de validação do backend e mensagens de falha genéricas). Exclusão otimista na lista para experiência fluida.
- Testes: Jest + Testing Library com mocks dos serviços; cobrimos lista, remoção e formulário (criação/edição/validação/voltar).

Backend (.NET + ASP.NET Core)
- Camadas Application/Domain/Infrastructure para separação de responsabilidades.
- Controllers REST (Heroes, Superpowers) com envelope `ApiResponse` unificado (code/message/result/errors).
- Swagger (Swashbuckle): documentação interativa em `/swagger` e exemplos de request/response.
- Migrations automáticas no startup; CORS liberado para facilitar o desenvolvimento local.

Trade-offs e racional
- Evitamos Redux/Zustand para não complexificar sem necessidade; o fluxo atual é simples.
- Mantivemos UI acessível (labels, `sr-only`, roles) e consistente com o tema do app.
- O toast é minimalista (sem dependências) para reduzir acoplamento; pode ser trocado por Sonner/Toastify facilmente.

Próximos passos (sugestões)
- Adotar Sonner/Toastify para toasts com mais recursos.
- Testes e2e (Playwright) para fluxos críticos.
- Paginação/consulta no backend quando a lista crescer.

