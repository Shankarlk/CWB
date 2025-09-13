# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# Use the ASP.NET Core runtime image for the final, lightweight container
# This image includes the Microsoft.AspNetCore.App framework
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

# Use the .NET SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CWB.Email/CWB.Email.csproj", "CWB.Email/"]
COPY ["CWB.Extensions/CWB.Extensions.csproj", "CWB.Extensions/"]
COPY ["CWB.Logging/CWB.Logging.csproj", "CWB.Logging/"]
COPY ["CWB.ExceptionHandling/CWB.ExceptionHandling.csproj", "CWB.ExceptionHandling/"]
COPY ["CWB.Constants/CWB.Constants.csproj", "CWB.Constants/"]
COPY ["CWB.Notification/CWB.Notification.csproj", "CWB.Notification/"]
COPY ["CWB.CommonUtils/CWB.CommonUtils.csproj", "CWB.CommonUtils/"]
RUN dotnet restore "CWB.Email/CWB.Email.csproj"
COPY . .
WORKDIR "/src/CWB.Email"
RUN dotnet build "CWB.Email.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CWB.Email.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CWB.Email.dll"]