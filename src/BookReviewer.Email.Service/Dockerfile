FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app


COPY ./BookReviewer.Email.Service/*.csproj ./BookReviewer.Email.Service/
COPY ./BookReviewer.Shared/*.csproj ./BookReviewer.Shared/

RUN dotnet restore BookReviewer.Email.Service

COPY . .
RUN dotnet publish BookReviewer.Email.Service -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR BookReviewer.Email.Service
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookReviewer.Email.Service.dll"]