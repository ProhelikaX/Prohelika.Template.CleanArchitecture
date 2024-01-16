FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebCSR/Prohelika.Template.CleanArchitecture.Presentation.WebCSR.csproj", "src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebCSR/"]
RUN dotnet restore "src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebCSR/Prohelika.Template.CleanArchitecture.Presentation.WebCSR.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebCSR"
RUN dotnet build "Prohelika.Template.CleanArchitecture.Presentation.WebCSR.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Prohelika.Template.CleanArchitecture.Presentation.WebCSR.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY src/Presentation/Prohelika.Template.CleanArchitecture.Presentation.WebCSR/nginx.conf /etc/nginx/nginx.conf
CMD ["nginx", "-g", "daemon off;"]
EXPOSE 80
EXPOSE 443