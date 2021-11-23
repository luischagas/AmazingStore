#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AmazingStore.Api/AmazingStore.Api.csproj", "AmazingStore.Api/"]
COPY ["AmazingStore.Infra.CrossCutting.AspNetFilters/AmazingStore.Infra.CrossCutting.AspNetFilters.csproj", "AmazingStore.Infra.CrossCutting.AspNetFilters/"]
COPY ["AmazingStore.Application/AmazingStore.Application.csproj", "AmazingStore.Application/"]
COPY ["AmazingStore.Domain/AmazingStore.Domain.csproj", "AmazingStore.Domain/"]
COPY ["AmazingStore.Domain.Shared/AmazingStore.Domain.Shared.csproj", "AmazingStore.Domain.Shared/"]
COPY ["AmazingStore.Infra.Data/AmazingStore.Infra.Data.csproj", "AmazingStore.Infra.Data/"]
COPY ["AmazingStore.Infra.CrossCutting.Services/AmazingStore.Infra.CrossCutting.Services.csproj", "AmazingStore.Infra.CrossCutting.Services/"]
COPY ["AmazingStore.Infra.CrossCutting.IoC/AmazingStore.Infra.CrossCutting.IoC.csproj", "AmazingStore.Infra.CrossCutting.IoC/"]
RUN dotnet restore "AmazingStore.Api/AmazingStore.Api.csproj"
COPY . .
WORKDIR "/src/AmazingStore.Api"
RUN dotnet build "AmazingStore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmazingStore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmazingStore.Api.dll"]
