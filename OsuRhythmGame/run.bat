@echo off
chcp 65001 >nul
echo ====================================
echo   osu! 스타일 리듬게임 실행
echo ====================================
echo.

cd /d "%~dp0"

echo [1/2] .NET SDK 확인 중...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [오류] .NET SDK가 설치되어 있지 않습니다!
    echo.
    echo .NET 6.0 이상을 설치해주세요:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo [확인] .NET SDK 설치됨
echo.

echo [2/2] 게임 실행 중...
echo.
dotnet run

if %errorlevel% neq 0 (
    echo.
    echo [오류] 게임 실행에 실패했습니다.
    echo.
    pause
    exit /b 1
)
