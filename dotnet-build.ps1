Get-ChildItem -Path "src" | ?{ $_.PSIsContainer } | % { dotnet build $_.FullName --configuration Release }
Get-ChildItem -Path "test" | ?{ $_.PSIsContainer } | % { dotnet build $_.FullName --configuration Release }