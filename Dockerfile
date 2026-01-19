# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["CSharpInterviewPrep.csproj", "./"]
RUN dotnet restore "CSharpInterviewPrep.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/."
RUN dotnet build "CSharpInterviewPrep.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CSharpInterviewPrep.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSharpInterviewPrep.dll"]