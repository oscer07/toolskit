@echo off
echo Building Tool Kit Application...
dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true -o .\build
echo.
echo Build complete! Your EXE is located in the 'build' folder.
pause
