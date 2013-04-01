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

Function NugetPush{
    Param(
        [string] $path,
        [string] $apiKey
    )


    $nugetApiSet = $nugetExe+ " setApiKey " + $apiKey
    Invoke-Expression $nugetApiSet

    $nugetPush = $nugetExe + " push "+$path
    LogWrite $nugetPush
    Invoke-Expression $nugetPush
    LogWrite("File pushed")
}