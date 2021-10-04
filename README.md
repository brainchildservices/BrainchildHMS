# BrainchildHMS


clone the project using 

`git clone https://github.com/brainchildservices/BrainchildHMS.git`

Goto the Brainchild.HMS.Web Project and run the command **dotnet restore**

to build, simply run **dotnet build**

Before run the project, delete the AppData folder inside Brainchild.Core.Web project

to run the project, you have to run **dotnet run** (always run the **Brainchild.HMS.Web** Project)

Then in the Setup page 
* select *Software as a Service* Recipe  
* select *SQL Server* and give the connection string 

![image](https://user-images.githubusercontent.com/87529271/135834587-5ae90771-3202-4237-8fbd-5d45a6f15407.png)


After the setup go to Admin Panel *https://localhost:5001/admin* , Open Features from the Configuration tab. Then Search for **Brainchild.HMS.API** and Enable it.

![image](https://user-images.githubusercontent.com/87529271/135835953-9f87e922-7884-4ea3-a8fc-29f8642f2931.png)




## make sure to update the ef core tools 
if not installed,install using

```
dotnet tool install --global dotnet-ef
```

##update using 
```
dotnet tool update --global dotnet-ef
```

If you don't have Visual studio or Microsoft SQL Management Studio in your local machine, please install it.
*for SQL Management studio https://www.microsoft.com/en-in/sql-server/sql-server-downloads*
*for Visual Studio https://visualstudio.microsoft.com/downloads/*

change the connection string according to your machine configuration DefaultConnection in appsettings.json inside Brainchild.Core.Data project, appsettings.json and Startup.cs inside the Brainchild.Core.Web project

After this, run following command to update your DB settings

## command to create/update database creation

*dotnet ef database update --project Brainchild.Portal.Data*   

Please make sure to run this from the root of your project directory 


