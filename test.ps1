$files = get-childitem src -include project.json -recurse

 foreach ($file in $files) {
    Write-Output $file 
 }