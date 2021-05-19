#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MCB.VBO.Microservices.Statements.StateService/MCB.VBO.Microservices.Statements.StateService.csproj", "MCB.VBO.Microservices.Statements.StateService/"]
RUN dotnet restore "MCB.VBO.Microservices.Statements.StateService/MCB.VBO.Microservices.Statements.StateService.csproj"
COPY . .
WORKDIR "/src/MCB.VBO.Microservices.Statements.StateService"
RUN dotnet build "MCB.VBO.Microservices.Statements.StateService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCB.VBO.Microservices.Statements.StateService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCB.VBO.Microservices.Statements.StateService.dll"]