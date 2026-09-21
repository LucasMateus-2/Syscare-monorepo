FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Prontuario.Domain/Prontuario.Domain.csproj", "src/Prontuario.Domain/"]
COPY ["src/Prontuario.Application/Prontuario.Application.csproj", "src/Prontuario.Application/"]
COPY ["src/Prontuario.Infrastructure/Prontuario.Infrastructure.csproj", "src/Prontuario.Infrastructure/"]
COPY ["src/Prontuario.API/Prontuario.API.csproj", "src/Prontuario.API/"]
RUN dotnet restore "src/Prontuario.API/Prontuario.API.csproj"

COPY . .
WORKDIR "/src/src/Prontuario.API"
RUN dotnet publish "Prontuario.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Prontuario.API.dll"]
