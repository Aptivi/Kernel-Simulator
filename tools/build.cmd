@echo off

REM    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
REM
REM    This file is part of Kernel Simulator
REM
REM    Kernel Simulator is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    Kernel Simulator is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.

REM This script builds KS and packs the artifacts. Use when you have VS installed.
for /f "tokens=* USEBACKQ" %%f in (`type version`) do set ksversion=%%f

:build
echo Building Kernel Simulator...
"%ProgramFiles%\dotnet\dotnet.exe" msbuild "..\Kernel Simulator.sln" -t:restore -p:Configuration=Release-dotnet
"%ProgramFiles%\dotnet\dotnet.exe" msbuild "..\Kernel Simulator.sln" -p:Configuration=Release-dotnet
"%ProgramFiles%\dotnet\dotnet.exe" msbuild "..\Kernel Simulator.sln" -t:restore -p:Configuration=Release
"%ProgramFiles%\dotnet\dotnet.exe" msbuild "..\Kernel Simulator.sln" -p:Configuration=Release
if %errorlevel% == 0 goto :success
echo There was an error trying to build (%errorlevel%).
goto :finished

:success
echo Build successful.
:finished
