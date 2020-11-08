restore:
	dotnet restore --configfile Nuget.config

build: restore
	dotnet build

publish:
	dotnet publish -c Release -r win10-x64 --self-contained true -o published