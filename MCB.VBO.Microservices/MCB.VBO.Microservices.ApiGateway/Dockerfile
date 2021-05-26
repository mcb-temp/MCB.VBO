#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:5555

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["../MCB.VBO.Microservices.ApiGateway/MCB.VBO.Microservices.ApiGateway.csproj", "../MCB.VBO.Microservices.ApiGateway/"]
RUN dotnet restore "../MCB.VBO.Microservices.ApiGateway/MCB.VBO.Microservices.ApiGateway.csproj"
COPY . .
WORKDIR "/src/../MCB.VBO.Microservices.ApiGateway"
RUN dotnet build "MCB.VBO.Microservices.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCB.VBO.Microservices.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCB.VBO.Microservices.ApiGateway.dll"]