Param(
  [Parameter(Mandatory=$false)][int]$major = '0',
  [Parameter(Mandatory=$false)][int]$minor = '3',
  [Parameter(Mandatory=$false)][string]$buildConfiguration='Release',
  [Parameter(Mandatory=$false)][string]$buildPlatform='Any CPU',
  [Parameter(Mandatory=$false)][bool]$makeReleaseVersion = $True
)

$ErrorActionPreference = "Stop"

##
## Apply semantic versioning
##

$date = Get-Date
$year = $date.Year.ToString().Substring(2)
$dayofyear = $date.DayOfYear.ToString()
$rev = [math]::Round($date.TimeOfDay.TotalSeconds/4)
$patch = "$year$dayofyear"
#$buildNumber = "$major.$minor.$patch.$rev"
#$slimBuildNumber = "$major.$minor.$patch"

$buildNumber = "$major.$minor.$rev.0"
$slimBuildNumber = "$major.$minor.$rev"

$applySemVerInvoke = ".\ApplySemanticVersioningToAssemblies.ps1 -pathToSearch '.' -buildNumber $buildNumber -makeReleaseVersion $makeReleaseVersion -preReleaseName 'beta' -includeRevInPreRelease $([bool]::FalseString) -versionNumbersInAssemblyVersion '2'"
Invoke-Expression $applySemVerInvoke

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
## NuGet pack
##

.\nuget.exe pack .\Nightwatch\Nightwatch.nuspec -version $slimBuildNumber

##
## Releasify
##

.\packages\squirrel.windows.1.7.8\tools\Squirrel --releasify Nightwatch.$slimBuildNumber.nupkg