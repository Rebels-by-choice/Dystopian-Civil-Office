FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Dystopian-Civil-Office/Dystopian-Civil-Office.csproj", "Dystopian-Civil-Office/"]
RUN dotnet restore "Dystopian-Civil-Office/Dystopian-Civil-Office.csproj"
COPY . .
WORKDIR "/src/Dystopian-Civil-Office"
RUN dotnet build "./Dystopian-Civil-Office.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Dystopian-Civil-Office.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dystopian-Civil-Office.dll"]
