Start-Process -FilePath cmd -ArgumentList "/K cd $PSScriptRoot\src\Blongo & dotnet run"
Start-Process -FilePath cmd -ArgumentList "/K mongod"
Start-Process -FilePath cmd -ArgumentList "/K gulp"
& "C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start
Start-Process -FilePath chrome.exe -ArgumentList "http://localhost:47328"