@echo off

set URL=https://secure.dev-fsw.com

IF /I %1 == stage (
	set URL=https://secure.stg-fsw.com
) ELSE IF /I %1 == prod ( 
	set URL=https://secure.fsw.com
)

echo Root URL set to: %URL%

java.exe -jar .\selenium-server-standalone-2.48.2.jar -htmlSuite "*firefox" "%URL%" ".\MasterSuite.html" ".\Logs\IdeTestResults.html" -port 5555
rem java.exe -jar .\selenium-server-standalone-2.48.2.jar -htmlSuite "*googlechrome" "https://secure.dev-fsw.com" ".\Architecture_UITests.html" ".\Logs\IdeTestResults.html" -port 5555
pause