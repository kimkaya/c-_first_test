@echo off
chcp 65001 >nul
echo ====================================
echo   환경 설정 확인
echo ====================================
echo.

cd /d "%~dp0"

echo [1/3] Windows 버전 확인...
ver
echo.

echo [2/3] .NET SDK 확인...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [X] .NET SDK가 설치되어 있지 않습니다!
    echo.
    echo 다음 링크에서 .NET 6.0 이상을 설치해주세요:
    echo https://dotnet.microsoft.com/download
    echo.
    echo 설치 후 다시 이 파일을 실행하세요.
    goto :error
) else (
    echo [O] .NET SDK 버전:
    dotnet --version
)
echo.

echo [3/3] 프로젝트 파일 확인...
if not exist "OsuRhythmGame.csproj" (
    echo [X] 프로젝트 파일을 찾을 수 없습니다!
    goto :error
) else (
    echo [O] 프로젝트 파일 확인됨
)
echo.

echo ====================================
echo   모든 준비가 완료되었습니다!
echo ====================================
echo.
echo 다음 명령으로 게임을 실행하세요:
echo   run.bat      - 게임 실행
echo   build.bat    - 빌드만 수행
echo   clean.bat    - 빌드 파일 정리
echo.
pause
exit /b 0

:error
echo.
echo ====================================
echo   설정에 문제가 있습니다
echo ====================================
echo.
pause
exit /b 1
