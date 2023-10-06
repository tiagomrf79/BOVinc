#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#base = dotnet/<runtime>
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#build = dotnet/sdk + /src + /app/build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FarmsAPI/FarmsAPI.csproj", "FarmsAPI/"]
RUN dotnet restore "FarmsAPI/FarmsAPI.csproj"
COPY . .
WORKDIR "/src/FarmsAPI"
RUN dotnet build "FarmsAPI.csproj" -c Release -o /app/build

#publish = build + app/publish
FROM build AS publish
RUN dotnet publish "FarmsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

#final = base + /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FarmsAPI.dll"]