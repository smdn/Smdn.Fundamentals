#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary:
    Compares and generates differences between the specified .nuspec files.

  Parameters:
    -OldNuspecPath: A path to the .nuspec file to be compared as an older one. This parameter can be omit.
    -NewNuspecPath: A path to the .nuspec file to be compared as a newer one.
    -ExceptFilesElement: A switch for specifying whether to exclude <files> elements in .nuspec files for comparison.
    -FilePath : A path to the file which the differences will be output.
                If not specified or '-' is specified, standard output is used.
#>
Param(
  [string]$OldNuspecPath,
  [Parameter(Mandatory = $true)][string]$NewNuspecPath,
  [switch][bool]$ExceptFilesElement = $false,
  [string]$FilePath = $null
)

function Remove-NuspecXmlFilesElement {
  param(
    [Parameter(Mandatory = $true)][System.Xml.XmlDocument]$nuspec_xml
  )

  # remove //package/files
  if ($nuspec_xml.package) {
    $files_element = $nuspec_xml.package.files

    if ($files_element) {
      [void]$files_element.ParentNode.RemoveChild($files_element)
    }
  }
}

# Summary: Writes XML document to the specified path with the format suitable for diff.
function Out-XmlForDiff {
  Param(
    [Parameter(Mandatory = $true)][System.Xml.XmlDocument]$xml_document,
    [Parameter(Mandatory = $true)][string]$path
  )

  $settings = New-Object System.Xml.XmlWriterSettings
  $settings.Indent = $true
  $settings.IndentChars = '  '
  $settings.NewLineOnAttributes = $false
  $settings.CloseOutput = $true
  $settings.Encoding = New-Object -TypeName System.Text.UTF8Encoding -ArgumentList $false

  $writer = [System.Xml.XmlWriter]::Create([string]$path, $settings)

  $xml_document.Save($writer)

  $writer.Close()
}

# Summary: Compares two .nuspec XML document and returns their differences in 'unified' diff format
function Compare-NuspecXmlDifference {
  [OutputType([string])]
  Param(
    [string]$nuspec_old = $null,
    [string]$nuspec_old_label = $null,
    [string]$nuspec_new = $null,
    [string]$nuspec_new_label = $null,
    [bool]$except_files_element = $false
  )

  $nuspec_xml_old = $null -eq $nuspec_old ? $null : [xml]$nuspec_old
  $nuspec_xml_new = $null -eq $nuspec_new ? $null : [xml]$nuspec_new

  if ($except_files_element) {
    Remove-NuspecXmlFilesElement $nuspec_xml_old
    Remove-NuspecXmlFilesElement $nuspec_xml_new
  }

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

#
# main
#
if ($OldNuspecPath) {
  $nuspec_old_content = Get-Content -Path $OldNuspecPath
  $nuspec_old_label = [System.IO.Path]::GetFileName($OldNuspecPath)
}
else {
  $nuspec_old_content = $null
  $nuspec_old_label = '<null>'
}

$nuspec_new_content = Get-Content -Path $NewNuspecPath
$nuspec_new_label = [System.IO.Path]::GetFileName($NewNuspecPath)

$nuspec_diff = Compare-NuspecXmlDifference `
  $nuspec_old_content `
  $nuspec_old_label `
  $nuspec_new_content `
  $nuspec_new_label `
  $ExceptFilesElement `

if ([string]::IsNullOrEmpty($FilePath) -or $FilePath -eq '-') {
  $nuspec_diff | Write-Output
}
else {
  $nuspec_diff | Out-File -FilePath $FilePath
}
