#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary: Gets versions of specified NuGet package.
  Ref: Package Content §Enumerate package versions
    https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource#enumerate-package-versions
#>
function Get-NuGetPackageVersions {
  [OutputType([string[]])]
  Param(
    [Parameter(Mandatory = $true)][string]$nuget_service_package_base_address,
    [Parameter(Mandatory = $true)][string]$package_id
  )

  $get_package_versions_response = Invoke-WebRequest `
    -SkipHttpErrorCheck `
    -URI "${nuget_service_package_base_address}$($package_id.ToLowerInvariant())/index.json"

  if ($get_package_versions_response.StatusCode -eq 404) {
    Write-Warning "The package ${package_id} did not find."
    return $null
  }
  elseif ($get_package_versions_response.StatusCode -ne 200) {
    $get_package_versions_response | Write-Error
    throw "Failed to GET package versions for ${package_id} (StatusCode: $($get_package_versions_response.StatusCode))"
  }

  return $get_package_versions_response.Content | `
    ConvertFrom-Json | `
    Select-Object -ExpandProperty 'versions'
}

<#
  Summary: Parses and gets the semantic version from the specified version sstring.
#>
function Get-SemanticVersion {
  Param(
    [Parameter(Mandatory = $true)][string]$Version
  )

  $components = $Version -split { $_ -eq '+' -or $_-eq '-' }, 2

  $version_core = $components[0]
  $is_prerelease = $Version[$version_core.Length] -eq '-'

  $suffix = $components.Count -eq 2 ? $components[1] : $null

  if ($is_prerelease) {
    $suffix_components = $suffix -split { $_ -eq '+' -or $_-eq '-' }, 2

    if ($suffix_components.Count -eq 2) {
      $prerelease = $suffix_components[0]
      $build = $suffix_components[1]
    }
    else {
      $prerelease = $suffix
      $build = $null
    }
  }
  else {
    $build = $components[1]
  }

  return [PSCustomObject]@{
    Version = $Version
    VersionCore = New-Object System.Version -ArgumentList $version_core
    Suffix = $suffix
    IsPreRelease = $is_prerelease
    PreRelease = $prerelease
    Build = $build
  }
}

<#
  Summary: Sorts the version strings by order of the semantic version.
#>
function Get-OrderedSemanticVersions {
  [OutputType([string[]])]
  Param(
    [Parameter(Mandatory = $true)]
    [string[]]$Versions,

    [switch][bool]$Descending = $false,
    [switch][bool]$Unique = $false
  )

  return $Versions | `
    ForEach-Object {
      Get-SemanticVersion $_
    } | `
    Sort-Object `
      -Property `
        @{Expression = "VersionCore"; Descending = $Descending}, `
        @{Expression = { -not $_.IsPreRelease }; Descending = $Descending}, `
        @{Expression = { $_.PreRelease.Length }; Descending = $Descending}, `
        @{Expression = "PreRelease"; Descending = $Descending}, `
        @{Expression = { $_.Build.Length }; Descending = $Descending}, `
        @{Expression = "Build"; Descending = $Descending} | `
    Select-Object `
      -ExpandProperty Version `
      -Unique:$Unique
}

<#
  Summary: Gets the latest version of specified NuGet package.
  Ref: Package Content §Enumerate package versions
    https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource#enumerate-package-versions
#>
function Get-NuGetLatestPackageVersion {
  [OutputType([string])]
  Param(
    [Parameter(Mandatory = $true)][string]$nuget_service_package_base_address,
    [Parameter(Mandatory = $true)][string]$package_id
  )

  $package_versions = Get-NuGetPackageVersions $nuget_service_package_base_address $package_id

  if ($package_versions.Count -le 0) {
    return $null
  }

  $ordered_versions = Get-OrderedSemanticVersions $package_versions -Descending

  return $ordered_versions[0]
}

<#
  Summary: Downloads .nuspec file for specified NuGet package and returns its content.
  Ref: Package Content §Download package manifest
    https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource#download-package-manifest-nuspec
#>
function Get-NuGetManifest {
  [OutputType([string])]
  Param(
    [Parameter(Mandatory = $true)][string]$nuget_service_package_base_address,
    [Parameter(Mandatory = $true)][string]$package_id,
    [Parameter(Mandatory = $true)][string]$package_version
  )

  $package_lower_id = $package_id.ToLowerInvariant()

  $get_package_manifest_response = Invoke-WebRequest `
    -SkipHttpErrorCheck `
    -URI "${nuget_service_package_base_address}${package_lower_id}/${package_version}/${package_lower_id}.nuspec" `
    -ContentType 'text/plain; charset=utf-8'

  if ($get_package_manifest_response.StatusCode -ne 200) {
    $get_package_manifest_response | Write-Error
    throw "Failed to GET manifest for ${package_id} ${package_version} (StatusCode: $($get_package_manifest_response.StatusCode))"
  }

  # Write-Host ($get_package_manifest_response.Headers | Format-Table | Out-String)

  $nuspec = $get_package_manifest_response.Content
  $is_content_charset_utf8 = (
    $get_package_manifest_response.Headers['Content-Type'] -and
    $get_package_manifest_response.Headers['Content-Type'].ToLowerInvariant().Contains('charset=utf-8')
  )

  if (!$is_content_charset_utf8) {
    $nuspec = [System.Text.Encoding]::UTF8.GetString(
      [System.Text.Encoding]::GetEncoding("ISO-8859-1").GetBytes($nuspec)
    )
  }

  return $nuspec
}

Export-ModuleMember -Function Get-NuGetLatestPackageVersion, Get-NuGetManifest
