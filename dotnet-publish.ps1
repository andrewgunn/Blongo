gulp build --production
New-Item -ItemType Directory -Force -Path artifacts\publish
dotnet publish (Resolve-Path .\src\Blongo) --configuration Release --output (Resolve-Path artifacts\publish)