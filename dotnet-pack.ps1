$packPath = "$PSScriptRoot\artifacts\nuget"

If (Test-Path $packPath)
{
	Remove-Item $packPath -ErrorAction Ignore -Recurse
}

New-Item -ItemType Directory -Force -Path $packPath

Get-ChildItem -Path "$PSScriptRoot\src" | ?{ $_.PSIsContainer } | % { dotnet pack (Resolve-Path $_.FullName) --configuration Release --output (Resolve-Path $packPath) }
