@echo off
chcp 65001 >nul
title osu! 리듬게임

echo.
echo ================================
echo   게임을 시작합니다...
echo ================================
echo.

cd /d "%~dp0"

REM 빌드된 실행파일이 있으면 바로 실행
if exist "OsuRhythmGame\bin\Release\net8.0-windows\OsuRhythmGame.exe" (
    echo [빠른 실행] 빌드된 파일 실행 중...
    start "" "OsuRhythmGame\bin\Release\net8.0-windows\OsuRhythmGame.exe"
    exit
)

REM 없으면 dotnet run
echo [첫 실행] 빌드 후 실행 중...
echo.
cd OsuRhythmGame
dotnet run --configuration Release

if %errorlevel% neq 0 (
    echo.
    echo [오류] 실행에 실패했습니다.
    pause
)
