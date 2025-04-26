Imports Microsoft.Data.Sqlite

Module Database
    Public Function OpenDatabase() As SqliteConnection
        Dim connection As New SqliteConnection("Data Source=budget.db")
        connection.Open()
        Return connection
    End Function

    Public Sub InitializeDatabase(connection As SqliteConnection)
        Dim createUserTable As SqliteCommand = connection.CreateCommand()
        createUserTable.CommandText = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT UNIQUE NOT NULL, password TEXT NOT NULL);"
        createUserTable.ExecuteNonQuery()

        Dim createTransactionTable As SqliteCommand = connection.CreateCommand()
        createTransactionTable.CommandText = "CREATE TABLE IF NOT EXISTS transactions (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT NOT NULL, amount REAL NOT NULL, type TEXT NOT NULL, user_id INTEGER NOT NULL, FOREIGN KEY (user_id) REFERENCES users(id));"
        createTransactionTable.ExecuteNonQuery()
    End Sub
End Module