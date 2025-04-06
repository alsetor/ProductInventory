# Build stage for Client (Blazor)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything
COPY . ./

# Restore and publish Client
WORKDIR /app/ProductInventory.Blazor
RUN dotnet publish -c Release -o ../published-client

# Restore and publish API
WORKDIR /app/ProductInventory.Api
RUN dotnet publish -c Release -o /app/publish

# Copy Blazor app into API wwwroot
RUN cp -r /app/published-client/wwwroot /app/publish/wwwroot

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "ProductInventory.Api.dll"]

