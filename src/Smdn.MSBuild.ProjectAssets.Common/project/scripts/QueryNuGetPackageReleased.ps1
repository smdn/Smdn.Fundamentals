#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary:
    Queries whether the specified ID and version of the package exists or not.

  Parameters:
    -PackageId : An ID for the package.
    -PackageVersion : A semver string for the package version.
    -PackageSource : A package source URI from which the package to be downloaded. nuget.org is used by default.

  Exit code:
    0: The specified package exists.
    1: The specified package does not exist.

  Usage:
    ./QueryNuGetPackageReleased.ps1 -PackageId Smdn.Fundamental.Exception -PackageVersion 3.0.0
#>
Param(
  [Parameter(Mandatory = $true)][string]$PackageId,
  [Parameter(Mandatory = $true)][string]$PackageVersion,
  [string]$PackageSource = 'https://api.nuget.org/v3/index.json'
)

function Get-NuGetResourceBaseAddress {
  [OutputType([string])]
  Param(
    [Parameter(Mandatory = $true)][string]$resource_type,
    [Parameter(Mandatory = $true)][string]$source_path
  )

  $get_service_index_response = Invoke-WebRequest `
    -Method 'Get' `
    -SkipHttpErrorCheck `
    -URI $source_path

  if ($get_service_index_response.StatusCode -ne 200) {
    $get_service_index_response | Write-Error
    throw "Failed to GET resource base address for ${resource_type} (StatusCode: $($get_service_index_response.StatusCode))"
  }

  $service_index = $get_service_index_response.Content | ConvertFrom-Json

  return $service_index.resources | `
    Where-Object '@type' -eq $resource_type | `
    Select-Object -ExpandProperty '@id'
}

function Get-NuGetPackageManifestStatus {
  [OutputType([bool])]
  Param(
    [Parameter(Mandatory = $true)][string]$package_source,
    [Parameter(Mandatory = $true)][string]$package_id,
    [Parameter(Mandatory = $true)][string]$package_version
  )

  $nuget_service_package_base_address = Get-NuGetResourceBaseAddress 'PackageBaseAddress/3.0.0' $package_source

  $package_id_lowercase = $package_id.ToLowerInvariant()

  $head_package_manifest_response = Invoke-WebRequest `
    -Method 'Head' `
    -SkipHttpErrorCheck `
    -URI "${nuget_service_package_base_address}${package_id_lowercase}/${package_version}/${package_id_lowercase}.nuspec"

  if ($head_package_manifest_response.StatusCode -eq 200) {
    Write-Output "${package_id} v${package_version} exists in ${package_source}."
    return $true
  }
  elseif ($head_package_manifest_response.StatusCode -eq 404) {
    Write-Warning "${package_id} v${package_version} does not exist in ${package_source}."
    return $false
  }
  else {
    throw "Failed to HEAD package manifest for ${package_id} v${package_version} (StatusCode: $($head_package_manifest_response.StatusCode))"
  }
}

$is_package_released = Get-NuGetPackageManifestStatus $PackageSource $PackageId $PackageVersion

exit $is_package_released ? 0 : 1
