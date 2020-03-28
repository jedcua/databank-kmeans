Public Module Clustering
    Public Class Point
        Private x As Double
        Private y As Double

        Public Sub New(x As Double, y As Double)
            Me.x = x
            Me.y = y
        End Sub

        Public Function getXY() As (Double, Double)
            Return (Me.getX(), Me.getY())
        End Function

        Public Function getX() As Double
            Return Math.Round(Me.x, 2)
        End Function

        Public Function getY() As Double
            Return Math.Round(Me.y, 2)
        End Function
    End Class

    Public Class RandomPoint
        Inherits Point

        Public Sub New(random As System.Random, minX As Double, minY As Double, maxX As Double, maxY As Double)
            MyBase.New(random.Next(minX, maxX), random.Next(minY, maxY))
        End Sub
    End Class

    Public Class Cluster
        Private members As List(Of Point)
        Private centroid As Point
        Public Sub New(centroid As Point)
            Me.centroid = centroid
            Me.members = New List(Of Point)
        End Sub

        Public Sub add(point As Point)
            Me.members.Add(point)
        End Sub

        Public Sub clear()
            Me.members.Clear()
        End Sub

        Public Function size() As Integer
            Return Me.members.Count
        End Function

        Public Function points() As List(Of Point)
            Return New List(Of Point)(Me.members)
        End Function

        Public Function getCentroid() As Point
            Return Me.centroid
        End Function

        Public Function updateCentroid() As Point
            If Me.members.Count = 0 Then
                Return Me.getCentroid()
            End If

            Dim sumX = 0.0
            Dim sumY = 0.0
            Dim size = Me.members.Count

            For Each member As Point In Me.members
                sumX += member.getX()
                sumY += member.getY()
            Next

            Me.centroid = New Point(sumX / size, sumY / size)
            Return Me.getCentroid()
        End Function
    End Class

    Function Distance(pt1 As Point, pt2 As Point) As Double
        Dim deltaX = pt1.getX() - pt2.getX()
        Dim deltaY = pt1.getY() - pt2.getY()
        Return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2))
    End Function

    Function KMeans(k As Integer, random As System.Random, points As List(Of Point)) As List(Of Cluster)
        ' K must not exceed size of points
        If k > points.Count Then
            Throw New System.Exception("k cannot be greater than number of points!")
        End If

        ' Determine the minima and maxima from all points
        Dim minX = Double.MaxValue
        Dim maxX = -Double.MaxValue
        Dim minY = Double.MaxValue
        Dim maxY = -Double.MaxValue

        For Each point As Point In points
            minX = Math.Min(minX, point.getX())
            minY = Math.Min(minY, point.getY())
            maxX = Math.Max(maxX, point.getX())
            maxY = Math.Max(maxY, point.getY())
        Next

        ' Make a copy list for clustering
        Dim forClustering = New List(Of Point)(points)

        ' Create k number of clusters, with initial random centroid
        Dim clusters As New List(Of Cluster)
        For i = 1 To k
            Dim cluster As New Cluster(New RandomPoint(random, minX, minY, maxX, maxY))
            clusters.Add(cluster)
        Next

        ' Iterate until all cluster's centroid are stable
        Dim stable = False
        Do Until stable
            ' Clear cluster but keep centroid
            stable = True
            For Each cluster As Cluster In clusters
                cluster.clear()
            Next

            ' Group all points to the nearest centroid of a cluster
            For Each point As Point In forClustering
                Dim scores As New List(Of (Double, Cluster))

                For Each cluster As Cluster In clusters
                    Dim dist = Distance(cluster.getCentroid(), point)
                    scores.Add((dist, cluster))
                Next

                scores.Sort(Function(s1, s2) s1.Item1.CompareTo(s2.Item1))

                Dim winner As Cluster = scores.ElementAt(0).Item2
                winner.add(point)
            Next

            ' Compute the new centroid for each cluster
            ' Check if centroid is same as previous iteration
            For Each cluster As Cluster In clusters
                Dim oldCentroid = cluster.getCentroid().getXY()
                Dim newCentroid = cluster.updateCentroid().getXY()

                If Not oldCentroid.Equals(newCentroid) And stable Then
                    stable = False
                End If
            Next
        Loop

        Return clusters
    End Function
End Module
