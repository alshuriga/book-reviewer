FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app


COPY ./BookReviewer.ApiGateway/*.csproj ./BookReviewer.ApiGateway/

RUN dotnet restore BookReviewer.ApiGateway

COPY . .
RUN dotnet publish BookReviewer.ApiGateway -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR BookReviewer.ApiGateway
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookReviewer.ApiGateway.dll"]