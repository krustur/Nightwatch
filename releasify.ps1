Param(
  [Parameter(Mandatory=$true)][int]$major,
  [Parameter(Mandatory=$true)][int]$minor,
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

.\packages\squirrel.windows.1.7.8\tools\Squirrel --releasify Nightwatch.$slimBuildNumber.nupkg --icon .\Nightwatch\icon.ico

##
## Create Release on github
##

Write-Output '===[ And now ]==='
Write-Output '1) Commit Latest Code - In order for GitHub to mark a new release as the Latest, you have at least one additional commit since the last release tag was added (i.e., releases tags must not share the same commit).'
Write-Output '2) Create a New Release - Create a new GitHub release in your MyApp repository matching your current release version (e.g., 1.0.0).'
Write-Output '3) Upload Release Files - upload all of the files from Releases as assets of the GitHub release (e.g., RELEASES, MyApp.1.0.0-full.nupkg, MyApp.1.0.1-delta.nupkg, MyApp.1.0.1-full.nupkg).'
Write-Output '4) Set Pre-release (optional) - if desired, set the release as a pre-release.'
Write-Output '5) Publish the Release - click the "Publish Release" to make the release available to the general public and your users.'