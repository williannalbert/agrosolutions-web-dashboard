FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["AgroSolutions.Identity.Web.csproj", "./"]
RUN dotnet restore "AgroSolutions.Identity.Web.csproj"
COPY . .
RUN dotnet build "AgroSolutions.Identity.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgroSolutions.Identity.Web.csproj" -c Release -o /app/publish


FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .

COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80