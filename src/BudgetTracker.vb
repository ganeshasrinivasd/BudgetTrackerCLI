Imports Microsoft.Data.Sqlite
Imports System.IO

Module BudgetTracker
    Public Function OpenDatabase() As SqliteConnection
        Dim connection As New SqliteConnection("Data Source=budget.db")
        connection.Open()
        Return connection
    End Function

    Public Sub InitializeDatabase(connection As SqliteConnection)
        Dim createTableCommand As SqliteCommand = connection.CreateCommand()
        createTableCommand.CommandText = "
            CREATE TABLE IF NOT EXISTS transactions (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                description TEXT NOT NULL,
                amount REAL NOT NULL,
                type TEXT NOT NULL
            );
        "
        createTableCommand.ExecuteNonQuery()
    End Sub

    Public Sub AddIncome(connection As SqliteConnection)
        Console.Write("Enter income amount: ")
        Dim amount As Double = Convert.ToDouble(Console.ReadLine())
        Console.Write("Enter description: ")
        Dim description As String = Console.ReadLine()

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "
            INSERT INTO transactions (description, amount, type)
            VALUES ($description, $amount, 'Income');
        "
        command.Parameters.AddWithValue("$description", description)
        command.Parameters.AddWithValue("$amount", amount)
        command.ExecuteNonQuery()

        Console.WriteLine("Income added successfully!")
    End Sub

    Public Sub AddExpense(connection As SqliteConnection)
        Console.Write("Enter expense amount: ")
        Dim amount As Double = Convert.ToDouble(Console.ReadLine())
        Console.Write("Enter description: ")
        Dim description As String = Console.ReadLine()

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "
            INSERT INTO transactions (description, amount, type)
            VALUES ($description, $amount, 'Expense');
        "
        command.Parameters.AddWithValue("$description", description)
        command.Parameters.AddWithValue("$amount", -amount)
        command.ExecuteNonQuery()

        Console.WriteLine("Expense added successfully!")
    End Sub

    Public Sub ViewBalance(connection As SqliteConnection)
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT SUM(amount) FROM transactions;"
        Dim balance As Object = command.ExecuteScalar()

        Console.WriteLine($"Current Balance: {If(balance IsNot Nothing, Convert.ToDouble(balance), 0):C}")
    End Sub

    Public Sub ViewTransactions(connection As SqliteConnection)
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT id, description, amount, type FROM transactions;"

        Using reader As SqliteDataReader = command.ExecuteReader()
            Console.WriteLine("=== Transactions ===")
            If Not reader.HasRows Then
                Console.WriteLine("No transactions found.")
            End If

            While reader.Read()
                Dim id As Integer = reader.GetInt32(0)
                Dim description As String = reader.GetString(1)
                Dim amount As Double = reader.GetDouble(2)
                Dim type As String = reader.GetString(3)

                Console.WriteLine($"{id}. {type}: {description} - {amount:C}")
            End While
        End Using
    End Sub

    Public Sub DeleteTransaction(connection As SqliteConnection)
        ViewTransactions(connection)

        Console.Write("Enter the ID of the transaction to delete: ")
        Dim input As String = Console.ReadLine()
        Dim id As Integer

        If Integer.TryParse(input, id) Then
            Dim command As SqliteCommand = connection.CreateCommand()
            command.CommandText = "DELETE FROM transactions WHERE id = $id;"
            command.Parameters.AddWithValue("$id", id)
            Dim rowsAffected = command.ExecuteNonQuery()

            If rowsAffected > 0 Then
                Console.WriteLine("Transaction deleted successfully!")
            Else
                Console.WriteLine("Transaction not found.")
            End If
        Else
            Console.WriteLine("Invalid ID.")
        End If
    End Sub

    Public Sub ExportToCsv(connection As SqliteConnection)
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT id, description, amount, type FROM transactions;"

        Using reader As SqliteDataReader = command.ExecuteReader()
            Dim filePath As String = "transactions.csv"
            Using writer As New StreamWriter(filePath)
                writer.WriteLine("ID,Description,Amount,Type")
                While reader.Read()
                    Dim id As Integer = reader.GetInt32(0)
                    Dim description As String = reader.GetString(1).Replace(",", " ")
                    Dim amount As Double = reader.GetDouble(2)
                    Dim type As String = reader.GetString(3)

                    writer.WriteLine($"{id},{description},{amount},{type}")
                End While
            End Using
        End Using

        Console.WriteLine("Transactions exported successfully to 'transactions.csv'!")
    End Sub
End Module
