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

	$gmsPath = "{0}\lib\{1}\{2}" -f $installPath, $scVersion, "Glass.Mapper.Sc.ContentSearch.dll";
	
	RemovingExisting "Glass.Mapper.Sc.ContentSearch";
	AddReference $gmsPath "Glass.Mapper.Sc.ContentSearch";

	$gmsPath = "{0}\lib\{1}\{2}" -f $installPath, $scVersion, "Glass.Mapper.Sc.ContentSearch.LuceneProvider.dll";
	
	RemovingExisting "Glass.Mapper.Sc.ContentSearch.LuceneProvider";
	AddReference $gmsPath "Glass.Mapper.Sc.ContentSearch.LuceneProvider";

}
else{
	Write-Host "ERROR: Could not locate Sitecore.Kernel.dll, please add reference before installing Glass.Mapper.Sc.ContentSearch.LuceneProvider";
}


Write-Host "Installation complete";
	