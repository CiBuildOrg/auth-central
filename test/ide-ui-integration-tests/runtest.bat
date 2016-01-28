@echo off

set URL=https://secure.dev-fsw.com

IF /I %1 == stage (
	set URL=https://secure.stg-fsw.com
) ELSE IF /I %1 == prod ( 
	set URL=https://secure.fsw.com
)

echo Root URL set to: %URL%

java.exe -jar C:\Jenkins\workspace\auth_central_deploy_smoke_tests\test\ide-ui-integration-tests\selenium-server-standalone-2.48.2.jar -htmlSuite "*firefox" "%URL%" "C:\Jenkins\workspace\auth_central_deploy_smoke_tests\test\ide-ui-integration-tests\Architecture_UITests.html" "C:\Jenkins\workspace\auth_central_deploy_smoke_tests\test\ide-ui-integration-tests\Logs\IdeTestResults.html" -port 5555