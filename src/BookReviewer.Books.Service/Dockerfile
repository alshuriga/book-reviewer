FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app


COPY ./BookReviewer.Books.Service/*.csproj ./BookReviewer.Books.Service/
COPY ./BookReviewer.Shared/*.csproj ./BookReviewer.Shared/

RUN dotnet restore BookReviewer.Books.Service

COPY . .
RUN dotnet publish BookReviewer.Books.Service -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR BookReviewer.Books.Service
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookReviewer.Books.Service.dll"]