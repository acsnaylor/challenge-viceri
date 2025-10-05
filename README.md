# ChallengeViceri API

API REST para cadastro de super-heróis e seus superpoderes.

Base URL
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

Documentação interativa (Swagger)
- Navegue até: `http://localhost:5000/swagger` (ou `https://localhost:5001/swagger`)
- Contém schemas, exemplos e filtros de pesquisa.

Envelope de resposta
Todas as respostas seguem o formato:

```
{
  "code": number,
  "message": string,
  "result": T | null,
  "errors": { [key: string]: string } | null
}
```

Recursos

- Heróis
  - `GET /api/heroes` — Lista heróis
    - 200 OK: `result` = `HeroResponse[]`
    - 404 Not Found: `result` = `null`
  - `GET /api/heroes/{id}` — Busca por Id
    - 200 OK: `result` = `HeroResponse`
    - 400 Bad Request | 404 Not Found
  - `POST /api/heroes` — Cria herói
    - Body (HeroRequest):
      ```json
      {
        "name": "Bruce Wayne",
        "heroName": "Batman",
        "birthDate": "1972-02-19T00:00:00Z",
        "height": 1.88,
        "weight": 95,
        "superpowerIds": [1,2]
      }
      ```
    - 201 Created: `result` = `HeroResponse`
    - 400 Bad Request: erros de validação em `errors`
  - `PUT /api/heroes/{id}` — Atualiza herói
    - Body (HeroRequest) igual ao POST
    - 200 OK | 400 Bad Request | 404 Not Found
  - `DELETE /api/heroes/{id}` — Remove herói
    - 200 OK: `result` = string com mensagem
    - 400 Bad Request | 404 Not Found

- Superpoderes
  - `GET /api/superpowers` — Lista superpoderes
    - 200 OK: `result` = `SuperpowerResponse[]`
    - 404 Not Found

Modelos
- `HeroResponse`:
  ```json
  {
    "id": 1,
    "name": "Bruce Wayne",
    "heroName": "Batman",
    "birthDate": "1972-02-19T00:00:00Z",
    "height": 1.88,
    "weight": 95,
    "superpowers": [{ "id": 1, "name": "Força" }]
  }
  ```
- `HeroRequest`: ver corpo do POST/PUT
- `SuperpowerResponse`:
  ```json
  { "id": 1, "name": "Força", "description": null }
  ```

Exemplo de erro de validação (400)
```json
{
  "code": 400,
  "message": "Invalid request.",
  "result": null,
  "errors": {
    "Validation": "Já existe um super-herói com esse nome de herói."
  }
}
```

Observações
- CORS liberado no Startup para facilitar o front local.
- Migrations aplicadas no startup (`Database.Migrate()`).
- Exemplos de Swagger habilitados (Swashbuckle + Filters).

