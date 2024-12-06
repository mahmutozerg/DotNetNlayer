# DotnetNLayer Framework Project
This is a framework project from my 2 year of experience in web api's

This project uses the **DotnetNLayer** framework and includes several features and functionalities for managing user roles, permissions, and background tasks.
## Layers Overview

This project follows a layered architecture with the following key layers:

### 1. **API Layer**
The **API Layer** handles HTTP requests and responses. It acts as the entry point for communication with the application. This layer exposes endpoints that the client can interact with, such as authentication, CRUD operations for users and roles, etc.

### 2. **Service Layer**
The **Service Layer** contains the business logic and service methods for the application. It is responsible for processing data and performing business rules. It is invoked by the API layer to handle more complex operations.

### 3. **Repository Layer**
The **Repository Layer** is responsible for abstracting data access and managing interactions with the database. It handles the CRUD operations and communicates with the underlying data storage (e.g., SQL Server). It is invoked by the Service Layer to retrieve and persist data.

### 4. **Background Job Layer**
The **Background Job Layer** uses Hangfire to manage and execute background tasks. These tasks are typically long-running processes that don't need to be executed immediately, such as database backups, scheduled email reminders, and other periodic operations.

## Features

- **Token-Based Authorization**: Secure access through token-based authentication.
- **Token Distributor**: Manages the distribution and validation of tokens.
- **CRUD Operations**: Supports Create, Read, Update, and Delete operations for Users and Roles.
- **Admin Role Management**: Allows admins to manage users, roles, and role assignments.

## Background Tasks (using Hangfire)

- **Database Backup**: Performs a database backup every 8 hours.

## Backlog (Planned Features)

- **Database Backup Removal**: Create a job that removes the excess backup files
- **SMTP Server for User Email Confirmation**: Implement an SMTP server to handle user email confirmations.
- **User Email Reminder**: Create a job that sends periodic emails to users. If the user does not confirm their email within a specified time, the user will be deleted.

## Setup

1. **Clone the repository**:
    ```bash
    git clone https://github.com/mahmutozerg/DotNetNlayer.git
    ```

2. **Install dependencies**:
    ```bash
    Add required Packages To this paths: DotnetNlayer/DotnetNayer/DotNlayer.API and  DotnetNlayer/DotnetNayer/SharedLibrary
    dotnet add package Microsoft.AspNetCore.Http.Abstractions
    dotnet add package Microsoft.AspNetCore.Mvc

    then
    cd DotnetNlayer/DotnetNayer/DotNlayer.API
    dotnet restore
    dotnet run
    ```

3. **Configure the database**:
    Update the `appsettings.json` file with your database connection string.
    Update The files under 'DotNetNlayer.Core/Constants' to your use case
    Update The files under 'SharedLibrary/Constants'     ⚠️ **Important:** The Aud variable should match with  "TokenOptions":"Audience" in 'appsettings.json'
   
   
5. **Run the project**:
    ```bash
    dotnet run
    ```

## Contributing

If you'd like to contribute to this project, feel free to fork the repository, create a branch, and submit a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
