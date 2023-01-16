#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

#
# Summary:
#   Compares and generates differences between .nuspec from the specified package's latest version and .nuspec generated from the specified project.
#   The specified project is always considered as the newer version.
#
# Parameters:
#   -PackageId : An ID for the target package from which the .nuspec to be compared.
#   -ProjectDirectoryPath : A path to the directory that contains the target project. The .nuspec file generated from this project will be used for comparison.
#   -NuSpecFilePath : A path to the .nuspec file that will be used for comparison.
#   -FilePath : A path to the file which the generated differences will be output. If not specified or '-' is specified, standard output is used.
#   -PackageSource : A package source URI from which the package to be downloaded. nuget.org is used by default.
#   -NoBuild : A switch for specifying whether to build the project or not when generating the .nuspec.
#
# Usage:
#   ./nuspec-diff.ps1 -NoBuild -PackageId Smdn.Fundamental.Exception -ProjectDirectoryPath path/to/project/Smdn.Fundamental.Exception/
#   ./nuspec-diff.ps1 -NoBuild -PackageId Smdn.Fundamental.Exception -ProjectDirectoryPath path/to/project/Smdn.Fundamental.Exception/ output.diff
#   ./nuspec-diff.ps1 -NoBuild -PackageId Smdn.Fundamental.Exception -NuSpecFilePath path/to/file.nuspec output.diff
#
[CmdletBinding(DefaultParameterSetName="CompareWithProject")]
Param(
  [Parameter(Mandatory = $true, ParameterSetName = 'CompareWithProject')]
  [Parameter(Mandatory = $true, ParameterSetName = 'CompareWithNuSpecFile')]
  [string]$PackageId,

  [Parameter(Mandatory = $true, ParameterSetName = 'CompareWithProject')]
  [string]$ProjectDirectoryPath,

  [Parameter(Mandatory = $true, ParameterSetName = 'CompareWithNuSpecFile')]
  [string]$NuSpecFilePath,

  [Parameter(ParameterSetName = 'CompareWithProject')]
  [Parameter(ParameterSetName = 'CompareWithNuSpecFile')]
  [string]$FilePath = $null,

  [Parameter(ParameterSetName = 'CompareWithProject')]
  [Parameter(ParameterSetName = 'CompareWithNuSpecFile')]
  [string]$PackageSource = 'https://api.nuget.org/v3/index.json',

  [Parameter(ParameterSetName = 'CompareWithProject')]
  [switch]
  [bool]$NoBuild = $false
)

# Summary: Fetches service index for the specified source, and returns resource base address (@id) for specified resource type (@type).
# Ref: https://learn.microsoft.com/en-us/nuget/api/service-index
function Get-NuGetResourceBaseAddress {
  param(
    $source_path,
    $resource_type
  )

  $get_service_index_response = Invoke-WebRequest -URI $source_path

  if ($get_service_index_response.StatusCode -ne 200) {
    $get_service_index_response | Write-Error
    throw "Failed to GET resource base address for ${resource_type}"
  }

  $service_index = $get_service_index_response.Content | ConvertFrom-Json

  return $service_index.resources | `
    Where-Object '@type' -eq $resource_type | `
    Select-Object -ExpandProperty '@id'
}

# Summary: Enumerates versions of specified NuGet package and returns the latest one.
# Ref: Package Content §Enumerate package versions
#   https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource#enumerate-package-versions
function Get-NuGetLatestPackageVersion {
  param(
    $nuget_service_package_base_address,
    $package_id
  )

  $get_package_versions_response = Invoke-WebRequest -URI "${nuget_service_package_base_address}$($package_id.ToLowerInvariant())/index.json"

  if ($get_package_versions_response.StatusCode -ne 200) {
    $get_package_versions_response | Write-Error
    throw "Failed to GET package versions for ${package_id}"
  }

  $package_versions = $get_package_versions_response.Content | `
    ConvertFrom-Json | `
    Select-Object -ExpandProperty 'versions' | `
    ForEach-Object { New-Object System.Version -ArgumentList $_ } | `
    Sort-Object -Descending

  return $package_versions.Count -gt 0 ? $package_versions[0] : $null
}

# Summary: Downloads .nuspec file for specified NuGet package and returns its content.
# Ref: Package Content §Download package manifest
#   https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource#download-package-manifest-nuspec
function Get-NuGetManifest {
  param(
    $nuget_service_package_base_address,
    $package_id,
    $package_version
  )

  $package_lower_id = $package_id.ToLowerInvariant()

  $get_package_manifest_response = Invoke-WebRequest `
    -URI "${nuget_service_package_base_address}${package_lower_id}/${package_version}/${package_lower_id}.nuspec" `
    -ContentType 'text/plain; charset=utf-8'

  if ($get_package_manifest_response.StatusCode -ne 200) {
    $get_package_manifest_response | Write-Error
    throw "Failed to GET manifest for ${package_id} ${package_version}"
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

# Summary: Runs 'GenerateNuspec' target for the specified project, and returns content of generated .nuspec file.
function New-ProjectNuSpec {
  param(
    $path_to_project,
    $nuspec_output_path,
    $no_build
  )

  $generate_nuspec_options = (
    '-noLogo',
    '/p:_IsPacking=true',
    '/p:Configuration=Release',
    '/p:APIListEnableGenerating=false',
    "/p:NuspecOutputPath=$nuspec_output_path"
  )
  $generate_nuspec_options += $no_build ? '/p:NoBuild=true' : '/p:NoBuild=false'
  $generate_nuspec_options += $path_to_project

  $(dotnet msbuild /t:GenerateNuspec $generate_nuspec_options) | Write-Host

  $path_to_generated_nuspec_file = Get-ChildItem -Path "${nuspec_output_path}*.nuspec" -File | `
     Sort-Object -Property LastWriteTime -Descending | `
     Select-Object -First 1

  return $path_to_generated_nuspec_file
}

<#
function Remove-NuSpecXmlFilesElement {
  param(
    $nuspec_xml
  )

  # remove //package/files
  if ($nuspec_xml.package) {
    $files_element = $nuspec_xml.package.files

    if ($files_element) {
      [void]$files_element.ParentNode.RemoveChild($files_element)
    }
  }
}
#>

# Summary: Writes XML document to the specified path with the format suitable for diff.
function Out-XmlForDiff {
  param(
    $xml_document,
    $path
  )

  $settings = New-Object System.Xml.XmlWriterSettings
  $settings.Indent = $true
  $settings.IndentChars = '  '
  $settings.NewLineOnAttributes = $false
  $settings.CloseOutput = $true

  $writer = [System.Xml.XmlWriter]::Create([string]$path, $settings)

  $xml_document.Save($writer)

  $writer.Close()
}

# Summary: Compares two .nuspec XML document and returns their differences in 'unified' diff format
function Compare-NuSpecXmlDifference {
  param(
    $nuspec_old, # nullable
    $nuspec_old_label,
    $nuspec_new, # nullable
    $nuspec_new_label
  )

  $nuspec_xml_old = $nuspec_old -eq $null ? $null : [xml]$nuspec_old
  $nuspec_xml_new = $nuspec_new -eq $null ? $null : [xml]$nuspec_new

  # Remove-NuSpecXmlFilesElement $nuspec_xml_old
  # Remove-NuSpecXmlFilesElement $nuspec_xml_new

  $path_to_temporary_directory = Join-Path -Path $([System.IO.Path]::GetTempPath()) -ChildPath $(New-Guid)

  try {
    New-Item -Type Directory -Path $path_to_temporary_directory | Out-Null

    $nuspec_xml_old_path = Join-Path -Path $path_to_temporary_directory -ChildPath 'old.nuspec'
    $nuspec_xml_new_path = Join-Path -Path $path_to_temporary_directory -ChildPath 'new.nuspec'

    if ($nuspec_xml_old) {
      Out-XmlForDiff $nuspec_xml_old $nuspec_xml_old_path
    }
    else {
      New-Item $nuspec_xml_old_path
    }

    if ($nuspec_xml_new) {
      Out-XmlForDiff $nuspec_xml_new $nuspec_xml_new_path
    }
    else {
      New-Item $nuspec_xml_new_path
    }

    $diff = diff `
      "--label=$nuspec_old_label" `
      "--label=$nuspec_new_label" `
      --unified=1000 `
      $nuspec_xml_old_path `
      $nuspec_xml_new_path

    return $diff
  }
  finally {
    Remove-Item -Path $path_to_temporary_directory -Recurse
  }
}

# Summary: Compares .nuspec of the latest package and .nuspec generated by current build with `diff` command, and returns their differences
function Compare-NuSpecDifference {
  param(
    $package_source_path,
    $package_id,
    $path_to_nuspec_file,
    $no_build
  )

  # gets content of specified .nuspec file
  $nuspec_content_new = Get-Content -Path $path_to_nuspec_file
  $nuspec_label_new = Split-Path $path_to_nuspec_file -Leaf

  # downloads .nuspec for specified NuGet package
  $package_base_address = Get-NuGetResourceBaseAddress $package_source_path 'PackageBaseAddress/3.0.0'
  $package_version_latest = Get-NuGetLatestPackageVersion $package_base_address $package_id

  if ($package_version_latest) {
    $nuspec_content_old = Get-NuGetManifest $package_base_address $package_id $package_version_latest
    $nuspec_label_old = "${package_id}.${package_version_latest}.nuspec"
  }
  else {
    Write-Warning "No latest version available for the package ${package_id}"

    $nuspec_content_old = $null
    $nuspec_label_old = '<null>'
  }

  #
  # compares and get differences of the two .nuspec
  #
  return Compare-NuSpecXmlDifference `
    $nuspec_content_old `
    $nuspec_label_old `
    $nuspec_content_new `
    $nuspec_label_new
}

#
# main
#
if ($PsCmdlet.ParameterSetName -eq "CompareWithProject") {
  # generates .nuspec file for the specified project
  $path_to_nuspec_outout_directory = Join-Path -Path $ProjectDirectoryPath -ChildPath 'nuspec/'

  $path_to_nuspec_file = New-ProjectNuSpec `
    $ProjectDirectoryPath `
    $path_to_nuspec_outout_directory `
    $NoBuild
}
elseif ($PsCmdlet.ParameterSetName -eq "CompareWithNuSpecFile") {
  $path_to_nuspec_file = $NuSpecFilePath
}
else {
  throw "unknown parameter set: $($PsCmdlet.ParameterSetName)";
}

$nuspec_diff = Compare-NuSpecDifference `
  $PackageSource `
  $PackageId `
  $path_to_nuspec_file

if ([string]::IsNullOrEmpty($FilePath) -or $FilePath -eq '-') {
  $nuspec_diff | Write-Host
}
else {
  $nuspec_diff | Out-File -FilePath $FilePath
}
