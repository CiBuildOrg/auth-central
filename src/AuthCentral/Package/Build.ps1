param (
    [parameter(Mandatory=$false)][String]$envFile="injectble-vars.env",
    [parameter(Mandatory=$false)][Boolean]$createPackage=$false,
    [parameter(Mandatory=$false)][Boolean]$publishPackage=$false,
    [parameter(Mandatory=$false)][String]$buildNumber="alpha",
    [parameter(Mandatory=$false)][String]$configuration="Release"
)    

function Get-ScriptDirectory {
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

# setup path vars for use in the script below
$projectFolder   = "AuthCentral"
$scriptDir       = (Get-ScriptDirectory)
$srcRoot         = Join-Path -Path $scriptDir        -ChildPath "..\.." -Resolve
$projectDir      = Join-Path -Path $srcRoot          -ChildPath $projectFolder -Resolve
$publishParentDir= Join-Path -Path $projectDir       -ChildPath "..\..\" -Resolve
$publishDir      = Join-Path -Path $publishParentDir -ChildPath "pubroot"
$wwwrootDir      = Join-Path -Path $publishDir       -ChildPath "approot\src\$projectFolder\wwwroot"
$packageFilePath = Join-Path -Path $projectDir       -ChildPath "project.json" -Resolve
$nuspecFile      = Join-Path -Path $scriptDir        -ChildPath "AuthCentral.nuspec" -Resolve
$sevenZip        = Join-Path -Path $scriptDir        -ChildPath "7za.exe" -Resolve

" "
"========================================================================="
" Important Paths "
"========================================================================="
"scriptDir: $scriptDir"
"srcRoot: $srcRoot"
"projectDir: $projectDir"
"publishDir: $publishDir"
"wwwrootDir: $wwwrootDir"
"packageFilePath: $packageFilePath"
"nuspecFile: $nuspecFile"

# Restore Dependencies
. ($scriptDir + '.\Restore.ps1')

# fix up the package.json file to have a specific version number
# and commit hash for reference by out app health routes
" "
"Reading our version number and append the build number to it"
$project = Get-Content $packageFilePath | Out-String | ConvertFrom-Json

" "
"Setting build number to $buildNumber"
$version = $project.version -replace "\*", $buildNumber
$project.version = $version

" "
"Determining the current commit hash"
$gitCommit = [String](git rev-parse HEAD)
If(!$?) {
    "Warning: Unable to determine current commit hash.  Continuing with value 'unknown'."
    $gitCommit = "unknown"
}

# add the git commit to the project.json file
if (Get-Member -InputObject $project -Name commit -MemberType Properties) {
	# property exists so set it
	$project.commit = $gitCommit
}
else {
	# property does not exist so create it and set it
	Add-Member -InputObject $project -MemberType NoteProperty -Name "commit" -Value $gitCommit
}

If($createPackage) {
	"Adding git commit and version change back to project.json file"
	$project | ConvertTo-Json | Out-File $packageFilePath -Encoding ASCII
}

# build as sanity check
" "
"========================================================================="
" Building "
"========================================================================="
"# dnu build $projectDir --configuration $configuration"
dnu build $projectDir --configuration $configuration
If(!$?) { Exit 1 } 

# create the package, which we will zip up
" "
"========================================================================="
" Prep for Zipping "
"========================================================================="
"#dnu publish $projectDir --out $publishDir --runtime dnx-clr-win-x64.1.0.0-rc1-update1 --wwwroot-out $wwwrootDir --configuration $configuration"
dnu publish $projectDir --out $publishDir --runtime dnx-clr-win-x64.1.0.0-rc1-update1 --wwwroot-out $wwwrootDir --configuration $configuration
If(!$?) { Exit 1 }

# 7-zip the publish directory
" "
"========================================================================="
" Zipping "
"========================================================================="
"# $sevenZip a $(Join-Path -Path $scriptDir -ChildPath 'site.7z') $publishDir\*"
Invoke-Expression "$sevenZip a $(Join-Path -Path $scriptDir -ChildPath 'site.7z') $publishDir\*"


If($createPackage) {
#    "Setting the <element> w/in the app.nuspec file"
#    [xml]$nuspecXML = Get-Content $nuspecFile
#
#    #can set any element in the nuspec
#    $nuspecXML.package.metadata.version = $version
#
#    #need to save the changes
#    $nuspecXML.Save($nuspecFile)

  " "
  "========================================================================="
  " Packaging "
  "========================================================================="
	# Build our package - just a wrapper around the 7z file so we can use choco install
	# requires NuGet on the command line: Install-Package NuGet.CommandLine (may have to add Chocolatey\bin to path)
 	"# nuget pack $nuspecFile -Version $version -NoPackageAnalysis"
 	nuget pack $nuspecFile -Version $version -NoPackageAnalysis
    
  # Make sure building the package succeeded
  If(!$?) { Exit 1 }

	# This will find the build package
	$pkg = Get-ChildItem AuthCentral*.nupkg    

	# Write the envFile

  " "
  "========================================================================="
	" $envFile "
  "========================================================================="
	"app_name=" + $project.name | Out-File $envFile -Encoding ASCII
	"version=" + $version | Out-File $envFile -Encoding ASCII -Append
	"commit=" + $project.commit | Out-File $envFile -Encoding ASCII -Append
	"package_name=" +  $pkg.Name | Out-File $envFile -Encoding ASCII -Append

  # Print out the contents of the env file
  Get-Content $envFile
  ""

  If($publishPackage) {
    If((Test-Path env:CHOCO_FEED_API_KEY) -and (Test-Path env:CHOCO_FEED_URI)) {
      " "
      "========================================================================="
      " Publishing to Chocolatey Feed "
      "========================================================================="
      # publish Chocolatey package to proget
      nuget push $pkg.Name $env:CHOCO_FEED_API_KEY -Source $env:CHOCO_FEED_URI
    }
    Else {
      ""
      "Error: Unable to publish the package to NuGet.  Make sure CHOCO_FEED_API_KEY and CHOCO_FEED_URI have been defined as environment variables."
      ""
    }
  }
}

If(!$?) {
    Exit 1
}
