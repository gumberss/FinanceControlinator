#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Microservices/Accounts/Accounts.API/Accounts.API.csproj", "Microservices/Accounts/Accounts.API/"]
COPY ["Microservices/Accounts/Accounts.Domain/Accounts.Domain.csproj", "Microservices/Accounts/Accounts.Domain/"]
COPY ["Common/FinanceControlinator.Common/FinanceControlinator.Common.csproj", "FinanceControlinator.Common/"]
COPY ["Common/FinanceControlinator.Events/FinanceControlinator.Events.csproj", "FinanceControlinator.Events/"]
COPY ["Microservices/Accounts/Accounts.Data/Accounts.Data.csproj", "Microservices/Accounts/Accounts.Data/"]
COPY ["Microservices/Accounts/Accounts.Handler/Accounts.Handler.csproj", "Microservices/Accounts/Accounts.Handler/"]
COPY ["Microservices/Accounts/Accounts.Application/Accounts.Application.csproj", "Microservices/Accounts/Accounts.Application/"]
RUN dotnet restore "Microservices/Accounts/Accounts.API/Accounts.API.csproj"
COPY . .
WORKDIR "Microservices/Accounts/Accounts.API"
RUN dotnet build "Accounts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accounts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounts.API.dll"]