FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR ProyectoAMY

EXPOSE 80
EXPOSE 8080

#COPY PROJECT FILES
COPY ./*.csproj ./
RUN dotnet restore

#COPY EVERYTHING ELSE
COPY . .
RUN dotnet publish -c Release -o out

#Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /ProyectoAMY
COPY --from=build /ProyectoAMY/out .
ENTRYPOINT ["dotnet", "ProyectoAMY.dll"]