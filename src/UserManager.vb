Imports Microsoft.Data.Sqlite
Imports System.Security.Cryptography
Imports System.Text

Module UserManager
    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant()
        End Using
    End Function

    Public Function RegisterUser(connection As SqliteConnection) As Boolean
        Console.Write("Choose a username: ")
        Dim username As String = Console.ReadLine()
        Console.Write("Choose a password: ")
        Dim password As String = Console.ReadLine()
        Dim hashedPassword As String = HashPassword(password)

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "INSERT INTO users (username, password) VALUES ($username, $password);"
        command.Parameters.AddWithValue("$username", username)
        command.Parameters.AddWithValue("$password", hashedPassword)

        Try
            command.ExecuteNonQuery()
            Console.WriteLine("User registered successfully!")
            Return True
        Catch ex As SqliteException
            Console.WriteLine("Username already exists. Try again.")
            Return False
        End Try
    End Function

    Public Function LoginUser(connection As SqliteConnection) As Integer
        Console.Write("Username: ")
        Dim username As String = Console.ReadLine()
        Console.Write("Password: ")
        Dim password As String = Console.ReadLine()
        Dim hashedPassword As String = HashPassword(password)

        Dim command As SqliteCommand = connection.CreateCommand()
        command.CommandText = "SELECT id FROM users WHERE username = $username AND password = $password;"
        command.Parameters.AddWithValue("$username", username)
        command.Parameters.AddWithValue("$password", hashedPassword)

        Dim result As Object = command.ExecuteScalar()

        If result IsNot Nothing Then
            Console.WriteLine("Login successful!")
            Return Convert.ToInt32(result)
        Else
            Console.WriteLine("Invalid credentials.")
            Return -1
        End If
    End Function
End Module
