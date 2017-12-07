Param(
  [Parameter(Mandatory=$true)][int]$major,
  [Parameter(Mandatory=$true)][int]$minor,
  [Parameter(Mandatory=$false)][string]$buildConfiguration='Release',
  [Parameter(Mandatory=$false)][string]$buildPlatform='Any CPU',
  [Parameter(Mandatory=$false)][bool]$makeReleaseVersion = $True
)

$ErrorActionPreference = "Stop"

##
## Build
##
$msbuildpath = & "$(${env:ProgramFiles(x86)})\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
if ($msbuildpath) {
  $msbuildpath = join-path $msbuildpath 'MSBuild\15.0\Bin\MSBuild.exe'
  if (test-path $msbuildpath) {
  }
}

& $msbuildpath Nightwatch.sln /t:Build /p:Configuration=$buildConfiguration /p:Platform=$buildPlatform

if ($LastExitCode -ne 0) {
    Write-Error "Build failed!!!"
}

##
## Apply semantic versioning
##

$date = Get-Date
$year = $date.Year.ToString().Substring(2)
$dayofyear = $date.DayOfYear.ToString()
$rev = [math]::Round($date.TimeOfDay.TotalSeconds/4)
$patch = "$year$dayofyear"
$buildNumber = "$major.$minor.$patch.$rev"

$applySemVerInvoke = ".\ApplySemanticVersioningToAssemblies.ps1 -pathToSearch '.' -buildNumber $buildNumber -makeReleaseVersion $makeReleaseVersion -preReleaseName 'beta' -includeRevInPreRelease $([bool]::FalseString) -versionNumbersInAssemblyVersion '4'"
Invoke-Expression $applySemVerInvoke

##
## NuGet pack
##

.\nuget.exe pack .\Nightwatch\Nightwatch.csproj -Prop Configuration=$buildConfiguration