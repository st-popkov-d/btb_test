@echo off

set errorlevel=0

REM Installing dotnet-ef global tool
dotnet tool install --global dotnet-ef --version 8.0.2 --verbosity quiet
dotnet tool update --global dotnet-ef --version 8.0.2 --verbosity quiet

REM Changing working directory to .csproj directory
cd ..

SET "DEFAULT_MIGRATION_NAME=Initial"
SET "DEFAULT_HOST=localhost"
SET "DEFAULT_DATABASE=bloggy"
SET "DEFAULT_USER=bloggy"
SET "DEFAULT_PASSWORD=VeryStrong(!Passw0rd)"

SET "ctx=BloggyDbContext"
SET "dst=Migrations"

SET /P name="[1/5] Enter migration name [default=%DEFAULT_MIGRATION_NAME%]: "
IF "%name%"=="" SET name=%DEFAULT_MIGRATION_NAME%

SET /P host="[2/5] Database host [default=%DEFAULT_HOST%]: "
IF "%host%"=="" SET host=%DEFAULT_HOST%

SET /P database="[3/5] Database name [default=%DEFAULT_DATABASE%]: "
IF "%database%"=="" SET database=%DEFAULT_DATABASE%

SET /P user="[4/5] Database user [default=%DEFAULT_USER%]: "
IF "%user%"=="" SET user=%DEFAULT_USER%

SET /P password="[5/5] Database user password [default=%DEFAULT_PASSWORD%]: "
IF "%password%"=="" SET password=%DEFAULT_PASSWORD%

dotnet ef migrations add "%name%" -c "%ctx%" -o "%dst%" -- "Server=%host%;Database=%database%;User Id=%user%;Password=%password%;Trusted_Connection=True;TrustServerCertificate=True;"

