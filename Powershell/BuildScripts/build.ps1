[CmdletBinding()]
Param(
   [Parameter(Mandatory=$True,Position=1)]
   [string]$releaseNumber,  
   [Parameter(Position=2)]
   [string]$nugetKey,
   [switch]$clean
)

$config = @{
 
       
}

$rootPath = ".\..\..";
$releasePath = $rootPath+"\Releases\"+$releaseNumber
$nugetsPath = $rootPath + "\Nugets\"+$releaseNumber
$env:windir
$logfile = $releasePath+"\Build.log"
$nugetExe = $releasePath+"\Nuget.exe"


Function LogWrite
{
   Param ([string]$logstring)
   Write-Host $logstring

   $logExists = Test-Path $logfile
   if($logExists -eq $false){
        New-Item $logfile -type file
   }
   Add-content $logfile -value $logstring
}

Function YesNoPrompt{
    Param([string] $caption,
          [string] $message)


    $yes = new-Object System.Management.Automation.Host.ChoiceDescription "&Yes","help";
    $no = new-Object System.Management.Automation.Host.ChoiceDescription "&No","help";
    $choices = [System.Management.Automation.Host.ChoiceDescription[]]($no,$yes);
    $answer = $host.ui.PromptForChoice($caption,$message,$choices,0)

    return $answer
}


#Copy everything to the release folder

if($clean)
{
    LogWrite "Removing directories"
    Remove-Item $releasePath -Force -Recurse
    Remove-Item $nugetsPath -Force -Recurse
}

if((Test-Path $releasePath) -or (Test-Path $nugetsPath)){
    LogWrite "Release already exists - aborting";
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
Copy-Item "build.proj" $releasePath
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
$build = $msbuild + $releasePath+"\build.proj"

Invoke-Expression $build


#create nuget packages

LogWrite "****** Creating Nuget Packages ******"

$nugets = Get-ChildItem -Path $releasePath -Filter *.nuspec | ForEach-Object -Process {$_.FullName}

foreach($nuget  in $nugets){
    LogWrite $nuget
    [xml] $nugetContent = Get-Content $nuget
    $nugetContent.package.metadata.version = $releaseNumber
    $nugetContent.Save($nuget)
	
    $nugetCmd = $nugetExe + " pack "+$nuget +" -Verbosity detailed -Version "+ $releaseNumber + " -OutputDirectory "+$nugetsPath
    
    LogWrite $nugetCmd
    Invoke-Expression $nugetCmd
}

LogWrite "****** Pushing Nuget Packages ******"

if($nugetKey){
    $nugetPackages = Get-ChildItem -Path $nugetsPath -Filter *.nupkg

    $nugetApiSet = $nugetExe+ " setApiKey " + $nugetKey
    Invoke-Expression $nugetApiSet
    
    foreach($nugetPackage  in $nugetPackages){
        $message = "Do you want to push "+$nugetPackage.FullName
        $result = YesNoPrompt "Push package" $message
        LogWrite $nugetPackage
        switch($result){
            0{ 
                LogWrite("File skipped")
             }
            1{
                $nugetPush = $nugetExe + " push "+$nugetPackage.FullName
                LogWrite $nugetPush
                Invoke-Expression $nugetPush
                LogWrite("File pushed")
            }
        }
    }
}
else{
    LogWrite "No nuget key, push skipped";
}



