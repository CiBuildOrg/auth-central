try {

    # Read in the install args file
    $installArgsFile = Join-Path -Path $env:chocolateyPackageFolder -ChildPath "installArgs.json"
    if (Test-Path $installArgsFile) {
        "Reading in the $installArgsFile to get access to the initial installation arguments"
        $installArgs = Get-Content $installArgsFile | Out-String | ConvertFrom-Json
    }

	# Stop and remove the service if it's already installed
	if (Get-Service $packageName -ErrorAction SilentlyContinue)
	{
        ""
	    "Stopping the $packageName Service..."
		Stop-Service $packageName

        ""
        "Removing the $packageName service..."
		sc.exe delete $packageName
	}
    else
    {
      ""
      "Service $packageName was not found!"
    }

    ############################################################
    # File & Directory Cleanup
    ############################################################

    # Remove the linked directory if it exists
    If ((Test-Path $installArgs.linkPath) -and (Test-Path -LiteralPath $installArgs.linkPath) ) 
	{ 
		"Removing symbolic link"
		[System.IO.Directory]::Delete($installArgs.linkPath, $true)
    }

    # Removed the installed app directory if it exists
    if ((Test-Path $installArgs.installPath) -and (Test-Path $installArgs.installPath) ) {
        "Removing installation directory"
        Remove-Item $installArgs.installPath -Recurse -Force
    }

    ############################################################

    ""
    "Done!"
}
Catch {
   throw $_.Exception
}

