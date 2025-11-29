@echo off
chcp 65001 >nul
echo.
echo ====================================
echo   osu! 리듬게임 런처
echo ====================================
echo.

cd /d "%~dp0OsuRhythmGame"

if not exist "OsuRhythmGame.csproj" (
    echo [오류] OsuRhythmGame 폴더를 찾을 수 없습니다!
    echo.
    pause
    exit /b 1
)

echo 게임을 시작합니다...
echo.

call run.bat
