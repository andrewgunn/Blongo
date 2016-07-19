$testPath = "artifacts\test"

If (Test-Path $testPath)
{
	Remove-Item $testPath -ErrorAction Ignore -Recurse
}

Get-ChildItem -Path "test" | ?{ $_.PSIsContainer } | % { $outputPath = $testPath + "\" + $_.Name; New-Item -ItemType Directory -Force -Path $outputPath; dotnet test $_.FullName --configuration Release --work (Resolve-Path $outputPath); }