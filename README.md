# DevelopsToday.NET

## Getting Started

### 1. Configure Environment Variables
Create a `.env` file inside the `Presentation` layer and add the following content:

```
CONNECTION_STRING='YourDatabaseConnectionString'
```

**IMPORTNANT: ENSURE THAT THE USER SPECIFIED IN THE CONNECTION STRING HAS THE NECESSARY PERMISSIONS TO CREATE DATABASES**

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

