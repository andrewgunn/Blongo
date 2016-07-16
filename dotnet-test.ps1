New-Item -ItemType Directory -Force -Path artifacts\test\AkismetSdk.Tests
dotnet test (Resolve-Path test\AkismetSdk.Tests) --configuration Release --work (Resolve-Path artifacts\test\AkismetSdk.Tests)
New-Item -ItemType Directory -Force -Path artifacts\test\Blongo.Tests
dotnet test (Resolve-Path test\Blongo.Tests) --configuration Release --work (Resolve-Path artifacts\test\Blongo.Tests)