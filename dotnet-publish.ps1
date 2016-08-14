gulp build --production

$publishPath = "$PSScriptRoot\artifacts\publish"

If (Test-Path $publishPath)
{
	Remove-Item $publishPath -ErrorAction Ignore -Recurse
}

New-Item -ItemType Directory -Force -Path $publishPath
dotnet publish (Resolve-Path "$PSScriptRoot\src\Blongo") --configuration Release --output (Resolve-Path $publishPath)