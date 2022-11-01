FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Common/Common.csproj", "Common/"]
COPY ["RoomBooking.BusinessLogic/RoomBooking.BusinessLogic.csproj", "RoomBooking.BusinessLogic/"]
COPY ["RoomBooking.BusinessLogic.Interfaces/RoomBooking.BusinessLogic.Interfaces.csproj", "RoomBooking.BusinessLogic.Interfaces/"]
COPY ["RoomBooking.DataAccess/RoomBooking.DataAccess.csproj", "RoomBooking.DataAccess/"]
COPY ["RoomBooking.DataAccess.Interfaces/RoomBooking.DataAccess.Interfaces.csproj", "RoomBooking.DataAccess.Interfaces/"]
COPY ["RoomBooking.Services/RoomBooking.Services.csproj", "RoomBooking.Services/"]
RUN dotnet restore "RoomBooking.Services/RoomBooking.Services.csproj"
COPY . .
WORKDIR "/src/RoomBooking.Services"
RUN dotnet build "RoomBooking.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RoomBooking.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "RoomBooking.Services.dll"]
