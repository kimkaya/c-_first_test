@echo off
chcp 65001 >nul
title osu! 리듬게임
cls

echo.
echo ╔════════════════════════════════╗
echo ║   osu! 스타일 리듬게임         ║
echo ╚════════════════════════════════╝
echo.

cd /d "%~dp0"

set "EXE_PATH=%~dp0bin\Release\net8.0-windows\OsuRhythmGame.exe"

if exist "%EXE_PATH%" (
    echo ✓ 실행 파일을 찾았습니다.
    echo   경로: %EXE_PATH%
    echo.
    echo 게임을 시작합니다...
    echo.
    start "" "%EXE_PATH%"
    exit
) else (
    echo ✗ 실행 파일을 찾을 수 없습니다.
    echo.
    echo 찾은 경로: %EXE_PATH%
    echo.
    echo [해결 방법]
    echo 1. build.bat을 먼저 실행하세요
    echo 2. 또는 run.bat을 실행하세요
    echo.
    pause
    exit /b 1
)
