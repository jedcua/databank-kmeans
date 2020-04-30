Module Main
    Sub Main(args As String())
        ' Database parameters
        Dim host = "localhost"
        Dim port = 3306
        Dim database = "databank"
        Dim username = "root"
        Dim password = "root"

        ' KMeans parameters
        Dim k = 2
        Dim random = New System.Random(123456789)

        ' Misc parameters
        Dim apiKey = "AIzaSyAmvQzOdlkF0HmBpX6Wa_rRPEGcrXXLdBk"
        Dim targetDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Desktop\kmeans-gis"
        Dim interval = 10000

        ' Create directory if not exist
        If (Not System.IO.Directory.Exists(targetDir)) Then
            System.IO.Directory.CreateDirectory(targetDir)
        End If

        ' Open index.html and apply API key, then save to target directory
        Dim siteContent = FileIO.FileSystem.ReadAllText("index.html").Replace("[API_KEY]", apiKey)
        FileIO.FileSystem.WriteAllText(targetDir & "\index.html", siteContent, False)

        ' Do the following every X seconds
        While True
            ' Fetch MSME from database and convert to Enterprise objects
            ' Cast List of Enterprise to List of Points
            Console.WriteLine("Fetching enterprises from database")
            Dim dbEnteprises As New Enterprises.DB(host, port, database, username, password)
            Dim points = dbEnteprises.List().Cast(Of Point).ToList()

            ' Execute KMeans Clustering
            Console.WriteLine("Executing Kmeans, k = " & k)
            Dim clusters As List(Of Cluster) = KMeans(k, random, points)

            ' Transform to JSON and write to file
            Console.WriteLine("Saving file to " & targetDir & "\clusters.json")
            Dim json = TransformToJson(clusters)
            FileIO.FileSystem.WriteAllText(targetDir & "\clusters.json", json, False)

            ' Sleep for X milliseconds
            Console.WriteLine("Re-running after " & interval & " milliseconds")
            Threading.Thread.Sleep(interval)
        End While
    End Sub
End Module
