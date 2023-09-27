#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RePurpose/RePurpose.csproj", "RePurpose/"]
COPY ["RePurpose_Models/RePurpose_Models.csproj", "RePurpose_Models/"]
COPY ["RePurpose_Service/RePurpose_Service.csproj", "RePurpose_Service/"]
COPY ["RePurpose_Utility/RePurpose_Utility.csproj", "RePurpose_Utility/"]
RUN dotnet restore "RePurpose/RePurpose.csproj"
COPY . .
WORKDIR "/src/RePurpose"
RUN dotnet build "RePurpose.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RePurpose.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RePurpose.dll"]