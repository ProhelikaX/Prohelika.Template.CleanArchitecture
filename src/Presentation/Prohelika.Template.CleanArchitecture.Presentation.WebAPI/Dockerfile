FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebAPI/Prohelika.Template.CleanArchitecture.Presentation.WebAPI.csproj", "src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebAPI/"]
RUN dotnet restore "src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebAPI/Prohelika.Template.CleanArchitecture.Presentation.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebAPI"
RUN dotnet build "Prohelika.Template.CleanArchitecture.Presentation.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Prohelika.Template.CleanArchitecture.Presentation.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prohelika.Template.CleanArchitecture.Presentation.WebAPI.dll"]
