# BudgetTrackerCLI

A powerful and secure Command-Line Budget Tracking Application built with **VB.NET** and **.NET 9.0**.  
It allows users to manage their income, expenses, and transactions efficiently, with a strong focus on security and extendability.

---

## Features

- 🔐 **User Authentication**  
  Secure registration and login system using **SHA-256** password hashing.

- 💸 **Income and Expense Management**  
  Add, delete, and list incomes and expenses.

- 📊 **Balance Overview**  
  Real-time balance calculation based on all transactions.

- 🗃️ **Transaction History**  
  View detailed logs of all transactions with timestamps.

- 💾 **Persistent Local Storage**  
  Data stored securely in a **SQLite** database file.

- 🛡️ **Automatic Backups**  
  Periodic data backups to prevent accidental data loss.

- 📄 **Export to CSV**  
  Export complete transaction history for external use.

---

## Technologies Used

- **VB.NET** (Visual Basic .NET)
- **SQLite** (Microsoft.Data.Sqlite)
- **.NET 9.0 SDK**
- **System.Security.Cryptography** for password hashing
- **System.IO** for CSV exports and backups

---

## How to Run

1. **Clone the repository**
    ```bash
    git clone git@github.com:ganeshasrinivasd/BudgetTrackerCLI.git
    cd BudgetTrackerCLI
    ```

2. **Build the project**
    ```bash
    dotnet build
    ```

3. **Run the application**
    ```bash
    dotnet run
    ```

4. Follow the interactive prompts to create an account or log in.

---

## Folder Structure

```
/BudgetTrackerCLI
│
├── /Models
│   ├── User.vb               # User data model
│   └── Transaction.vb        # Transaction data model
│
├── /Services
│   ├── AuthService.vb        # Handles user authentication
│   ├── TransactionService.vb # Handles income/expense operations
│   └── BackupService.vb      # Manages automatic backups
│
├── /Database
│   └── database.db           # SQLite database file (auto-created)
│
├── Program.vb                 # Main program entry point
├── README.md                  # Project documentation
└── .gitignore                 # Git ignored files
```

---

## Future Improvements

- **Dependency Injection (DI) Architecture**  
  Refactor services to use native `.NET Core DI` for better scalability and testing.

- **Cross-Platform Release**  
  Package the CLI app as a single `.exe` or `.dll` using `dotnet publish` for Linux, Mac, and Windows.

- **Asynchronous Database Operations**  
  Implement `async/await` pattern for smoother non-blocking DB operations.

- **Cloud Sync Feature**  
  Integrate optional cloud storage (Azure Blob Storage or AWS S3) to sync local backups online.

- **Role-Based Access Control**  
  Expand authentication to support multiple users with different roles (admin, user).

- **Logging and Monitoring**  
  Add proper logging using `.NET ILogger` for better debugging and production monitoring.

- **Unit and Integration Testing**  
  Write tests using `xUnit` or `NUnit` to ensure code quality and prevent regressions.

- **API Exposure**  
  Build a minimal WebAPI around BudgetTrackerCLI to allow web/mobile clients in future.

---


