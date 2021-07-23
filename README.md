# OJT Training Website

This is a OJT Training Website project. The objective of this project is to provide a base application on which the trainee could build upon.

A live version of this application is available [here](http://10.164.60.180/).

You need the company VPN connection in order to access the website.

This project require Visual Studio 2019 to develop.

# Database

## Shared Database

Shared database is provided with the following information:
- SQL Server : PostgreSQL v13
- IP : 10.164.60.180
- Port : 5432
- Database : BlazorDB
- RootUsername : postgres
- RootPassword : PMOPass
- Username : PMOUser
- Password : PMOPassword

## Local Database

If you want to set up a local database. You can run the following command in Nuget Package Manager Console.
```nuget
Update-Database
```

This command will apply the database structure to the database set in appsettings.Development.json

Note that the data will not be applied so you need to create your own users and roles.

# Framework

This website was build with Blazor framework.

You can read more about the framework [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0)

# Specification

A specification of this application is provided [here](https://fujitsu-my.sharepoint.com/:f:/p/mohamad_fadhli/Egh9vdNPCLVDns_iJ4PhU1YBQjgLdrmsiwB4SZPb1XghUQ?e=7A47o6)
