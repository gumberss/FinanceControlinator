#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Microservices/Payments/Payments.API/Payments.API.csproj", "Microservices/Payments/Payments.API/"]
COPY ["Microservices/Payments/Payments.Domain/Payments.Domain.csproj", "Microservices/Payments/Payments.Domain/"]
COPY ["Common/FinanceControlinator.Common/FinanceControlinator.Common.csproj", "FinanceControlinator.Common/"]
COPY ["Common/FinanceControlinator.Events/FinanceControlinator.Events.csproj", "FinanceControlinator.Events/"]
COPY ["Microservices/Payments/Payments.Data/Payments.Data.csproj", "Microservices/Payments/Payments.Data/"]
COPY ["Microservices/Payments/Payments.Handler/Payments.Handler.csproj", "Microservices/Payments/Payments.Handler/"]
COPY ["Microservices/Payments/Payments.Application/Payments.Application.csproj", "Microservices/Payments/Payments.Application/"]
RUN dotnet restore "Microservices/Payments/Payments.API/Payments.API.csproj"
COPY . .
WORKDIR "Microservices/Payments/Payments.API"
RUN dotnet build "Payments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.API.dll"]