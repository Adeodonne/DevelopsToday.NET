# DevelopsToday.NET

## Deliverables

1.Github:https://github.com/Adeodonne/DevelopsToday.NET

2.SQL-scripts: 
https://github.com/Adeodonne/DevelopsToday.NET/blob/main/Infrastructure/DatabaseInitializer/DatabaseScripts/TableCreation.sql
https://github.com/Adeodonne/DevelopsToday.NET/blob/main/Infrastructure/DatabaseInitializer/DatabaseInitializer.cs
(The database is created and populated with data from a CSV file when the application starts. Instructions can be found in the README file in the GitHub repository.)

3.Number of Rows in the Table: 29,378
(Rows were excluded if they contained an empty string in any property, an incorrect datetime format, a passenger count of ≤ 0, or an invalid store_and_fwd_flag (only values 0, 1, N, and Y were accepted).)

4. For Larger Datasets:
  I would create two separate applications:
  User App – Allows users to retrieve information from the database.
  Data Loader App – Runs in the background to read data from CSV files and inserts it into the database in bulk (e.g., 10,000 records at a time).

## Getting Started

### 1. Configure Environment Variables
Create a `.env` file inside the `Presentation` layer and add the following content:

```
CONNECTION_STRING='YourDatabaseConnectionString'
```

<span style="color: red;">**IMPORTNANT: ENSURE THAT THE USER SPECIFIED IN THE CONNECTION STRING HAS THE NECESSARY PERMISSIONS TO CREATE DATABASES AND DATABASE IS NOT CREATED BEFORE**</span>

### 2. Run the Application
Open a terminal and navigate to the `Presentation` directory:

```sh
cd Presentation
```

Start the application using:

```sh
dotnet run
```

### 3. Access API Documentation
Once the application is running, you can explore the available endpoints using Swagger:

[Swagger UI](http://localhost:5105/swagger/index.html)

