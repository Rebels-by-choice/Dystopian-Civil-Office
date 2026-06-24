# Pages and Routing

## Route Table

Routes are configured in `src/app/app.routes.ts`.

| Path | Component | Responsibility |
| --- | --- | --- |
| `/home` | `HomePageComponent` | Landing/home content. |
| `/persons` | `PersonsPageComponent` | Person list, gender filter, sorting, CRUD dialogs. |
| `/births` | `BirthsPageComponent` | Birth record list, sorting, CRUD dialogs. |
| `/addresses` | `AddressesPageComponent` | Address list, sorting, CRUD dialogs. |
| `/marriages` | `MarriagesPageComponent` | Marriage record list, sorting, CRUD dialogs. |
| `/deaths` | `DeathsPageComponent` | Death record list, sorting, CRUD dialogs. |
| `/documents` | `DocumentsPageComponent` | Document list, category filter, sorting, CRUD dialogs. |
| `/statistics` | `StatisticsPageComponent` | API statistics summary grouped by operation type. |
| `/cases` | `CasesPageComponent` | Cases placeholder or future feature page. |
| `/` | redirect | Redirects to `/home`. |
| `**` | redirect | Redirects unknown URLs to `/home`. |

## Page Conventions

Resource pages share a consistent structure:

- page title and primary create button
- optional filter form
- async list block using `@if (records$ | async; as records)`
- error state block when page-level API error exists
- `app-table` with projected `thead` and `tbody`
- `@for` row rendering with stable resource ID tracking
- `@empty` row for empty list state
- edit and delete icon buttons per row

## Sorting

Most resource pages use:

- a local `SortColumn` union type
- a `SortDirection` union type of `'asc' | 'desc'`
- a `SortState` interface
- `BehaviorSubject<SortState>`
- `sortBy(column)` to toggle direction or change active column
- `getSortDirection(column)` for template indicators
- a private sort function that copies arrays before sorting

Sorting is client-side. Date fields are converted to timestamps, and string values are normalized by trimming and lowercasing.

## Filters

Two pages currently expose filters:

- persons: gender filter with options `Default`, `Male`, `Female`, `Other`
- documents: category filter

Filters use `FormsModule` and `ngModel` because they are simple page-level controls, not full dialog forms. The selected value is normalized before being pushed into a filter `BehaviorSubject`.

## Dialog Workflow

Resource pages open dialogs through Angular Material:

```ts
this.dialog.open(CreatePersonDialogComponent, {
  disableClose: true,
  panelClass: 'document-dialog-panel',
});
```

Dialog close values are boolean:

- `true` means the mutation succeeded and the page should refresh
- `false` means no page refresh is needed

After a successful create, update, or delete, the page calls its reload method, usually by incrementing a refresh trigger.

## Statistics Page

`StatisticsPageComponent` reads API statistics and groups entries into:

- new records: `POST` with status code `201`
- edited records: update-like methods/statuses defined in the page logic
- deleted records: delete-like methods/statuses defined in the page logic

This page is read-only and does not use CRUD dialogs.
