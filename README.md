# Introduction 
ShowRenamer is a small, kind of hacky .NET Core WPF Tool for renaming files based on a regex and the [TMDB API](https://developers.themoviedb.org/3/getting-started/introduction).

![Introduction GIF][./docs/imgs/ShowRenamer.gif]

# Building and running
You need .NET Core 3.1 to build the project.
Before you run the project for the first time, the [TDMB Bearer Token](https://developers.themoviedb.org/3/getting-started/authentication) has to be replaced in the [appsettings](../src/ShowRenamer/appsettings.json).