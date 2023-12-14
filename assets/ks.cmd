@echo off

REM    Nitrocid KS  Copyright (C) 2018-2024  Aptivi
REM
REM    This file is part of Nitrocid KS
REM
REM    Nitrocid KS is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    Nitrocid KS is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.

REM This script runs Nitrocid KS. This is a shortcut for running Kernel
REM Simulator so that you don't have to write the full name of the executable.

REM Please note that we don't support updating in this script, because we use
REM Chocolatey and NuGet to manage updates for KS.
"Nitrocid.exe" %*
