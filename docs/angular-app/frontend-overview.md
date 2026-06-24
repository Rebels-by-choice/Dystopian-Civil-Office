# Frontend Overview

## Purpose

The Angular frontend is the browser client for Dystopian Civil Office. It exposes civil-office resources through a tabular administrative interface:

- persons
- birth records
- addresses
- marriages
- death records
- documents
- API statistics
- cases placeholder page

Most resource pages follow the same workflow:

1. Fetch records from the backend API through a resource service.
2. Render records in a shared table shell.
3. Allow client-side sorting.
4. Open Angular Material dialogs for create, update, and delete actions.
5. Refresh local service cache after successful mutations.

## Runtime Shape

The app is bootstrapped as a standalone Angular application. The root app renders:

- `TopNavbarComponent`
- `RouterOutlet`
- `FooterComponent`

Primary navigation is route-based and lazy-loads standalone page components from `src/app/app.routes.ts`.

## Main Technologies

The implementation uses:

- standalone Angular components
- Angular Router lazy `loadComponent`
- Angular reactive forms for dialogs
- template-driven `ngModel` only for simple page filters
- Angular Material dialog, form field, select, input, and datepicker controls
- RxJS `BehaviorSubject`, `combineLatest`, `switchMap`, `map`, `catchError`, `shareReplay`
- Tailwind CSS utility classes and theme tokens

## Backend Dependency

All API calls are made against:

```text
http://localhost:5159/api
```

The backend must be running for list pages and dialogs to work. Pages display a connection-specific error when Angular receives HTTP status `0`.

## Rendering and SSR

The package includes Angular SSR dependencies and server entry files:

- `src/server.ts`
- `src/main.server.ts`
- `src/app/app.routes.server.ts`
- `src/app/app.config.server.ts`

The browser config uses hydration with event replay in `src/app/app.config.ts`.

One important implementation note: there is also `src/app/core/config/api.config.ts`, which provides `provideHttpClient()`. The root `src/app/app.config.ts` currently provides router and hydration but does not include `provideHttpClient()`. If runtime HTTP injection fails, consolidate these configs or add `provideHttpClient()` to the root browser app config.
