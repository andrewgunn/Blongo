$packPath = "artifacts\nuget"

If (Test-Path $packPath)
{
	Remove-Item $packPath -ErrorAction Ignore -Recurse
}

New-Item -ItemType Directory -Force -Path $packPath
dotnet pack (Resolve-Path src\AkismetSdk) --configuration Release --output (Resolve-Path $packPath)
dotnet pack (Resolve-Path src\RealFaviconGeneratorSdk) --configuration Release --output (Resolve-Path $packPath)
dotnet pack (Resolve-Path src\Blongo) --configuration Release --output (Resolve-Path $packPath)