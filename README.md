# Prohelika.Template.CleanArchitecture

-----------------

### Clean Architecture Solution Template for .NET

-----------------

## How to use?

---------------------

### 1. Installation

---------------------
1. Clone the repository
2. Run `dotnet new .` from the repository root
3. Verify the installation by running `dotnet new list`

### 2. Usage

---------------------
1. To create a new project run `dotnet new pro-clean-arch -o <project-name>`
2. Adjust any settings in `appsettings.json` or `appsettings.Development.json`
3. Make necessary changes
4. Run `dotnet build` or `dotnet run`

### 3. Deployment

---------------------
#### Deploy to Docker
1. Make sure you have Docker installed
2. cd into the `build` directory
3. Make necessary changes to `docker-compose.yml` or `docker-compose-prod.yml` file
4. Run `chmod +x manage.sh` to make `./manage.sh` executable
5. Run `./manage.sh` and follow the instructions
6. Run `docker ps` to check if the containers are running


-----------------

## Contributing

-----------------
Feel free to contribute to the project by feeding issues and PRs at https://github.com/ProhelikaX/Prohelika.Template.CleanArchitecture
