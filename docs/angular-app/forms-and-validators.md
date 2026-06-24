# Forms and Validators

## Form Strategy

CRUD dialogs use Angular reactive forms with `FormBuilder`.

Typical create dialog pattern:

1. Define `protected readonly form = this.fb.group(...)`.
2. Apply validators from `shared/validators`.
3. Expose getter methods for each field error.
4. On confirm, clear `apiErrorMessage`.
5. If invalid, call `markAllAsTouched()` and stop.
6. Trim string values while building the request DTO.
7. Set `isSubmitting = true`.
8. Call the feature service.
9. Close the dialog with `true` on success.
10. Restore submit state and show API error on failure.

Update dialogs additionally compare current form values with the original input data before calling the API. If nothing changed, they display an error message instead of sending a request.

Delete dialogs do not use forms. They confirm the action, verify the record identifier, call the delete service method, and close with `true` on success.

## Shared Validator Helpers

Location:

```text
src/app/shared/validators/global-form.validators.ts
```

`GlobalFormValidators` provides:

- `notBlank()` - rejects values that contain only whitespace
- `getControlErrorMessage(control, label)` - maps common Angular validation errors to user-facing messages
- `getApiErrorMessage(error)` - extracts backend error details from `HttpErrorResponse`

Control error messages are only returned after a control is touched or dirty.

## Resource Validators

### PersonFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `pesel` | required, exactly 11 characters |
| `firstName` | required, 1-100 characters, not blank |
| `middleName` | max 100 characters |
| `lastName` | required, 1-100 characters, not blank |
| `gender` | required, one of `Male`, `Female`, `Other` |
| `birthDate` | required |
| `birthPlace` | required, 1-100 characters, not blank |
| `addressRegistryNumber` | max 50 characters |
| `documentName` | 1-100 characters |

Also provides `hasPersonChanges(...)`.

### AddressFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `registryNumber` | required, 1-50 characters, not blank |
| `city` | required, 1-100 characters, not blank |
| `street` | required, 1-100 characters, not blank |
| `houseNumber` | required, 1-20 characters, not blank |
| `apartmentNumber` | 1-20 characters |
| `postalCode` | required, 2-15 characters, not blank |
| `country` | required, 2-60 characters, not blank |
| `documentName` | 2-100 characters |

Also provides `hasAddressChanges(...)`.

### DocumentFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `name` | required, 2-100 characters, not blank |
| `category` | required, 2-100 characters, not blank |

Also provides `hasDocumentChanges(...)`.

### BirthRecordsFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `registryNumber` | required, 1-50 characters, not blank |
| `personPesel` | required, exactly 11 characters, not blank |
| `motherPesel` / `fatherPesel` | exactly 11 characters when provided |
| `documentName` | 2-100 characters |

Also provides `hasBirthRecordChanges(...)`.

### MarriageFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `registryNumber` | required, 1-50 characters, not blank |
| `spouse1Pesel` | required, exactly 11 characters |
| `spouse2Pesel` | required, exactly 11 characters |
| `marriageDate` | required |
| `marriagePlace` | required, 1-100 characters, not blank |
| `documentName` | 2-100 characters |

Special validator:

- `spousesDifferent()` is a group-level validator that rejects the form when both spouse PESEL values are equal.

Also provides `hasMarriageChanges(...)`.

### DeathRecordFormValidators

Rules:

| Field | Rules |
| --- | --- |
| `registryNumber` | required, 1-50 characters, not blank |
| `personPesel` | required, exactly 11 characters |
| `deathDate` | required |
| `deathPlace` | required, 1-100 characters, not blank |
| `registryDate` | required |
| `causeOfDeath` | required, max 200 characters, not blank |
| `documentName` | 2-100 characters |

Also provides `hasDeathRecordChanges(...)`.

## Dialog Error State

Dialog components use two common fields:

```ts
protected isSubmitting = false;
protected apiErrorMessage = '';
```

`isSubmitting` disables confirm buttons and blocks cancel while a request is in progress. `apiErrorMessage` is passed into `DialogShellComponent`, which renders it below the dialog body.

## Date Fields

Date fields use Angular Material datepicker with `provideNativeDateAdapter()` declared in dialog component providers. Request DTOs currently accept date values as defined by the local interfaces, so maintain the existing model type when adding new date fields.
