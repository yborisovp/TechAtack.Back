FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 2007
COPY . .

RUN dotnet restore "Host/Host.csproj"
RUN dotnet restore "OggettoCase.DataAccess/OggettoCase.DataAccess.csproj"
RUN dotnet restore "OggettoCase.DataContracts/OggettoCase.DataContracts.csproj"

RUN dotnet publish "Host/Host.csproj" -c Release -o /src/published-app


FROM mcr.microsoft.com/dotnet/sdk:8.0 as runtime
WORKDIR /src
COPY --from=build /src/published-app /src 

ENTRYPOINT [ "dotnet", "/src/Host.dll" ]

