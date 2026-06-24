# Angular Frontend Documentation

This directory documents the Angular frontend located in `apps/client/Dystopian-Civil-Office`.

The frontend is a standalone Angular application for the Dystopian Civil Office domain. It provides navigation, list pages, filtering, sorting, CRUD dialogs, validation, and typed communication with the backend API.

## Documentation Map

- [Frontend Overview](frontend-overview.md) - application purpose, stack, runtime shape, and high-level flow.
- [Architecture](architecture.md) - folder structure, Angular patterns, route shell, state flow, and module boundaries.
- [Pages and Routing](pages-and-routing.md) - route table, page responsibilities, sorting/filtering conventions, and user flows.
- [UI Components and Styling](ui-components-and-styling.md) - shared UI components, layout, Tailwind theme tokens, Angular Material usage, and assets.
- [Forms and Validators](forms-and-validators.md) - reactive form conventions, validator rules, error handling, and dialog submission behavior.
- [Services and API Models](services-and-api-models.md) - API configuration, services, caching, request/response models, and backend contract assumptions.
- [Frontend Conventions](frontend-conventions.md) - naming, implementation rules, adding new resources, quality checklist, and known improvement items.
- [Backend Integration](backend.md) - existing backend-facing notes.

## Quick Start

From `apps/client/Dystopian-Civil-Office`:

```bash
npm ci
npm start
```

The app expects the backend API at:

```text
http://localhost:5159/api
```

The base URL is currently defined in `src/app/core/config/api-main-url.ts`.

## Current Stack

- Angular `21.x`
- Angular Material `21.x`
- Angular SSR package and Express server support
- RxJS `7.8`
- Tailwind CSS `4.x`
- TypeScript `5.9`
- Vitest dependency present, with Angular test script still using `ng test`
