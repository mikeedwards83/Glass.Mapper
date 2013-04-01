param($target = "../../", $ending = "CRE", $extension =".cs")

$header = "/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the ""License"");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an ""AS IS"" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-"

function Write-Header ($file)
{
	Write-Output("start"+$file);
	
    $content = [Io.File]::ReadAllText($file)

	$index =  $content.IndexOf($ending);
	
	if($index -gt 0){
		$index  = $index +$ending.Length+2;
	}
	else{
		$index = 0;
	}
		$content = $content.Substring($index);

    $filename = Split-Path -Leaf $file

    $fileheader = $header -f $filename

   Set-Content $file $fileheader

    Add-Content $file $content

	Write-Output("end"+$file);

}

Get-ChildItem $target -Recurse | ? { $_.Extension -like $extension } | % `
{
    Write-Header $_.PSPath.Split(":", 3)[2]
}