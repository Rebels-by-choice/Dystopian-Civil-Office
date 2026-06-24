# Services and API Models

## API Base URL

The API base URL is defined in:

```text
src/app/core/config/api-main-url.ts
```

Current value:

```ts
export const apiConfig = {
  baseUrl: 'http://localhost:5159/api',
};
```

All feature services build resource URLs from this value.

## Service Pattern

Feature services are provided in root:

```ts
@Injectable({
  providedIn: 'root',
})
```

Common service responsibilities:

- inject `HttpClient`
- define one resource URL
- expose typed `get`, `create`, `update`, `delete`, `refresh`, and `clearCache` methods
- cache list requests with `shareReplay`
- reverse list responses so newest records appear first
- clear cache after successful mutations with `tap(() => this.clearCache())`
- reset failed cached observables inside `catchError`

## Services

| Service | API resource | Main methods |
| --- | --- | --- |
| `PersonsService` | `/Person` | `getPersons`, `getPersonsByGender`, `createPerson`, `updatePerson`, `deletePerson`, refresh/clear cache |
| `DocumentsService` | `/Document` | `getDocuments`, `getDocumentsByCategory`, `createDocument`, `updateDocument`, `deleteDocument`, refresh/clear cache |
| `AddressesService` | `/Address` | `getAddresses`, `createAddress`, `updateAddress`, `deleteAddress`, refresh/clear cache |
| `BirthRecordsService` | `/BirthRecord` | `getBirthRecords`, `createBirthRecord`, `updateBirthRecord`, `deleteBirthRecord`, refresh/clear cache |
| `MarriagesService` | `/Marriage` | `getMarriages`, `createMarriage`, `updateMarriage`, `deleteMarriage`, refresh/clear cache |
| `DeathRecordsService` | `/DeathRecord` | `getDeathRecords`, `createDeathRecord`, `updateDeathRecord`, `deleteDeathRecord`, refresh/clear cache |
| `StatisticsService` | `/ApiStats` | `getStatistics` |

## Filtered Caches

`PersonsService` and `DocumentsService` keep separate caches for filtered lists:

- persons by gender
- documents by category

The cache key is the trimmed filter value. Refresh methods delete the relevant cached entry and then call the corresponding getter.

## Error Handling

Services rethrow HTTP errors with:

```ts
catchError((error) => throwError(() => error))
```

List pages convert these errors into page-level messages. Dialogs convert them into form/dialog-level messages through validator helper methods.

## Request Models

Request interfaces live in:

```text
src/app/shared/api-models/requests
```

Each main resource has create/update/delete interfaces:

- `person.model.ts`
- `address.model.ts`
- `birthRecord.model.ts`
- `deathRecord.model.ts`
- `marriage.model.ts`
- `document.model.ts`

Create and update dialogs should build request objects explicitly from form controls rather than passing raw form values. This keeps trimming, optional defaults, and field naming under control.

## Response View Models

Response interfaces live in:

```text
src/app/shared/api-models/responses
```

Current view models:

- `PersonViewModel`
- `AddressViewModel`
- `BirthRecordViewModel`
- `DeathRecordViewModel`
- `MarriageViewModel`
- `DocumentViewModel`
- `StatisticsViewModel`

Page templates should render from response view models, not request DTOs.

## Backend Contract Assumptions

The frontend currently assumes:

- list endpoints return arrays
- create/update/delete endpoints return `void`
- update and delete endpoints include the record ID in the URL
- filtered endpoints use query parameters, for example `gender` for persons
- backend errors may include `description`, `detail`, or `title`

Keep these assumptions aligned with backend changes.
