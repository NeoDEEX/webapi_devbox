 #
 # Biz 모듈 dll 과 .foxml 파일의 날짜를 최신으로 갱신.
 #
 Get-ChildItem -Path "./bizmodules/*" | ForEach-Object {
    Write-Host "touching $($_.Name)"
    $_.LastWriteTime = Get-Date
}
 Get-ChildItem -Path "./foxml/*" | ForEach-Object { 
    Write-Host "touching $($_.Name)"
    $_.LastWriteTime = Get-Date
}