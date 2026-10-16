# Estágio de Compilação (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os ficheiros e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código e compila
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Estágio de Execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Define a porta padrão
EXPOSE 8080

ENTRYPOINT ["dotnet", "ABMTech.dll"]