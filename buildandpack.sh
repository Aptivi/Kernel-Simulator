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

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=0.0.16.23

mkdir ~/tmp
echo Make sure you have the following:
echo "  - msbuild (from mono repos)"
echo 
echo Press any key to start.
read -n 1

# Check for dependencies
msbuildpath=`which msbuild`
if [ ! $? == 0 ]; then
	echo MSBuild is not found.
	exit 1
fi
gzippath=`which gzip`
if [ ! $? == 0 ]; then
	echo gzip is not found.
	exit 1
fi
tarpath=`which tar`
if [ ! $? == 0 ]; then
	echo tar is not found.
	exit 1
fi
zippath=`which zip`
if [ ! $? == 0 ]; then
	echo zip is not found.
	exit 1
fi

# Download packages
echo Downloading packages...
"$msbuildpath" -t:restore > ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Download failed.
	exit 1
fi

# Build KS
echo Building KS...
"$msbuildpath" -p:Configuration=Release >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Build failed.
	exit 1
fi

# Pack binary
echo Packing binary...
find . -type f -iname \*.nupkg -delete >> ~/tmp/buildandpack.log
(cd "Kernel Simulator/KSBuild/net48/" && "$zippath" -r /tmp/$ksversion-bin.zip . && cd -) >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Packing using zip failed.
	exit 1
fi

# Pack source
echo Packing source...
rm -r "Kernel Simulator/KSBuild" >> ~/tmp/buildandpack.log
rm -r "Kernel Simulator/obj" >> ~/tmp/buildandpack.log
rm -r "KSTests/KSTest" >> ~/tmp/buildandpack.log
rm -r "KSTests/obj" >> ~/tmp/buildandpack.log
rm -r "KSJsonifyLocales/obj" >> ~/tmp/buildandpack.log
rm -r "KSConverter/obj" >> ~/tmp/buildandpack.log
"$zippath" -r /tmp/$ksversion-src.zip . >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Packing source using zip failed.
	exit 1
fi

# Pack source using tar
echo Packing source using tar...
"$tarpath" --exclude-vcs -cf ~/tmp/$ksversion-src.tar . >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Packing source using tar failed.
	exit 1
fi

# Compress source
echo Compressing source...
"$gzippath" -9 ~/tmp/$ksversion-src.tar >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Compressing source failed.
	exit 1
fi

# Inform success
mv ~/tmp/$ksversion-bin.zip .
mv ~/tmp/$ksversion-src.zip .
mv ~/tmp/$ksversion-src.tar.gz .
echo Build and pack successful.
exit 0
