#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MCB.VBO.Microservices.Statements/MCB.VBO.Microservices.Statements.csproj", "MCB.VBO.Microservices.Statements/"]
COPY ["MCB.VBO.Microservices.Statements.Shared/MCB.VBO.Microservices.Statements.Shared.csproj", "MCB.VBO.Microservices.Statements.Shared/"]
COPY ["MCB.VBO.TemplatesLib/MCB.VBO.TemplatesLib.csproj", "MCB.VBO.TemplatesLib/"]
RUN dotnet restore "MCB.VBO.Microservices.Statements/MCB.VBO.Microservices.Statements.csproj"
COPY . .
WORKDIR "/src/MCB.VBO.Microservices.Statements"
RUN dotnet build "MCB.VBO.Microservices.Statements.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCB.VBO.Microservices.Statements.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCB.VBO.Microservices.Statements.dll"]
