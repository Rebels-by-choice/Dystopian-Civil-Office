# Backend integration (Angular client)

This document describes how the Angular client calls the backend API and where to change the API base URL.

**API Base URL**: The client uses a central config with the base URL. See [src/app/core/config/api-main-url.ts](apps/client/Dystopian-Civil-Office/src/app/core/config/api-main-url.ts) (value used during development):

- `baseUrl: 'http://localhost:5159/api'`

Change this value to point the client to a different backend host/port.

**Key services and endpoints**

- **Persons service**: [src/app/features/services/persons.service.ts](apps/client/Dystopian-Civil-Office/src/app/features/services/persons.service.ts)
  - Base URL: `${apiConfig.baseUrl}/Person`
  - Methods: `GET /Person`, `GET /Person?gender=...`, `POST /Person`, `PUT /Person/{personId}`, `DELETE /Person/{personId}`

- **Marriages service**: [src/app/features/services/marriages.service.ts](apps/client/Dystopian-Civil-Office/src/app/features/services/marriages.service.ts)
  - Base URL: `${apiConfig.baseUrl}/Marriage`
  - Methods: `GET /Marriage`, `POST /Marriage`, `PUT /Marriage/{marriageRecordId}`, `DELETE /Marriage/{marriageRecordId}`

- **Documents service**: [src/app/features/services/documents.service.ts](apps/client/Dystopian-Civil-Office/src/app/features/services/documents.service.ts)
  - Base URL: `${apiConfig.baseUrl}/Document`
  - Methods: `GET /Document`, `GET /Document?category=...`, `POST /Document`, `PUT /Document/{documentId}`, `DELETE /Document/{documentId}`

- **Statistics / ApiStats service**: [src/app/features/services/statistics.service.ts](apps/client/Dystopian-Civil-Office/src/app/features/services/statistics.service.ts)
  - Base URL: `${apiConfig.baseUrl}/ApiStats`
  - Methods: `GET /ApiStats`

**HttpClient registration**

The app provides `HttpClient` globally in [src/app/core/config/api.config.ts](apps/client/Dystopian-Civil-Office/src/app/core/config/api.config.ts).

**CORS / Local development note**

The backend allows CORS for `http://localhost:4200` and `http://127.0.0.1:4200`. If you host the frontend on a different origin, update the backend CORS policy in `Program.cs`.

**Quick checklist to point the client to another backend**

1. Update `baseUrl` in [src/app/core/config/api-main-url.ts](apps/client/Dystopian-Civil-Office/src/app/core/config/api-main-url.ts).
2. Ensure backend CORS includes the client origin (see `Program.cs`).
3. Restart the frontend dev server.
