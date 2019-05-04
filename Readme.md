# Help learn our sight words.  

A simple web app that shows Sight Words and measures success rates.

## Contributer Notes
ASP.NET Core 2.1 and ReactJS are used for the backend and front end. EFCore is used to work with a PostgreSQL database.

### Bundling
Parcel is used as our bundler. Change your directory to Web and run the following to actively watch for changes:
```
npm run dev
```
...or, if you are using Visual Studio Code, run task `npm: dev - Web`. Note that Parcel is not used to serve the content, only to bundle it and place it in /wwwroot. ASP.NET Core hosts the content as a web app.

### Running
To build and run the web app locally, execute the following command in the root directory  
```
dotnet run
```
...or, if you are using Visual Studio Code, launch `.NET Core Launch (web)` by pressing F5 or pressing `Ctrl`+`Shift`+`D` then clicking the green arrow. If Parcel is watching for changes, then the updates to the front end code should show up on the screen as soon as one saves any changes.

### Publishing
``` 
dotnet publish --configuration Release 
```