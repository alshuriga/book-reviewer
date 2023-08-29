FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app


COPY ./BookReviewer.Reviews.Service/*.csproj ./BookReviewer.Reviews.Service/
COPY ./BookReviewer.Shared/*.csproj ./BookReviewer.Shared/

RUN dotnet restore BookReviewer.Reviews.Service

COPY . .
RUN dotnet publish BookReviewer.Reviews.Service -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR BookReviewer.Books.Service
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookReviewer.Reviews.Service.dll"]