# KeyFortress - A Password Manager CLI
## Overview
KeyFortress is a multi-user, console-based password manager built using .NET Core and SQLite. It allows users to securely store, manage, and retrieve passwords for various applications, websites, and games

## Table of Contents
- Overview
- Features
- Project Structure
- Design Patterns
- CypherGenius (Encryption/Decryption)
- Database
- Installation
- Usage
- Classes and Methods

## Features
- Multi-user support
- Secure password storage with encryption
- Command-line interface for user interactions
- Password generation
- Cross-platform compatibility (Windows, macOS, Linux)

## Project Structure

```
.gitignore
enums/
    LoginResponse.cs
    SignupResponse.cs
KeyFortress.csproj
KeyFortress.sln
Migrations/
    20240105233447_InitialCreate.cs
    20240105233447_InitialCreate.Designer.cs
    DBModelSnapshot.cs
models/
    DB.cs
    Password.cs
    User.cs
Program.cs
README.md
repositories/
    PasswordRepository.cs
    UserRepository.cs
run.bat
run.sh
shared/
    Shared.state.cs
ui/
    Auth.cs
    Menu.cs
    UI.cs
UML/
    CipherGenius.plantuml
    Class-Diagram.plantuml
    Given-1.plantuml
    Given-2.plantuml
    My-Old.plantuml
utils/
    CipherGenius.cs
    Utils.cs
```

## Composition of Classes
<img src="https://github.com/khaledCSE/KeyFortress/blob/main/images/class-diagram.png"/>

## Design Patterns
*KeyFortress* employs several design patterns to ensure a clean, maintainable, and scalable codebase. Here are some of the key design patterns used:

### 1. Repository Pattern
The repository pattern is used to abstract the data access logic and provide a clean API for the rest of the application. This pattern is implemented in the `repositories` directory.

- **UserRepository:** Manages user data access.
- **PasswordRepository:** Manages password data access.

```csharp
public class UserRepository
{
    // ...existing code...
    public void Create(User user)
    {
        // ...existing code...
    }
    // ...existing code...
}
```

### 2. Singleton Pattern
The singleton pattern ensures that a class has only one instance and provides a global point of access to it. This pattern is used in the `SharedState` class to manage shared state and configurations.

```csharp
public class SharedState
{
    private static SharedState instance;

    private SharedState() { }

    public static SharedState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SharedState();
            }
            return instance;
        }
    }

    public DB dB { get; set; }
    public User loggedInUser { get; set; }
}
```

### 3. Factory Pattern
The factory pattern is used to create objects without specifying the exact class of the object that will be created. This pattern can be seen in the `CipherGenius` class, which handles encryption and decryption.

```csharp
public class CipherGenius
{
    public static string Encrypt(string plainText, string key)
    {
        // ...existing code...
    }

    public static string Decrypt(string cipherText, string key)
    {
        // ...existing code...
    }
}
```

### 4. MVC (Model-View-Controller) Pattern
Although `KeyFortress` is a console-based application, it follows the MVC pattern to separate concerns:

- **Model**: Represents the data and business logic (e.g., `User`, `Password`, `DB`).
- **View**: Manages the user interface and presentation logic (e.g., `UI`, `Menu`).
- **Controller**: Handles user input and updates the model and view accordingly (e.g., `Auth`, `Menu`).

```csharp
public class Menu
{
    public void DisplayMenu()
    {
        // ...existing code...
    }

    public void AddPassword()
    {
        // ...existing code...
    }

    // ...existing code...
}
```

These design patterns help to keep the codebase organized, promote code reuse, and make it easier to maintain and extend the application.

## CypherGenius
The `CipherGenius` class is responsible for handling encryption and decryption of passwords in KeyFortress. It ensures that all stored passwords are securely encrypted, providing an additional layer of security for user data.

<img src="https://github.com/khaledCSE/KeyFortress/blob/main/images/cypherGenius.png" />

### Methods
- **Encrypt**: Encrypts a plain text password using a specified key.
- **Decrypt**: Decrypts an encrypted password using a specified key.

### Example Usage

```csharp
// Encrypting a password
string encryptedPassword = CipherGenius.Encrypt("myPlainPassword", "myEncryptionKey");

// Decrypting a password
string decryptedPassword = CipherGenius.Decrypt(encryptedPassword, "myEncryptionKey");
```

The `CipherGenius` class uses advanced encryption algorithms to ensure that passwords are stored securely and can only be accessed by authorized users. This is a crucial component of the KeyFortress application, providing robust security for sensitive user data.

### Database
### SQLite and CRUD Operations
`KeyFortress` uses `SQLite` as its database to store user and password information. `SQLite` is a lightweight, disk-based database that doesn't require a separate server process, making it ideal for embedded applications like KeyFortress.

## Installation
### Prerequisites
- .NET Core must be installed
- Entity Framework Core must be installed
  - To install it, run: `dotnet tool install --global dotnet-ef`

## Using Automated Script

KeyFortress comes baked with automated scripts for development.

- For windows: double click and open `run.bat` and that's it!
- For mac and linux, run `sudo chmod +x run.sh` for the first time and `./run.sh` from then on.
> NOTE: Linux and macOS must have at least .NET Core installed

### Running it From Scratch
1. Clone the repository or download the source files.
2. Navigate to the project directory in the console.
3. Run `dotnet restore` to install the dependencies.
4. Sync the database to the latest changes: `dotnet ef database update`

## Usage
### Authentication
Upon starting the application, users are prompted to either log in or sign up.

- Login: Existing users can log in with their username and password.
- Signup: New users can sign up by providing a username and password.

### Main Menu
After authentication, users are presented with the main menu:

1. 🔑 Add Password
2. 📃 List Passwords
3. 🔍 Search Password
4. 🛡️ Update Password
5. 🗑️ Delete Password
6. 🍀 About
7. ❌ Exit

### Adding a Password
Users can add passwords for websites, applications, or games by providing the necessary details.

### Listing Passwords
Users can list all their stored passwords, grouped by type (website, application, game).

### Searching Passwords
Users can search for passwords using a search term.

### Updating a Password
Users can update the details of an existing password by providing the password ID.

### Deleting a Password
Users can delete a password by providing the password ID.

### About
Displays information about the KeyFortress application.