param($installPath, $toolsPath, $package, $project)




Write-Host "Check Sitecore Version"

$scKernel = $project.Object.References.Item("Sitecore.Kernel");

if($scKernel){	
	$scKernelPath = $scKernel.Path

	Write-Host "Sitecore.Kernel Path: "$scKernelPath;

	$scFileVersion = (Get-item $scKernelPath).VersionInfo.FileVersion;

	Write-Host "Check Sitecore File Version is "$scFileVersion;


	$scVerion = [double]::Parse([string]::Format("{0}.{1}",$scFileVersion.Split(".")[0], $scFileVersion.Split(".")[1]));
	$gmsPath = "";

	if($scVersion -ge 8.0){
		$gmsPath = $installPath +"\libs\80\"
	}
	elseif($scVersion -ge 7.5){
		$gmsPath = $installPath +"\libs\75\"
	}
	elseif($scVersion -ge 7.2){
		$gmsPath = $installPath +"\libs\72\"
	}
	elseif($scVersion -ge 7.1){
		$gmsPath = $installPath +"\libs\71\"
	}
	else{ 
		$gmsPath = $installPath +"\libs\70\"
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
	AddReference("Glass.Mapper.Sc");
	
}
else{
	Write-Host "Could not locate Sitecore.Kernel.dll, please add reference before installing Glass.Mapper.Sc";
}


