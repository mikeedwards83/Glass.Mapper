param($installPath, $toolsPath, $package, $project)

Function GetVersion{
	params($name);

	Write-Host "Check Version of "$name;
	$item = $project.Object.References.Item($name);


	if($item){	
		$itemPath = $item.Path

		Write-Host "{0} Path: {1}" -f $name, $itemPath;

		$fileVersion = (Get-item $scKernelPath).VersionInfo.FileVersion;

		Write-Host "Check {0} File Version is {1}" -f $name, $fileVersion;
	
		$versionString = "{0}.{1}" -f $fileVersion.Split(".")[0], $fileVersion.Split(".")[1];

		$version = [double]::Parse($versionString);

		Write-Host "Parsed Version for {0} is {1}" -f $name, $version;
		
		return $version;
	}


	return -1;
}

$scVerion = GetVersion "Sitecore.Kernel";

if($scVerion -ge -1){	

	$gmsPath = "";

	Write-Host "Checking version "$scVerion;

	if($scVersion -ge 8.0){
		$gmsPath = $installPath +"\lib\80\"
		Write-Host "Checking version in at 8";
	}
	elseif($scVersion -ge 7.5){
		$gmsPath = $installPath +"\lib\75\"
	}
	elseif($scVersion -ge 7.2){
		$gmsPath = $installPath +"\lib\72\"
	}
	elseif($scVersion -ge 7.1){
		$gmsPath = $installPath +"\lib\71\"
	}
	else{ 
		$gmsPath = $installPath +"\lib\70\"
	}


	Function RemovingExisting{
		param($name);

		$existing =  $project.Object.References.Item($name);

		if($existing){
			Write-Host "Removing existing "$name;
			$existing.Remove();
		}
    }

	Function AddReference{
		param($name);


		$finalPath= "{0}{1}" -f $gmsPath,$name;

		Write-Host "Adding reference to "$finalPath;

		$project.Object.References.Add($finalPath);
		$project.Object.References.Item("Glass.Mapper.Sc").CopyLocal = "True"
    }

	RemovingExisting("Glass.Mapper.Sc");
	AddReference("Glass.Mapper.Sc.dll");
	
}
else{
	Write-Host "Could not locate Sitecore.Kernel.dll, please add reference before installing Glass.Mapper.Sc";
}


