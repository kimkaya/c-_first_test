@echo off
chcp 65001 >nul
echo ====================================
echo   빌드 파일 정리
echo ====================================
echo.

cd /d "%~dp0"

echo bin 폴더 삭제 중...
if exist bin (
    rmdir /s /q bin
    echo [완료] bin 폴더 삭제됨
) else (
    echo [정보] bin 폴더가 없습니다
)

echo.
echo obj 폴더 삭제 중...
if exist obj (
    rmdir /s /q obj
    echo [완료] obj 폴더 삭제됨
) else (
    echo [정보] obj 폴더가 없습니다
)

echo.
echo ====================================
echo   정리 완료!
echo ====================================
echo.
pause
