$ErrorActionPreference = 'Stop';
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$pkgName    = "KS"
$url        = "https://github.com/EoflaOE/Kernel-Simulator/releases/download/v0.0.20.10-beta/0.0.20.10-bin.rar"
$md5check   = "afb21565190b06e88335013de2a8f680"

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Installation directory: $toolsDir"
Write-Output "<*> Package Name: $pkgName"
Write-Output "<*> URL: $url"
Write-Output "<*> Expected MD5 Sum: $md5check"
Write-Output "<+> Configuration will be automatically generated on startup."

Install-ChocolateyZipPackage $pkgName $url $toolsDir -ChecksumType "md5" -Checksum $md5check