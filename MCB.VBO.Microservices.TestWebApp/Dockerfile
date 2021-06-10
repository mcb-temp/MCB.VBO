#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["../MCB.VBO.Microservices.TestWebApp/MCB.VBO.Microservices.TestWebApp.csproj", "../MCB.VBO.Microservices.TestWebApp/"]
RUN dotnet restore "../MCB.VBO.Microservices.TestWebApp/MCB.VBO.Microservices.TestWebApp.csproj"
COPY . .
WORKDIR "/src/../MCB.VBO.Microservices.TestWebApp"
RUN dotnet build "MCB.VBO.Microservices.TestWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCB.VBO.Microservices.TestWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCB.VBO.Microservices.TestWebApp.dll"]