$testPath = "artifacts\test"

If (Test-Path $testPath)
{
	Remove-Item $testPath -ErrorAction Ignore -Recurse
}

New-Item -ItemType Directory -Force -Path $($testPath + "\AkismetSdk.Tests")
dotnet test (Resolve-Path test\AkismetSdk.Tests) --configuration Release --work (Resolve-Path $($testPath + "\AkismetSdk.Tests"))

New-Item -ItemType Directory -Force -Path $($testPath + "\Blongo.Tests")
dotnet test (Resolve-Path test\Blongo.Tests) --configuration Release --work (Resolve-Path $($testPath + "\Blongo.Tests"))