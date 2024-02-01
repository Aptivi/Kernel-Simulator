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

for /f "tokens=* USEBACKQ" %%f in (`type version`) do set ksversion=%%f

:packbin
echo Packing binary...
del "Kernel Simulator\KSBuild\*.nupkg" >> %temp%/buildandpack.log 2>&1
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-bin.zip "..\Kernel Simulator\KSBuild\net48\*" >> %temp%/buildandpack.log 2>&1
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-bin-dotnet.zip "..\Kernel Simulator\KSBuild\net8.0\*" >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%ksversion%-bin.zip
move %temp%\%ksversion%-bin-dotnet.zip
copy "..\Kernel Simulator\KSBuild\net48\Kernel Simulator.pdb" .\%ksversion%.pdb
copy "..\Kernel Simulator\KSBuild\net8.0\Kernel Simulator.pdb" .\%ksversion%-dotnet.pdb

echo Pack successful.
:finished
