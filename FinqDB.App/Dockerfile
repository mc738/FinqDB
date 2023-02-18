FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinqDB.App/FinqDB.App.fsproj", "FinqDB.App/"]
RUN dotnet restore "FinqDB.App/FinqDB.App.fsproj"
COPY . .
WORKDIR "/src/FinqDB.App"
RUN dotnet build "FinqDB.App.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinqDB.App.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinqDB.App.dll"]
