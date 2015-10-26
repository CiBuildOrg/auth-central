try {

    $arguments = @{};
    $packageParams = $env:chocolateyPackageParameters;
	$envVarsFile = $null;

	$serviceName = $packageName;
	$serviceDisplayName = $packageName;
	$serviceUser = $null;
	$servicePassword = $null;

    $applicationPath = 'C:\Apps';
	
    # set a default $installRoot
    $installRoot = Join-Path -Path $applicationPath -ChildPath "Web";
	

    # check for parameters provided to the script as part of the choco install process
    if($packageParams)
    {
      $MATCH_PATTERN = "/([a-zA-Z]+):([`"'])?(\w:)?([a-zA-Z0-9 _\\\.-]+)([`"'])?";
      $PARAMATER_NAME_INDEX = 1;
      $VALUE_INDEX = 4;
      $DRIVE_INDEX = 3;

      if($packageParams -match $MATCH_PATTERN )
      {
        $results = $packageParams | Select-String $MATCH_PATTERN -AllMatches
        $results.Matches | % {
          $arguments.Add(
          $_.Groups[$PARAMATER_NAME_INDEX].Value.Trim(),
          $_.Groups[$DRIVE_INDEX].Value.Trim() + $_.Groups[$VALUE_INDEX].Value.Trim())
        }
      }

      if($arguments.ContainsKey("InstallPath"))
      {
        $installRoot = $arguments["InstallRoot"]
      }

      if($arguments.ContainsKey("EnvVarsFile"))
      {
        $envVarsFile = $arguments["EnvVarsFile"]
      }

      if($arguments.ContainsKey("ServiceName"))
      {
        $serviceName = $arguments["ServiceName"]
      }

      if($arguments.ContainsKey("ServiceUser"))
      {
        $serviceUser = $arguments["ServiceUser"]
      }

      if($arguments.ContainsKey("ServicePassword"))
      {
        $servicePassword = $arguments["ServicePassword"]
      }

      if($arguments.ContainsKey("ServiceDisplayName"))
      {
        $serviceDisplayName = $arguments["ServiceDisplayName"]
      }

    }
    else
    {
      Write-Host "No package parameters were provided.";
    }

	Write-Host ""
    Write-Host "======================================================";
    Write-Host "Install Root set to: $installRoot";
    Write-Host "Service name set to: $serviceName";
    Write-Host "Service display name set to: $serviceDisplayName";
	if ($envVarsFile) {
		Write-Host "Environment File set to: $envVarsFile";
	}
	else {
		Write-Host "Environment File set to: <empty>";
	}	
	if ($serviceUser) {
		Write-Host "Service user set to: $serviceUser";
	}
	else {
		Write-Host "Service user set to: <empty>";
	}
	if ($servicePassword) {
		Write-Host "Service password set to (hidden): ********";
	}
	else {
		Write-Host "Service password set to: <empty>";
	}
    Write-Host "======================================================";
	Write-Host ""


    # make sure $linkPath exists
    $linkPath = Join-Path -Path $installRoot -ChildPath "$env:chocolateyPackageName"
    New-Item -Force -ItemType directory -Path $linkPath
	"linkPath: $linkPath"

    # make $installPath a versioned path, we will move content here
    $installPath = Join-Path -Path $installRoot -ChildPath "$env:chocolateyPackageName.$env:chocolateyPackageVersion"

	# Stop and remove the service if it's already installed
	if (Get-Service $serviceName -ErrorAction SilentlyContinue)
	{
		Stop-Service $serviceName
		Invoke-Expression "sc.exe delete $serviceName"
	}


    ##############################################################################
    # Extract the package to install dir
    ##############################################################################

    $appZip = Join-Path -Path $env:chocolateyPackageFolder -ChildPath "site\site.7z"

    "Extracting $appZip zip to $installPath"
    Invoke-Expression "$env:chocolateyPackageFolder\tools\7za.exe -y x -o$installPath $appZip"

    if ($LASTEXITCODE -ne 0) {
	    Write-Error "Failed to unpack $serviceName"
	    Exit 1
    }
    
    "Coping nssm.exe to the app directory"
    Copy-Item $env:chocolateyPackageFolder\tools\nssm.exe $installPath

    ##############################################################################


	# We use linked directories to keep track of installed apps
    If (Test-Path -LiteralPath $linkPath ) 
	{ 
		"Removing old link"
		[System.IO.Directory]::Delete($linkPath, $true)
	}

	Write-Host "Creating new link"
    cmd /c mklink /j $linkPath $installPath

	# keep only the most recent X packages
	$NumberToSave=3
	$items = Get-ChildItem "$installRoot" |
		Where-Object Name -match "$packageName.*" |
		Sort-Object LastWriteTime -Descending

	if ($NumberToSave -lt $items.Count)
	{
		$items[$NumberToSave..($items.Count-1)] | Remove-Item -Recurse -Confirm:$false
	}

	# Move nssm and create a path var for it
	$nssm = Join-Path -Path $env:chocolateyPackageFolder -ChildPath "\tools\nssm.exe";
    Move-Item -path $nssm -destination $installPath -force
	$nssm = Join-Path -Path $installPath -ChildPath "nssm.exe";

	# Install the self hosted web app as a windows service
	Invoke-Expression "$nssm install $serviceName $linkPath\approot\host.cmd"
	if ($LASTEXITCODE -ne 0)
	{
		Write-Error "Failed to install $serviceDisplayName!"
		Exit 1
	}


    #############################################################################
    # write a file containing all the non-senstive install params
    #############################################################################

    $installArgsFile = Join-Path -Path $env:chocolateyPackageFolder -ChildPath "installArgs.json"
    $installArgs = New-Object PSObject -Property @{ 
        installPath =  $installPath
        linkPath    = $linkPath
        envVarsFile = $envVarsFile
        serviceName = $serviceName
        serviceUser = $serviceUser
        serviceDisplayName = $serviceDisplayName
    }

    $installArgs | ConvertTo-Json | Out-File $installArgsFile

    #############################################################################


	# If provided -- Set the user to run the service as
	if ($serviceUser -AND $servicePassword) {
		Write-Error "Setting service user."

		# Set the user to run the service as.  Ansible should have created this user during the server provisioning
		Invoke-Expression "$nssm set $serviceName ObjectName .\$serviceUser $servicePassword"
		if ($LASTEXITCODE -ne 0)
		{
			Write-Error "Failed to set service user"
			Exit 1
		}
	}

	# Set environment variables for the windows service
	if($envVarsFile) {
        $appEnvironmentExtra = $null
	    $ok_pattern = "(\w+)=(.*)";

		# loop through each line of the $envVarsFile file
		# building the AppEnvironmentExtra parameter to 
		# pass to nssm
		Get-Content $envVarsFile | ForEach-Object { 
			if($_ -match $ok_pattern ) {
                $appEnvironmentExtra += "$_ "
			}
		}

	    if ($appEnvironmentExtra) {
            Invoke-Expression "$nssm set $serviceName AppEnvironmentExtra $appEnvironmentExtra"

		    if ($LASTEXITCODE -ne 0)
		    {
			    Write-Error "Failed to set environment block for AppEnvrionmentExtra parameter."
			    Exit 1
		    }
	    }
	}

	# Set service display name
	Invoke-Expression "$nssm set $serviceName DisplayName $serviceDisplayName"

    # Use IO Redirect to save stdout and stderr to a log file
    New-Item -Force -ItemType directory -Path $linkPath\logs
    Invoke-Expression "$nssm set $serviceName AppStdout $linkPath\logs\console.log"
    Invoke-Expression "$nssm set $serviceName AppStderr $linkPath\logs\console.log"

    # Set File Rotation for cosole.log (10mb max log size)
    Invoke-Expression "$nssm set $serviceName AppStdoutCreationDisposition 4"
    Invoke-Expression "$nssm set $serviceName AppStderrCreationDisposition 4"
    Invoke-Expression "$nssm set $serviceName AppRotateFiles 1"
    Invoke-Expression "$nssm set $serviceName AppRotateOnline 1"
    Invoke-Expression "$nssm set $serviceName AppRotateBytes 1048576"

	# Start service
	sc.exe start $serviceName
}
Catch {
   throw $_.Exception
}

