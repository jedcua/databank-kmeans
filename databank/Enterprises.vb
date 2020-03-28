Imports databank.Enterprise
Imports MySql.Data.MySqlClient
Public Interface Enterprises
    Function List() As List(Of Enterprise.Base)

    Class DB
        Implements Enterprises

        Private host As String
        Private port As Integer
        Private database As String
        Private username As String
        Private password As String
        Const query =
            "SELECT " &
                "establishment, " &
                "yearstarted, " &
                "dailyincome, " &
                "classification, " &
                "typeofbusiness, " &
                "industry, " &
                "contactperson, " &
                "noofemployees, " &
                "longitude, " &
                "latitude " &
            "FROM MSME;"

        Public Sub New(hst As String, prt As Integer, db As String, user As String, pass As String)
            host = hst
            port = prt
            database = db
            username = user
            password = pass
        End Sub

        Public Function List() As List(Of Base) Implements Enterprises.List
            Dim str = String.Format(
                "host={0};port={1};database={2};user={3};password={4}",
                host,
                port,
                database,
                username,
                password
            )
            Dim conn As New MySqlConnection(str)
            Dim reader As MySqlDataReader
            Dim enterpriseList As New List(Of Enterprise.Base)

            Try
                conn.Open()
                reader = New MySqlCommand(query, conn).ExecuteReader()

                While reader.Read()
                    enterpriseList.Add(New Enterprise.DB(reader))
                End While

            Catch ex As Exception
                Console.WriteLine("Error: " & ex.ToString())
            Finally
                reader.Close()
                conn.Close()
            End Try

            Return enterpriseList
        End Function
    End Class
End Interface
