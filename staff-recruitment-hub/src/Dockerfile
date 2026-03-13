# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_HTTP_PORTS=8000

# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy .csproj
COPY ["src/SRB_WebPortal/SRB_WebPortal.csproj", "src/SRB_WebPortal/"]
COPY ["src/SRB_ViewModel/SRB_ViewModel.csproj", "src/SRB_ViewModel/"]

RUN dotnet restore "src/SRB_WebPortal/SRB_WebPortal.csproj"

COPY . .
WORKDIR "/src/src/SRB_WebPortal"
RUN dotnet publish "SRB_WebPortal.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Binary to Image Runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SRB_WebPortal.dll"]
