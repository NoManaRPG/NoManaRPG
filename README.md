# WafClast RPG

A RPG game for Discord.

## 🚀 Selfhosting WafClastRPG (Discord)

### 📜 Selfhosting conditions and warnings
If you are planning to selfhost WafClastRPG, here are some stuff that you should keep in mind...
1. We keep the source code open so people can see, learn and be inspired by how WafClastRPG was made and, if they want to, they can help the project with features and bug fixes.
2. This is a community project, if you make changes to WafClastRPG's source code you need to follow the [AGPL-3.0](LICENSE) and keep the changes open source! And, if you want to help WafClastRPG, why not create a pull request? 😉
3. We **won't** give support for selfhosted instances, you need to know how to troubleshoot the issues yourself. We tried to make the selfhost process as painless as possible, but it is impossible to know all the different issues you may find.
4. Don't lie saying that you "created WafClastRPG". Please give credits to the creators!
5. WafClastRPG requires a lot of different API keys for a lot of features. While they aren't required, you may encounter issues when trying to use some of the features.
6. WafClastRPG's assets (fonts, images, etc) aren't not distributed with the source code, you will need to create and include your own assets.
7. We use Ubuntu 18.04 to run her, she may work on other Linux operating systems or even in Windows, but we recommend hosting her on Ubuntu!
8. To avoid problems and confusions, we **do not allow** using the name "WafClastRPG", "ClastRPG" or any similar names on your selfhosted versions. Call her "Nii RPG" if you aren't creative enough to create your own name to give to your selfhosted version. Don't like "Nii RPG"? Generate your own [here](https://www.behindthename.com/random/)!

Seems too hard but you *really* want to use WafClastRPG? Don't worry, you can use our free public instance by clicking here [clicking here](https://discord.gg/MAR4NFq)!


### 👷 Prerequisites

* PowerShell (Windows) or Terminal (Linux).
> ⚠️ While Windows' command prompt may work, it is better to use PowerShell!
* You need to have the [.net core](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed on your machine.
* You need to have Git installed on your machine.
* You need to have [Mongo](https://www.mongodb.com/) installed on you machine.
* If you want to help to develop WafClastRPG, or if you only want a good C# IDE, then download [Visual Studio IDE](https://visualstudio.microsoft.com/pt-br/)! The community edition is enough, so you don't need to be like "oh my god I need to *pay* for it". 😉

### 🧹 Preparing the environment
* Clone the repository with git:
```bash
git clone https://github.com/TalionOak/WafclastRPG.git
```

### 💻 Compiling
* Go inside the source code folder and open PowerShell or the terminal inside of it.

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
Update the configurations with your own values. You don't need to configure everything, just the bare minimum (bot token, folders, databases, etc) to get her up and running!

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[GNU AGPLv3](https://choosealicense.com/licenses/agpl-3.0/)
