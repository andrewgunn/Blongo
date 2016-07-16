New-Item -ItemType Directory -Force -Path artifacts\nuget
dotnet pack (Resolve-Path src\AkismetSdk) --configuration Release --output (Resolve-Path artifacts\nuget)
dotnet pack (Resolve-Path src\RealFaviconGeneratorSdk) --configuration Release --output (Resolve-Path artifacts\nuget)
dotnet pack (Resolve-Path src\Blongo) --configuration Release --output (Resolve-Path artifacts\nuget)