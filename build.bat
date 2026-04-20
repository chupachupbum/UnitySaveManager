@echo off
echo ================================
echo  Building Unity Save Manager
echo ================================
echo.

REM Find dotnet
where dotnet >nul 2>&1
if %ERRORLEVEL% neq 0 (
    set DOTNET="C:\Program Files\dotnet\dotnet.exe"
) else (
    set DOTNET=dotnet
)

echo Restoring packages...
%DOTNET% restore UnitySaveManager\UnitySaveManager.csproj
if %ERRORLEVEL% neq 0 goto :error

echo.
echo Publishing self-contained single-file exe...
%DOTNET% publish UnitySaveManager\UnitySaveManager.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o publish
if %ERRORLEVEL% neq 0 goto :error

echo.
echo ================================
echo  Build complete!
echo  Output: publish\UnitySaveManager.exe
echo ================================
exit /b 0

:error
echo.
echo BUILD FAILED!
exit /b 1
