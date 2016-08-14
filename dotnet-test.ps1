$testPath = "$PSScriptRoot\artifacts\test"

If (Test-Path $testPath)
{
	Remove-Item $testPath -ErrorAction Ignore -Recurse
}

Get-ChildItem -Path "$PSScriptRoot\test" | ?{ $_.PSIsContainer } | % { $outputPath = $testPath + "\" + $_.Name; New-Item -ItemType Directory -Force -Path $outputPath; dotnet test $_.FullName --configuration Release --work $outputPath; }