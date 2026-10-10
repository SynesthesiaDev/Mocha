FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV DOTNET_GCLatencyMode=0
ENV DOTNET_GCHeapHardLimitPercent=70
ENV DOTNET_GCConserveMemory=9

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["Mocha/Mocha.csproj", "Mocha/"]

RUN case "$TARGETARCH" in \
      amd64) echo x64 > /tmp/arch ;; \
      arm64) echo arm64 > /tmp/arch ;; \
      *) echo "unsupported: $TARGETARCH" && exit 1 ;; \
    esac \
 && dotnet restore "Mocha/Mocha.csproj" -a "$(cat /tmp/arch)"
COPY . .
WORKDIR "/src/Mocha"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mocha.csproj" \
    -c $BUILD_CONFIGURATION \
    -a "$(cat /tmp/arch)" \
    -o /app/publish \
    --no-restore \
    --self-contained false \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
RUN mkdir -p /app/data && chown -R 1654:1654 /app/data
ENV DOTNET_RUNNING_IN_CONTAINER=true
USER root
ENTRYPOINT ["dotnet", "Mocha.dll", "--urls", "http://0.0.0.0:8080"]
