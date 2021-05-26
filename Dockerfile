FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Mercury-Backend.csproj", "./"]
RUN dotnet restore "Mercury-Backend.csproj"
COPY . .
WORKDIR "/src/Mercury-Backend"
RUN dotnet build "Mercury-Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mercury-Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mercury-Backend.dll"]
