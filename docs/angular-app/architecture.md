# Architecture

## Source Layout

Frontend source lives in:

```text
apps/client/Dystopian-Civil-Office/src/app
```

Important directories:

```text
app/
  core/
    config/
    layout/
  features/
    dialogs/
    pages/
    services/
  shared/
    api-models/
      requests/
      responses/
    ui/
    validators/
```

## Layer Responsibilities

`core`

Contains application-wide infrastructure:

- API base URL configuration
- top navigation
- footer

`features/pages`

Contains route-level screens. Resource pages own list loading, sorting, filters, error display, and opening CRUD dialogs.

`features/dialogs`

Contains create, update, and delete dialogs grouped by resource. Dialogs own forms, validation display, request object construction, submit state, and dialog close result.

`features/services`

Contains backend access services. Services wrap `HttpClient`, expose typed CRUD methods, cache read results with `shareReplay`, and clear caches after mutations.

`shared/api-models`

Contains TypeScript interfaces for request DTOs and response view models.

`shared/ui`

Contains reusable presentational components:

- `ButtonComponent`
- `TableComponent`
- `DialogShellComponent`

`shared/validators`

Contains static validator factories, shared error-message helpers, and update-change detection helpers.

## Component Pattern

The app uses standalone components rather than NgModules. Components declare their imports locally:

```ts
@Component({
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe],
})
```

Dependencies are usually resolved with Angular `inject()`:

```ts
private readonly dialog = inject(MatDialog);
private readonly personsService = inject(PersonsService);
```

Class members used by templates are generally marked `protected`, while implementation details are `private`.

## Data Flow

Typical resource page data flow:

1. Page owns one or more `BehaviorSubject`s for sort, filter, and refresh triggers.
2. Page calls a feature service from an RxJS pipeline.
3. Service calls the backend with `HttpClient`.
4. Service caches read results using `shareReplay({ bufferSize: 1, refCount: true })`.
5. Page maps records into a sorted array.
6. Template subscribes with `AsyncPipe`.
7. Dialog mutation calls service create/update/delete.
8. Service clears cache after successful mutation.
9. Page refresh trigger reloads current data.

## Routing Boundary

Routes are defined in `src/app/app.routes.ts`. Each route lazy-loads a standalone page component with `loadComponent`. This keeps page code split by route and avoids eager imports for every feature page.

## UI Boundary

Page templates define table columns and row markup directly, while `TableComponent` provides the shared outer table container. This gives each page freedom over resource-specific cells and actions while preserving consistent table framing.

Dialogs use `DialogShellComponent` for title, subtitle, body slot, action slot, and API error display. Forms inside dialogs use Angular Material controls.
