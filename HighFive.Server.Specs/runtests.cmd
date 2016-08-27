@pushd %~dp0

set BASE=%CD%

REM Package is already built, no need to rebuild
REM %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "HighFive.Server.Specs.csproj"

@if ERRORLEVEL 1 goto end

@cd ..\packages\SpecRun.Runner.*\tools

SpecRun.exe run "%BASE%\Default.srprofile" "/baseFolder:%BASE%\bin\Debug" /log:specrun.log "/report:%BASE%\SpecFlowReport.html" %2 %3 %4 %5

:end

@popd
