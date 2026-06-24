# Frontend Conventions

## Naming

Current naming conventions:

- page components: `*.component.ts` inside `features/pages/<resource>-page.component/`
- dialogs: `create-*`, `update-*`, `delete-*` grouped by resource under `features/dialogs`
- services: plural resource name plus `.service.ts`
- request models: `CreateXModel`, `UpdateXModel`, `DeleteXModel`
- response models: `XViewModel`
- validators: `XFormValidators`

Prefer preserving these names when adding new resources.

## Angular Implementation Rules

Use standalone components for new UI.

Prefer `inject()` for service and Angular dependency access.

Use `protected` for component members referenced by templates.

Use `private readonly` for injected dependencies and implementation-only observables.

Use Angular control flow syntax already present in the app:

```html
@if (...) { ... }
@for (item of items; track item.id) { ... }
@empty { ... }
```

Use `AsyncPipe` for observable page data instead of manual page-level subscriptions, except for dialog `afterClosed()` and mutation submit handlers.

## Form Rules

Use reactive forms for create/update dialogs.

Centralize resource validation in `shared/validators`.

Expose field-specific error getters in the dialog component and render them with `mat-error`.

Trim string values before constructing request DTOs.

Update dialogs should check for meaningful changes before calling the API.

Disable confirm buttons while submitting.

Close dialogs with a boolean result:

- `true` after successful mutation
- `false` on cancel

## Service Rules

Feature services should:

- keep API paths in one `url` field
- return typed `Observable<T>`
- use `shareReplay` for cached list reads
- clear cache after successful create/update/delete
- provide explicit refresh methods for pages
- rethrow errors so pages/dialogs decide how to display them

## UI Rules

Use shared UI components first:

- `app-button` for standard buttons
- `app-table` for resource tables
- `app-dialog-shell` for dialogs

Use Tailwind theme tokens from `src/styles.css` before adding raw colors.

Open Material dialogs with:

```ts
{
  disableClose: true,
  panelClass: 'document-dialog-panel',
}
```

Use `assets/edit.png` and `assets/bin.png` for existing table row edit/delete actions unless the icon system is changed globally.

## Adding a New Resource Page

Recommended steps:

1. Add request and response interfaces under `shared/api-models`.
2. Add a resource service under `features/services`.
3. Add resource validators under `shared/validators`.
4. Add create, update, and delete dialogs under `features/dialogs/<resource>-dialogs`.
5. Add a page component under `features/pages/<resource>-page.component`.
6. Use `BehaviorSubject` for sort state and refresh triggers.
7. Render records with `app-table`.
8. Add the route in `app.routes.ts`.
9. Add the navigation item in `TopNavbarComponent` when it should be globally accessible.

## Quality Checklist

Before completing frontend changes:

- `npm run build` succeeds.
- Main route loads without injection errors.
- List page shows loading, success, empty, and error states.
- Create/update dialogs validate required and optional fields.
- Update dialog does not send unchanged data.
- Delete dialog handles missing IDs.
- Successful mutations refresh the current page list.
- Backend validation errors are visible in the dialog.
- Table sorting still works after refresh.

## Known Improvement Items

These are not blockers, but they are worth tracking:

- Root `app.config.ts` should be checked for `provideHttpClient()` consistency.
- `App` imports `TopNavbarComponent` twice.
- Some sort indicators appear mojibake-encoded in templates and should be normalized to ASCII or proper Unicode.
- `TableComponent` exposes `columns`, `data`, and `emptyMessage`, but current pages mainly use projection. Either formalize the projection-only approach or adopt the inputs.
- The navbar and main layout use large minimum widths, so responsive behavior is limited.
