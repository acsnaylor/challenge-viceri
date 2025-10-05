Frontend React (Vite + TS)

Scripts
- dev: start dev server
- build: production build
- preview: preview production build

Env
- VITE_API_BASE_URL=http://localhost:5000

Estrutura
- src/app: App shell, rotas
- src/components: componentes reutilizáveis e layout
- src/pages: páginas (CRUD de heróis)
- src/services: client axios, tipos e chamadas
- src/styles: CSS global e utilitários

Rotas
- /heroes: lista de heróis
- /heroes/new: novo herói
- /heroes/:id: detalhes
- /heroes/:id/edit: editar

