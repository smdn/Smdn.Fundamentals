#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary:
    Queries the latest version of the specified package and outputs to the standard output stream.

  Parameters:
    -PackageId : An ID for the package.
    -PackageSource : A package source URI from which the package to be downloaded. nuget.org is used by default.
#>
Param(
  [Parameter(Mandatory = $true)][string]$PackageId,
  [string]$PackageSource = 'https://api.nuget.org/v3/index.json'
)

Import-Module "$PSScriptRoot\psmodules\NuGet.ServiceIndex\" -Function 'Get-NuGetResourceBaseAddress'
Import-Module "$PSScriptRoot\psmodules\NuGet.PackageContent\" -Function 'Get-NuGetLatestPackageVersion'

$nuget_service_package_base_address = Get-NuGetResourceBaseAddress 'PackageBaseAddress/3.0.0' $PackageSource

$latest_version = Get-NuGetLatestPackageVersion $nuget_service_package_base_address $PackageId

if ($latest_version) {
  $latest_version.ToString() | Write-Output
}
