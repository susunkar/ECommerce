#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ECommerce.Api.Products/ECommerce.Api.Products.csproj", "ECommerce.Api.Products/"]
RUN dotnet restore "ECommerce.Api.Products/ECommerce.Api.Products.csproj"
COPY . .
WORKDIR "/src/ECommerce.Api.Products"
RUN dotnet build "ECommerce.Api.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.Api.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Api.Products.dll"]
