#!/bin/bash

#    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
#
#    This file is part of Kernel Simulator
#
#    Kernel Simulator is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Kernel Simulator is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with this program.  If not, see <https://www.gnu.org/licenses/>.

# This script builds KS. Use when you have MSBuild installed.
ksversion=$(cat version)
ksreleaseconf=$1
if [ -z $ksreleaseconf ]; then
	ksreleaseconf=Release
fi

# Check for dependencies
msbuildpath=`which msbuild`
if [ ! $? == 0 ]; then
	echo MSBuild is not found.
	exit 1
fi

# Download packages
echo Downloading packages...
"$msbuildpath" "../Kernel Simulator.sln" -t:restore -p:Configuration=$ksreleaseconf
if [ ! $? == 0 ]; then
	echo Download failed.
	exit 1
fi

# Build KS
echo Building KS...
"$msbuildpath" "../Kernel Simulator.sln" -p:Configuration=$ksreleaseconf
if [ ! $? == 0 ]; then
	echo Build failed.
	exit 1
fi

# Inform success
echo Build successful.
exit 0
