
Imports Microsoft.Data.Sqlite
Imports System.IO

Module BudgetManager
    Public Sub AddIncome(connection As SqliteConnection, userId As Integer)
        Console.Write("Enter income amount: ")
        Dim amount As Double = Convert.ToDouble(Console.ReadLine())
        Console.Write("Enter description: ")
        Dim description As String = Console.ReadLine()

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "INSERT INTO transactions (description, amount, type, user_id) VALUES ($description, $amount, 'Income', $user_id);"
        command.Parameters.AddWithValue("$description", description)
        command.Parameters.AddWithValue("$amount", amount)
        command.Parameters.AddWithValue("$user_id", userId)
        command.ExecuteNonQuery()

        Console.WriteLine("Income added successfully!")
    End Sub

    Public Sub AddExpense(connection As SqliteConnection, userId As Integer)
        Console.Write("Enter expense amount: ")
        Dim amount As Double = Convert.ToDouble(Console.ReadLine())
        Console.Write("Enter description: ")
        Dim description As String = Console.ReadLine()

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "INSERT INTO transactions (description, amount, type, user_id) VALUES ($description, $amount, 'Expense', $user_id);"
        command.Parameters.AddWithValue("$description", description)
        command.Parameters.AddWithValue("$amount", -amount)
        command.Parameters.AddWithValue("$user_id", userId)
        command.ExecuteNonQuery()

        Console.WriteLine("Expense added successfully!")
    End Sub

    Public Sub ViewBalance(connection As SqliteConnection, userId As Integer)
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT SUM(amount) FROM transactions WHERE user_id = $user_id;"
        command.Parameters.AddWithValue("$user_id", userId)
        Dim balance As Object = command.ExecuteScalar()
        Console.WriteLine($"Current Balance: {If(balance IsNot Nothing, Convert.ToDouble(balance), 0):C}")
    End Sub

    Public Sub ViewTransactions(connection As SqliteConnection, userId As Integer)
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT id, description, amount, type FROM transactions WHERE user_id = $user_id;"
        command.Parameters.AddWithValue("$user_id", userId)
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

    Public Sub DeleteTransaction(connection As SqliteConnection, userId As Integer)
        ViewTransactions(connection, userId)
        Console.Write("Enter ID of transaction to delete: ")
        Dim id As Integer = Convert.ToInt32(Console.ReadLine())
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "DELETE FROM transactions WHERE id = $id AND user_id = $user_id;"
        command.Parameters.AddWithValue("$id", id)
        command.Parameters.AddWithValue("$user_id", userId)
        Dim rowsAffected = command.ExecuteNonQuery()
        If rowsAffected > 0 Then
            Console.WriteLine("Transaction deleted successfully!")
        Else
            Console.WriteLine("Transaction not found or not yours.")
        End If
    End Sub

    Public Sub ExportToCsv(connection As SqliteConnection, userId As Integer, Optional filePath As String = "transactions.csv")
        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT id, description, amount, type FROM transactions WHERE user_id = $user_id;"
        command.Parameters.AddWithValue("$user_id", userId)
        Using reader As SqliteDataReader = command.ExecuteReader()
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
    End Sub
End Module
