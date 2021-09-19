#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Microservices/PiggyBanks/PiggyBanks.API/PiggyBanks.API.csproj", "Microservices/PiggyBanks/PiggyBanks.API/"]
COPY ["Microservices/PiggyBanks/PiggyBanks.Domain/PiggyBanks.Domain.csproj", "Microservices/PiggyBanks/PiggyBanks.Domain/"]
COPY ["Common/FinanceControlinator.Common/FinanceControlinator.Common.csproj", "FinanceControlinator.Common/"]
COPY ["Common/FinanceControlinator.Events/FinanceControlinator.Events.csproj", "FinanceControlinator.Events/"]
COPY ["Microservices/PiggyBanks/PiggyBanks.Data/PiggyBanks.Data.csproj", "Microservices/PiggyBanks/PiggyBanks.Data/"]
COPY ["Microservices/PiggyBanks/PiggyBanks.Handler/PiggyBanks.Handler.csproj", "Microservices/PiggyBanks/PiggyBanks.Handler/"]
COPY ["Microservices/PiggyBanks/PiggyBanks.Application/PiggyBanks.Application.csproj", "Microservices/PiggyBanks/PiggyBanks.Application/"]
RUN dotnet restore "Microservices/PiggyBanks/PiggyBanks.API/PiggyBanks.API.csproj"
COPY . .
WORKDIR "Microservices/PiggyBanks/PiggyBanks.API"
RUN dotnet build "PiggyBanks.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PiggyBanks.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PiggyBanks.API.dll"]