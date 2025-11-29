# PowerShell 스크립트 - 게임 실행
# UTF-8 인코딩
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "====================================" -ForegroundColor Cyan
Write-Host "  osu! 스타일 리듬게임 실행" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host ""

# 스크립트 위치로 이동
Set-Location $PSScriptRoot

# .NET SDK 확인
Write-Host "[1/2] .NET SDK 확인 중..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "[확인] .NET SDK 버전: $dotnetVersion" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "[오류] .NET SDK가 설치되어 있지 않습니다!" -ForegroundColor Red
    Write-Host ""
    Write-Host ".NET 6.0 이상을 설치해주세요:" -ForegroundColor Yellow
    Write-Host "https://dotnet.microsoft.com/download" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "계속하려면 Enter를 누르세요"
    exit 1
}

# 게임 실행
Write-Host "[2/2] 게임 실행 중..." -ForegroundColor Yellow
Write-Host ""

dotnet run

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "[오류] 게임 실행에 실패했습니다." -ForegroundColor Red
    Write-Host ""
    Read-Host "계속하려면 Enter를 누르세요"
    exit 1
}
