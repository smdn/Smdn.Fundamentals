#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary:
    Downloads .nuspec for the specified ID and version of the package.

  Parameters:
    -PackageId : An ID for the package.
    -PackageVersion : A semver string for the package version.
    -PackageSource : A package source URI from which the package to be downloaded. nuget.org is used by default.
    -FilePath : A path to the file which the downloaded content will be output.
                If not specified or '-' is specified, standard output is used.
#>
Param(
  [Parameter(Mandatory = $true)][string]$PackageId,
  [Parameter(Mandatory = $true)][string]$PackageVersion,
  [string]$PackageSource = 'https://api.nuget.org/v3/index.json',
  [string]$FilePath = $null
)

Import-Module "$PSScriptRoot\psmodules\NuGet.ServiceIndex\" -Function 'Get-NuGetResourceBaseAddress'
Import-Module "$PSScriptRoot\psmodules\NuGet.PackageContent\" -Function 'Get-NuGetManifest'

$nuget_service_package_base_address = Get-NuGetResourceBaseAddress 'PackageBaseAddress/3.0.0' $PackageSource

$nuspec_content = Get-NuGetManifest $nuget_service_package_base_address $PackageId $PackageVersion

if ([string]::IsNullOrEmpty($FilePath) -or $FilePath -eq '-') {
  $nuspec_content | Write-Output
}
else {
  $nuspec_content | Out-File -FilePath $FilePath
}
