param($installPath, $toolsPath, $package, $project)


Write-Host "Check Sitecore Version"

$scKernel = $project.Object.References.Item('Sitecore.Kernel').FullPath

$scFileVersion = (Get-item $scKernel).VersionInfo.FileVersion;

Write-Host "Check Sitecore File Version is "+$scFileVersion;


$scVerion = [double]::Parse([string]::Format("{0}.{1}",$scFileVersion.Split(".")[0], $scFileVersion.Split(".")[1]));
$gmsPath = String.Empty;

if($scVersion -ge 7.5){
	$gmsPath = $installPath +"\libs\sc75\Glass.Mapper.Sc.dll"
}
else{ 
	$gmsPath = $installPath +"\libs\sc75\Glass.Mapper.Sc.dll"
}

$existing = $project.Object.References.Item("Glass.Mapper.Sc");
if(existing -ne null){
    Write-Host "Removing existing Glass.Mapper.Sc reference";
	$existing.Remove();
}

Write-Host "Adding reference to "+$gmsPath;

$project.Object.References.Add($gmsPath);
$project.Object.References.Item("Glass.Mapper.Sc").CopyLocal = "True"