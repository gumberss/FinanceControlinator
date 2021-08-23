#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Microservices/Invoices/Invoices.API/Invoices.API.csproj", "Microservices/Invoices/Invoices.API/"]
COPY ["Microservices/Invoices/Invoices.Domain/Invoices.Domain.csproj", "Microservices/Invoices/Invoices.Domain/"]
COPY ["Common/FinanceControlinator.Common/FinanceControlinator.Common.csproj", "FinanceControlinator.Common/"]
COPY ["Common/FinanceControlinator.Events/FinanceControlinator.Events.csproj", "FinanceControlinator.Events/"]
COPY ["Microservices/Invoices/Invoices.Data/Invoices.Data.csproj", "Microservices/Invoices/Invoices.Data/"]
COPY ["Microservices/Invoices/Invoices.Handler/Invoices.Handler.csproj", "Microservices/Invoices/Invoices.Handler/"]
COPY ["Microservices/Invoices/Invoices.Application/Invoices.Application.csproj", "Microservices/Invoices/Invoices.Application/"]
RUN dotnet restore "Microservices/Invoices/Invoices.API/Invoices.API.csproj"
COPY . .
WORKDIR "Microservices/Invoices/Invoices.API"
RUN dotnet build "Invoices.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Invoices.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invoices.API.dll"]