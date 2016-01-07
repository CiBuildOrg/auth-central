" "
"========================================================================="
" Setting .NET Version "
"========================================================================="
"# dnvm use 1.0.0-rc1-update1"
dnvm use 1.0.0-rc1-update1 -arch x64

" "
"========================================================================="
" Restoring Dependencies "
"========================================================================="
"# dnu restore $projectDir --no-cache --source https://www.myget.org/F/identity/ --source https://www.nuget.org/api/v2/ --source http://proget.fsw.com/nuget/Default"
dnu restore $projectDir --no-cache --source https://www.myget.org/F/identity/ --source https://www.nuget.org/api/v2/ --source http://proget.fsw.com/nuget/Default
If(!$?) { Exit 1 } 

