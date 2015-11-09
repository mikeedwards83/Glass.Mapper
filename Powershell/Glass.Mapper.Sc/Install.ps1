param($installPath, $toolsPath, $package, $project)

Function GetVersion{
	param($name, $length);

	Write-Host "Check Version of "$name;
	$item = $project.Object.References.Item($name);

	if($item){	
		$itemPath = $item.Path

		Write-Host ("{0} Path: {1}" -f $name, $itemPath);

		$fileVersion = (Get-item $itemPath).VersionInfo.FileVersion;

		Write-Host ("Check {0} File Version is {1}" -f $name, $fileVersion);
	
		if($length -eq 2){
			return $versionString = "{0}{1}" -f $fileVersion.Split(".")[0], $fileVersion.Split(".")[1];
		}
		else{
			return $fileVersion.Split(".")[0];
		}
	}

	return $null;
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
		param($path, $name);
	
		Write-Host "Adding reference to "$path;

		$project.Object.References.Add($path);
		$project.Object.References.Item($name).CopyLocal = "True"
    }


$scVersion = GetVersion "Sitecore.Kernel" 2;

if($scVersion){	

	$gmsPath = "{0}\lib\{1}\{2}" -f $installPath, $scVersion, "Glass.Mapper.Sc.dll";
	
	RemovingExisting "Glass.Mapper.Sc";
	AddReference $gmsPath "Glass.Mapper.Sc";

	$mvcVersion = GetVersion "System.Web.Mvc" 2;
	
	if($mvcVersion){

		$mvcPath = "{0}\lib\Mvc{1}\{2}" -f $installPath, $mvcVersion, "Glass.Mapper.Sc.Mvc.dll";
		Write-Host ("Mvc Path 1 {0}" -f $mvcPath);
		
		$mvcExists = Test-Path $mvcPath

		if($mvcExists -eq $false ){
			$mvcVersion = GetVersion "System.Web.Mvc" 1;
			$mvcPath = ("{0}\lib\Mvc{1}\{2}" -f $installPath, $mvcVersion, "Glass.Mapper.Sc.Mvc.dll");
			Write-Host ("Mvc Path 2 {0}" -f $mvcPath);

		}

		Write-Host ("MVC Final Path {0}" -f $mvcPath);

		RemovingExisting "Glass.Mapper.Sc.Mvc" ;
		AddReference $mvcPath "Glass.Mapper.Sc.Mvc";

	}
	else{
		Write-Host "WARNING: Could not locate System.Web.Mvc.dll, cannot add reference to Glass.Mapper.Sc.Mvc";

	}

}
else{
	Write-Host "ERROR: Could not locate Sitecore.Kernel.dll, please add reference before installing Glass.Mapper.Sc";
}


Write-Host "Installation complete";
	