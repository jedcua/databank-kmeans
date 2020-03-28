Module Main
    Sub Main(args As String())
        ' Database parameters
        Dim host = "localhost"
        Dim port = 3306
        Dim database = "databank"
        Dim username = "root"
        Dim password = "root"

        ' KMeans parameters
        Dim k = 10
        Dim random = New System.Random(123456789)

        ' Fetch MSME from database and convert to Enterprise objects
        ' Cast List of Enterprise to List of Points
        Dim dbEnteprises As New Enterprises.DB(host, port, database, username, password)
        Dim points = dbEnteprises.List().Cast(Of Point).ToList()

        ' Execute KMeans Clustering
        Dim clusters As List(Of Cluster) = KMeans(k, random, points)

        ' Display summmary for each cluster
        For Each cluster As Cluster In clusters
            Console.WriteLine("-----------------------------")
            Console.WriteLine(String.Format("Centroid: {0}", cluster.getCentroid().getXY()))
            Console.WriteLine(String.Format("Size    : {0}", cluster.size()))
            Console.WriteLine("Members:")

            For Each enterprise As Enterprise.Base In cluster.points().Cast(Of Enterprise.Base).ToList()
                Console.WriteLine(String.Format("- {0} at {1}", enterprise.establishment, enterprise.getXY()))
            Next
        Next
    End Sub
End Module
