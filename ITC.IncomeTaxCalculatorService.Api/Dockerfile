#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ITC.IncomeTaxCalculatorService.Api/ITC.IncomeTaxCalculatorService.Api.csproj", "ITC.IncomeTaxCalculatorService.Api/"]
RUN dotnet restore "ITC.IncomeTaxCalculatorService.Api/ITC.IncomeTaxCalculatorService.Api.csproj"
COPY . .
WORKDIR "/src/ITC.IncomeTaxCalculatorService.Api"
RUN dotnet build "ITC.IncomeTaxCalculatorService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITC.IncomeTaxCalculatorService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITC.IncomeTaxCalculatorService.Api.dll"]