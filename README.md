# WafClast RPG

A RPG game for Discord.

## Installation

You must have [.net core](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed in your machine


## Build

#### Linux:

```bash
make build
```

#### Windows:
```bash
dotnet restore --configfile Nuget.config

dotnet build
```

## Publish

#### Linux:

```shell
make publish
```

#### Windows:
```shell
dotnet publish -c Release -r win10-x64 --self-contained true -o published
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)