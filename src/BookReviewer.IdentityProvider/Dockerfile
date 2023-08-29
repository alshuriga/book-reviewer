FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app


COPY ./BookReviewer.IdentityProvider/*.csproj ./BookReviewer.IdentityProvider/

RUN dotnet restore BookReviewer.IdentityProvider

COPY . .
RUN dotnet publish BookReviewer.IdentityProvider -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR BookReviewer.IdentityProvider
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookReviewer.IdentityProvider.dll"]