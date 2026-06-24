# UI Components and Styling

## Shared Components

### ButtonComponent

Location:

```text
src/app/shared/ui/button.component/
```

Selector:

```html
<app-button></app-button>
```

Inputs:

| Input | Type | Purpose |
| --- | --- | --- |
| `label` | `string` | Visible button text. |
| `variant` | `'primary' \| 'secondary'` | Selects main or secondary color treatment. |
| `disabled` | `boolean` | Blocks click handling and applies disabled styling. |
| `customClass` | `string` | Allows page-specific utility overrides. |

Output:

| Output | Purpose |
| --- | --- |
| `buttonClick` | Emits only when the button is not disabled. |

Usage convention:

- Use `primary` for confirm/create/action buttons.
- Use `secondary` for cancel/destructive-adjacent controls.
- Use `customClass` for filter/reset variants already represented by theme tokens.

### TableComponent

Location:

```text
src/app/shared/ui/table/
```

Selector:

```html
<app-table></app-table>
```

The component provides the outer table container and projects caller-defined header/body content:

```html
<app-table>
  <thead table-head>...</thead>
  <tbody table-body>...</tbody>
</app-table>
```

Inputs `columns`, `data`, and `emptyMessage` exist but current page templates mostly define headers and rows manually through content projection.

### DialogShellComponent

Location:

```text
src/app/shared/ui/dialog-shell/
```

Selector:

```html
<app-dialog-shell></app-dialog-shell>
```

Inputs:

| Input | Purpose |
| --- | --- |
| `title` | Dialog heading. |
| `subtitle` | Optional supporting text. |
| `errorMessage` | API or submission error displayed below body content. |
| `containerClass` | Tailwind class override for shell sizing and color. |

Projected slots:

```html
<div dialog-body>...</div>
<div dialog-actions>...</div>
```

## Layout Components

### TopNavbarComponent

Location:

```text
src/app/core/layout/top-navbar.component/
```

The navbar renders:

- logo link to `/home`
- main resource links
- active route styling through `RouterLinkActive`
- dedicated `Cases` link on the right

The header has a minimum width of `1100px`, so the current UI is optimized for desktop/tablet administrative layouts rather than narrow mobile use.

### FooterComponent

Location:

```text
src/app/core/layout/footer.component/
```

Provides the global footer rendered by `App`.

## Styling System

Global styles live in:

```text
src/styles.css
```

The project uses Tailwind CSS 4 with custom theme tokens under `@theme`. Token groups include:

- text colors: `dco-text-*`
- navbar backgrounds: `dco-bg-navbar*`
- button backgrounds: `dco-bg-button-*`
- table backgrounds: `dco-bg-table`, `dco-bg-thead`
- dialog/error colors: `dco-dialog-*`, `dco-bg-error`

Use these tokens before introducing new hard-coded colors.

## Typography

The app defines a local `Roboto Slab` font face from:

```text
src/assets/fonts/Roboto_Slab/
```

The global `html, body` font family is set to `Roboto Slab`.

## Angular Material Overrides

Global CSS overrides Angular Material dialog, select, option, overlay, and datepicker styles. Important selectors include:

- `.document-dialog-panel .mat-mdc-dialog-container .mdc-dialog__surface`
- `.mat-mdc-select-panel`
- `.mat-mdc-option`
- `.mat-datepicker-content`
- `.cdk-overlay-panel`

Dialogs opened from pages use `panelClass: 'document-dialog-panel'` to receive the custom dark dialog surface.

## Assets

Current shared visual assets:

- `src/assets/logo.svg`
- `src/assets/favicon.png`
- `src/assets/edit.png`
- `src/assets/bin.png`

Edit/delete row actions use the PNG assets with accessible `aria-label` text on the button.
