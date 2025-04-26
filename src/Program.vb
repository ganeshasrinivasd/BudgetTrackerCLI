Imports Microsoft.Data.Sqlite
Imports System.Threading.Tasks
Imports System.Threading
Imports System.Globalization

Module Program
    Sub Main()
        Using connection As SqliteConnection = Database.OpenDatabase()
            Database.InitializeDatabase(connection)

            Console.WriteLine("===== Welcome to Budget Tracker =====")
            Console.WriteLine("1. Login")
            Console.WriteLine("2. Register")
            Console.WriteLine("3. Exit")

            Dim userId As Integer = -1

            While userId = -1
                Console.Write("Enter choice: ")
                Dim choice As String = Console.ReadLine()

                Select Case choice
                    Case "1"
                        userId = UserManager.LoginUser(connection)
                    Case "2"
                        If UserManager.RegisterUser(connection) Then
                            userId = UserManager.LoginUser(connection)
                        End If
                    Case "3"
                        Environment.Exit(0)
                    Case Else
                        Console.WriteLine("Invalid choice. Please try again.")
                End Select
            End While

            ' Start auto-backup task
            Task.Run(Sub()
                While True
                    Thread.Sleep(TimeSpan.FromMinutes(5))
                    ' Thread.Sleep(TimeSpan.FromSeconds(50))

                    Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd_HH-mm", CultureInfo.InvariantCulture)
                    Dim fileName As String = $"backup_{timestamp}.csv"
                    BudgetManager.ExportToCsv(connection, userId, fileName)
                    Console.WriteLine($"[Auto Backup] Saved to {fileName}")
                End While
            End Sub)

            Dim running As Boolean = True
            While running
                Console.WriteLine()
                Console.WriteLine("===== BUDGET MENU =====")
                Console.WriteLine("1. Add Income")
                Console.WriteLine("2. Add Expense")
                Console.WriteLine("3. View Balance")
                Console.WriteLine("4. View Transactions")
                Console.WriteLine("5. Delete Transaction")
                Console.WriteLine("6. Export Transactions to CSV")
                Console.WriteLine("7. Exit")
                Console.Write("Enter your choice: ")
                Dim choice As String = Console.ReadLine()

                Select Case choice
                    Case "1"
                        BudgetManager.AddIncome(connection, userId)
                    Case "2"
                        BudgetManager.AddExpense(connection, userId)
                    Case "3"
                        BudgetManager.ViewBalance(connection, userId)
                    Case "4"
                        BudgetManager.ViewTransactions(connection, userId)
                    Case "5"
                        BudgetManager.DeleteTransaction(connection, userId)
                    Case "6"
                        BudgetManager.ExportToCsv(connection, userId)
                    Case "7"
                        running = False
                    Case Else
                        Console.WriteLine("Invalid choice. Please try again.")
                End Select
            End While
        End Using
    End Sub
End Module
