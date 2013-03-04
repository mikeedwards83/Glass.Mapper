<#
           .SYNOPSIS 
           Automatically builds script for Glass.Mapper.

           .DESCRIPTION
           Automatically builds Glass.Mapper creating debug and release builds. It when then push the Nuget
           packages to Nuget.org and SymbolSource.org.
    
           .PARAMETER releaseNumber
           The release number to create. Release numbers in each assembly will automatically be updated.

           .PARAMETER nugetKey
           Nuget API key, required if you want push a Nuget packages to the Nuget server. If missing Nuget packages
           are built locally only

           .PARAMETER clean
           Removes any previous Nuget or Release folders with the same release number.

           .PARAMETER tidy
           Removes the specific Release folder after the build has completed.

           .PARAMETER silent
           Does not prompt you for permission to push packages to Nuget.

           .INPUTS
           None. You cannot pipe objects to build.ps1.

           .OUTPUTS
           None. build.ps1 does not generate any output.

           .EXAMPLE
           C:\PS> .\build.ps1 0.0.0.4 1dd320ca-t59s-789d-897c-190458dS4a5 -tidy

           
           #>

[CmdletBinding()]
Param(
   [Parameter(Mandatory=$True,Position=1)]
   [string]$releaseNumber,  
   [Parameter(Position=2)]
   [string]$nugetKey,
   [switch]$clean,
   [switch]$tidy,
   [switch]$silent
)




$config = @{
 
       
}

$rootPath = ".\..\..";
$releasePath = $rootPath+"\Releases\"+$releaseNumber
$nugetsPath = $rootPath + "\Nugets\"+$releaseNumber
$env:windir
$logfile = $releasePath+"\Build.log"
$nugetExe = $releasePath+"\Nuget.exe"


. .\utilities.ps1


#Copy everything to the release folder

if($clean)
{
    LogWrite "Removing directories"
   
    if(Test-Path $releasePath){
        Remove-Item $releasePath -Force -Recurse
    }

    if(Test-Path $nugetsPath){
         Remove-Item $nugetsPath -Force -Recurse
    }
}

if((Test-Path $releasePath) -or (Test-Path $nugetsPath)){
    Write-Host "Release already exists - aborting";
    return
}

#create release folder
New-Item $releasePath -type directory
New-Item $nugetsPath -type directory

#copy all source
LogWrite "******* Copying Files ******"

Copy-Item $rootPath"\Packages" $releasePath"\Packages" -recurse
Copy-Item $rootPath"\Depends" $releasePath"\Depends" -recurse
Copy-Item $rootPath"\Source" $releasePath"\Source" -recurse
Copy-Item $rootPath"\Tests" $releasePath"\Tests" -recurse
Copy-Item $rootPath"\*.sln" $releasePath
Copy-Item "*.proj" $releasePath
Copy-Item "*.nuspec" $releasePath
Copy-Item "Nuget.exe" $releasePath


#update the version numbers

LogWrite "****** Updating version numbers ******"
 
$assInfos = Get-ChildItem -Path $releasePath -Filter AssemblyInfo.cs -Recurse | ForEach-Object -Process {$_.FullName}

$newAssVersion = 'AssemblyVersion("'+$releaseNumber+'")'
$newFileVersion = 'AssemblyFileVersion("'+$releaseNumber+'")'
$copyright = 'AssemblyCopyright("Copyright Michael Edwards  2012")'

foreach($assInfo  in $assInfos){
    LogWrite $assInfo
    $content = Get-Content $assInfo | Out-String
    $content = $content -replace 'AssemblyVersion\("[\d\.]*"\)', $newAssVersion
    $content = $content -replace 'AssemblyFileVersion\("[\d\.]*"\)', $newFileVersion
    $content = $content -replace 'AssemblyCopyright\("[^"]*"\)', $copyright
    $content | Out-File $assInfo 
}



#build the solution"
$msbuild = $env:windir+"\Microsoft.NET\Framework\v4.0.30319\msbuild "
$releaseBuild = $msbuild + $releasePath+"\build-release.proj"
$debugBuild = $msbuild + $releasePath+"\build-debug.proj"

Invoke-Expression $releaseBuild
Invoke-Expression $debugBuild


#create nuget packages

LogWrite "****** Creating Nuget Packages ******"

$nugets = Get-ChildItem -Path $releasePath -Filter *.nuspec | ForEach-Object -Process {$_.FullName}

foreach($nuget  in $nugets){
    LogWrite $nuget
 
 #Removed, using $version$ in Nuget files
 #   [xml] $nugetContent = Get-Content $nuget
 #   $nugetContent.package.metadata.version = $releaseNumber
 #   $nugetContent.Save($nuget)
	
    $nugetCmd = $nugetExe + " pack "+$nuget +" -Symbols -Verbosity detailed -Version "+ $releaseNumber + " -OutputDirectory "+$nugetsPath
    
    LogWrite $nugetCmd
    Invoke-Expression $nugetCmd
}

LogWrite "****** Pushing Nuget Packages ******"


if($nugetKey){
    $nugetPackages = Get-ChildItem -Path $nugetsPath -Filter *.$releaseNumber.nupkg
    
    foreach($nugetPackage  in $nugetPackages){
       
	    if($silent){
            NugetPush $nugetPackage.FullName $nugetKey
		}
		else{
			$message = "Do you want to push "+$nugetPackage.FullName
			$result = YesNoPrompt "Push package" $message
      
			switch($result){
				0{ 
					LogWrite("File skipped")
				 }
				1{
					NugetPush $nugetPackage.FullName $nugetKey
				}
			}
		}
    }
}
else{
    LogWrite "No nuget key, push skipped";
}

if($tidy)
{
    LogWrite "Removing release directory"
    Remove-Item $releasePath -Force -Recurse
}