@pushd %~dp0

REM Package is already built, no need to rebuild
REM %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "HighFive.Server.Specs.csproj"

@if ERRORLEVEL 1 goto end

@cd ..\packages\SpecRun.Runner.*\tools

@set profile=%1
@if "%profile%" == "" set profile=Default

SpecRun.exe run "%~dp0\%profile%.srprofile" "/baseFolder:%~dp0\bin\Debug" /log:specrun.log "/report:%~dp0\SpecFlowReport.html" %2 %3 %4 %5

:end

@popd
