@echo off

set errorlevel=0

REM Installing dotnet-ef global tool
dotnet tool install --global dotnet-ef --version 8.0.2 --verbosity quiet

REM Changing working directory to .csproj directory
cd ..

SET "DEFAULT_HOST=localhost"
SET "DEFAULT_DATABASE=bloggy"
SET "DEFAULT_USER=bloggy"
SET "DEFAULT_PASSWORD=VeryStrong(!Passw0rd)"

SET "ctx=BloggyDbContext"

SET /P host="[1/4] Database host [default=%DEFAULT_HOST%]: "
IF "%host%"=="" SET host=%DEFAULT_HOST%

SET /P database="[2/4] Database name [default=%DEFAULT_DATABASE%]: "
IF "%database%"=="" SET database=%DEFAULT_DATABASE%

SET /P user="[3/4] Database user [default=%DEFAULT_USER%]: "
IF "%user%"=="" SET user=%DEFAULT_USER%

SET /P password="[4/4] Database user password [default=%DEFAULT_PASSWORD%]: "
IF "%password%"=="" SET password=%DEFAULT_PASSWORD%

dotnet ef migrations remove -c "%ctx%" -- "Server=%host%;Database=%database%;User Id=%user%;Password=%password%;Trusted_Connection=True;TrustServerCertificate=True;"
