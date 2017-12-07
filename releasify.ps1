
$msbuildpath = & "$(${env:ProgramFiles(x86)})\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
if ($msbuildpath) {
  $msbuildpath = join-path $msbuildpath 'MSBuild\15.0\Bin\MSBuild.exe'
  if (test-path $msbuildpath) {
  }
}

& $msbuildpath Nightwatch.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU"

if ($LastExitCode -ne 0) {
    Write-Error "Build failed!!!"
}