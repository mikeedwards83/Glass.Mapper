[CmdletBinding()]
Param(
   [Parameter(Mandatory=$True,Position=1)]
   [string]$releaseNumber   
)

$config = @{
 
       
}




$rootPath = ".\..\..";
$releasePath = $rootPath+"\Releases\"+$releaseNumber
$env:windir
$logfile = $releasePath+"\Build.log"


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


#Copy everything to the release folder

if(Test-Path $releasePath){
    LogWrite "Release already exists - aborting";
    return
}

#create release folder
New-Item $releasePath -type directory

#copy all source
LogWrite "******* Copying Files ******"

Copy-Item $rootPath"\Packages" $releasePath"\Packages" -recurse
Copy-Item $rootPath"\Depends" $releasePath"\Depends" -recurse
Copy-Item $rootPath"\Source" $releasePath"\Source" -recurse
Copy-Item $rootPath"\Tests" $releasePath"\Tests" -recurse
Copy-Item $rootPath"\*.sln" $releasePath
Copy-Item "build.proj" $releasePath


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



