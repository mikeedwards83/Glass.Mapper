param($installPath, $toolsPath, $package, $project)

	Function RemovingExisting{
		param($name);

		$existing =  $project.Object.References.Item($name);

		if($existing){
			Write-Host "Removing existing "$name;
			$existing.Remove();
		}
    }

	
	RemovingExisting("Glass.Mapper.Sc.ContentSearch.SolrProvider");
	RemovingExisting("Glass.Mapper.Sc.ContentSearch");
	
	


