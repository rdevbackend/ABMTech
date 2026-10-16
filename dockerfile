# Estágio de Compilação e Publicação Auto-Contida (.NET 10)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o código e publica como Auto-Contido (Self-Contained)
COPY . ./
RUN dotnet publish -c Release -r linux-x64 --self-contained true -o /app/out

# Estágio de Execução usando imagem base Linux Ubuntu
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

# Executa o executável gerado diretamente
ENTRYPOINT ["./ABMTech.Api"]