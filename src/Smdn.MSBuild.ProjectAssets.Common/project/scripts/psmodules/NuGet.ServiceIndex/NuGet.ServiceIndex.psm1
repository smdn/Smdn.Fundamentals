#!/usr/bin/env pwsh
# SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
# SPDX-License-Identifier: MIT

<#
  Summary: Retrieves resource base address for the specified resource type.
  Ref: Service index
    https://learn.microsoft.com/en-us/nuget/api/service-index
#>
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

Export-ModuleMember -Function 'Get-NuGetResourceBaseAddress'
