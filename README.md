# [.NET Core Web API Starter](https://github.com/dalbitresb12/netcore-webapi-starter)

This [template](https://github.com/dalbitresb12/netcore-webapi-starter) should help get you started developing a Web API using .NET Core 6. It includes opinionated configurations for JetBrains Rider, Visual Studio, [commitlint](https://commitlint.js.org/) with commit hooks (powered by [Husky.Net](https://alirezanet.github.io/Husky.Net/)).

This project was generated using Rider's .NET Core Web API template using version 2022.1.1.

## Recommended IDE Setup

[JetBrains Rider](https://www.jetbrains.com/rider/) or [Microsoft Visual Studio 2022](https://visualstudio.microsoft.com/)

## Project Setup

You should also have [Node.js](https://nodejs.org/) and [NPM](https://npmjs.com/) installed for commitlint to work (any supported LTS version should be fine).

If you don't have Node.js installed, it is highly encouraged that you install it using a version manager:

- Linux/MacOS: use [nvm](https://github.com/nvm-sh/nvm)
- Windows: use [nvm-windows](https://github.com/coreybutler/nvm-windows)

After that, open the project on your preferred IDE from the [list above](#recommended-ide-setup).

Your IDE should automatically restore all the tools needed (Husky.Net, `dotnet-ef` and npm dependencies) during project restore. If this doesn't happen run `dotnet restore` in the terminal.

### 🚨 **IMPORTANT** 🚨

JetBrains Rider might prompt you to install EF Core tools automatically with a pop-up when you open the project. You can safely ignore this prompt, as the tool is already included in the projects manifest and should be installed during project restore. To verify, run the following command:

```sh
# The output should be the one below the command
$ dotnet ef

                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

Entity Framework Core .NET Command-line Tools 6.0.5

<Usage documentation follows, not shown.>
```

## Database Connection

This project is configured to use MySQL as the database, using [Pomelo's MySQL connector](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql). The connection string will be read from the Secret Manager tool. You can read more about it [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0).

To allow the app to connect to the database, set the connection string as a secret using the .NET CLI:

```sh
# Don't forget to replace {YOUR_CONNECTION_STRING} with the appropriate value
$ dotnet user-secrets set "DbConnectionString" "{YOUR_CONNECTION_STRING}" --project Starter.API
Successfully saved DbConnectionString = {YOUR_CONNECTION_STRING} to the secret store.
# Alternatively, move to project directory to avoid having to use `--project`
$ cd Starter.API/
$ dotnet user-secrets set "DbConnectionString" "{YOUR_CONNECTION_STRING}"
Successfully saved DbConnectionString = {YOUR_CONNECTION_STRING} to the secret store.
# Check that your value has been saved correctly
$ dotnet user-secrets list
DbConnectionString = {YOUR_CONNECTION_STRING}
```

## Migrations

This app is configured to use database migrations. You can read more about them [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/).

If this is the first time you're running the app, you probably won't have the database created. You can have EF create your database and create your schema from the migration files. This can be done via the following:

```sh
dotnet ef database update
```

If you make any changes to the database models, don't forget to update the migration scripts and re-sync your local database schema with the new schema:

```sh
# Don't forget to replace {YOUR_CHANGE_NAME} with a description of the changes you did to the model
# Note that we give migrations a descriptive name, to make it easier to understand the project history later
# You can use this like if it was a commit for your version control system
$ dotnet ef migrations add {YOUR_CHANGE_NAME}
# Check the migration files before applying any migrations, as they could cause data loss
# You can now apply your migration as before
$ dotnet ef database update
```

Whenever you create a new migration, EF Core will create the files needed to describe the migration. This files must be added to version control.

If you named your migration `SomeChanges`, three files will be added to your project under the Migrations directory:

- `XXXXXXXXXXXXXX_SomeChanges.cs` -- The main migrations file. Contains the operations necessary to apply the migration (in Up) and to revert it (in Down).
- `XXXXXXXXXXXXXX_SomeChanges.Designer.cs` -- The migrations metadata file. Contains information used by EF.
- `AppDbContextModelSnapshot.cs` -- A snapshot of your current model. Used to determine what changed when adding the next migration.

The timestamp in the filename helps keep them ordered chronologically so you can see the progression of changes.

### 🚨 **ALWAYS CHECK THE MIGRATION FILE** 🚨

It's **super important** that you **always** check the migration file (`XXXXXXXXXXXXXX_SomeChanges.cs`), since EF Core might drop columns that were just renamed and **you'll lose all the data in them if you apply the migration**. For more information on how to customize the migration code generated by EF Core, read [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing#customize-migration-code).

For more advanced usage of migrations, check the documentation [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing).

## Development server

Start a development server by running the ISS Express configuration from your IDE. Don't forget to trust the generated SSL certificate.

If you want to customize the SSL certificate used for IIS, check the following Stack Overflow [answer](https://stackoverflow.com/a/43676994/15040387).

## Build

Run `dotnet build` to build the project. The build artifacts will be stored in the `bin/` directory.

## License

[MIT](LICENSE)
