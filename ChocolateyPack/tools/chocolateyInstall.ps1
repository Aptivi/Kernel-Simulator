﻿$ErrorActionPreference = 'Stop';
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$pkgName    = "KS"
$url        = "https://github.com/EoflaOE/Kernel-Simulator/releases/download/v0.0.12.5-alpha/0.0.12.5-bin.rar"
$md5check   = "ffc4d678f9fb91b39aba2daeec758172"

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Installation directory: $toolsDir"
Write-Output "<*> Package Name: $pkgName"
Write-Output "<*> URL: $url"
Write-Output "<*> Expected MD5 Sum: $md5check"
Write-Output "<+> Configuration will be automatically generated on startup."

Install-ChocolateyZipPackage $pkgName $url $toolsDir -ChecksumType "md5" -Checksum $md5check