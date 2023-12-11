#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
# 暴露80端口
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/efcore/Z.EntityFrameworkCore.SqlServer/Z.EntityFrameworkCore.SqlServer.csproj", "src/efcore/Z.EntityFrameworkCore.SqlServer/"]
COPY ["src/efcore/Z.EntityFrameworkCore/Z.EntityFrameworkCore.csproj", "src/efcore/Z.EntityFrameworkCore/"]
COPY ["Z.SunBlog.EntityFrameworkCore/Z.SunBlog.EntityFrameworkCore.csproj", "Z.SunBlog.EntityFrameworkCore/"]
COPY ["Z.SunBlog.Host/Z.SunBlog.Host.csproj", "Z.SunBlog.Host/"]
COPY ["Z.SunBlog.Application/Z.SunBlog.Application.csproj", "Z.SunBlog.Application/"]
COPY ["src/Z.Fantasy.Application/Z.Fantasy.Application.csproj", "src/Z.Fantasy.Application/"]
COPY ["src/Z.Fantasy.Core/Z.Fantasy.Core.csproj", "src/Z.Fantasy.Core/"]
COPY ["src/Z.Module/Z.Module.csproj", "src/Z.Module/"]
COPY ["Z.SunBlog.Core/Z.SunBlog.Core.csproj", "Z.SunBlog.Core/"]
COPY ["Z.SunBlog.Common/Z.SunBlog.Common.csproj", "Z.SunBlog.Common/"]

RUN dotnet restore "Z.SunBlog.Host/Z.SunBlog.Host.csproj"
COPY . .
WORKDIR "/src/Z.SunBlog.Host"

RUN dotnet build "Z.SunBlog.Host.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Z.SunBlog.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#安装fontconfig
RUN apt-get clean
RUN apt-get update && apt-get install -y fontconfig

ENTRYPOINT ["dotnet", "Z.SunBlog.Host.dll"]