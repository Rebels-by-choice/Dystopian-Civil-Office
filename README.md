# Dystopian Civil Office
Dystopian Civil Office provides our idea for anarchic world, but with simular functionalities to real civil office in Poland. The whole application infrastructure is divided to multiple instances. 

- The backend section has ASP.NET main application for API endpoint execution and Postgres database for storing data. Additionally there is a  microservice for documents managment imported from open-source paperless: https://docs.paperless-ngx.com.
- Frontend layer is written in Angular with Tailwind and basic CSS stylesheet.

# Main features
- DML operations on persons.
- DML operations on birth_records.
- DML operations on death_records.
- DML operations on documents.
- DML operations on marrage_records.
- DML operations on addresses.
- Filtering and sorting mechanisms.
- Storing archive data.
- Analizing quick Create, Update and Delete statistics.

# Requirements
- Unix or Windows operating system.
- System with docker containers support.
- Node.JS minimum 14.0.0 ver.
- .NET 10+.

You can download required runtime environments here:
- Node.JS: https://nodejs.org/en/download/current.
- .NET SDK: https://dotnet.microsoft.com/en-us/download.

# How to use?
We implemented production and developer versions of software. You can start them manually, or by our provided scripts. Provided scripts are compatible with windows and unix operating systems.

## Production
Checkout our releases for clear archives, or clone the repository. After successfull run, open browser and go to http://localhost:4200.

### Automated usage
> 1. Go to `/scripts/Production`.
> 2. Run `/Windows/Run.ps1` or `/Unix/Run.sh`.

Those scripts will build run production containers. Check `scripts/Production/README.md` for more information.

### Manual usage
> 1. In the repo root run `docker compose --profile production up -d --build`.

## Developer
After successfull installation, build and run, check backend SwaggerUI documentation: http://localhost:5159/swagger/index.html. Check application main functionalities on Angular interface: http://localhost:4200.

### Automated usage
> 1. Go to `/scripts/Developer`.
> 2. Run `/Windows/Run.ps1` or `/Unix/Run.sh`.

Those scripts will build run required containers. Check `scripts/Developer/README.md` for more information.

### Manual usage
> 1. In the repo root run `docker compose up -d --build`.
> 2. Go to `apps/server/src/Dystopian-Civil-Office/Dystopian-Civil-Office`.
> 3. Update existing migrations to database, run: `dotnet ef database update`.
> 4. Run dotnet application: `dotnet run` or for hot-reload: `dotnet watch run`.
> 5. Navigate now to `apps/client/Dystopian-Civil-Office`.
> 6. Install dependencies: `npm ci`.
> 7. Run Angular app: `ng serve`.
