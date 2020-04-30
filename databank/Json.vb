Public Module Json
    Class JsonClusters
        Public Property clusters As List(Of JsonCluster)
        Public Sub New(clusters As List(Of JsonCluster))
            Me.clusters = clusters
        End Sub
    End Class
    Class JsonCluster
        Public Property label As String
        Public Property longitude As Double
        Public Property latitude As Double
        Public Property members As List(Of JsonEnterprise)
        Public Sub New(
            label As String,
            longitude As Double,
            latitude As Double,
            members As List(Of JsonEnterprise)
        )
            Me.label = label
            Me.longitude = longitude
            Me.latitude = latitude
            Me.members = members
        End Sub
    End Class
    Class JsonEnterprise
        Public Property establishment As String
        Public Property yearStarted As Integer
        Public Property dailyIncome As Double
        Public Property classification As String
        Public Property typeOfBusiness As String
        Public Property industry As String
        Public Property contactPerson As String
        Public Property noOfEmployees As Integer
        Public Property longitude As Integer
        Public Property latitude As Integer

        Public Sub New(
            enterprise As Enterprise.Base
        )
            Me.establishment = enterprise.establishment
            Me.yearStarted = enterprise.yearStarted
            Me.dailyIncome = enterprise.dailyIncome
            Me.classification = enterprise.classification
            Me.typeOfBusiness = enterprise.typeOfBusiness
            Me.industry = enterprise.industry
            Me.contactPerson = enterprise.contactPerson
            Me.noOfEmployees = enterprise.noOfEmployees
            Me.longitude = enterprise.longitude
            Me.latitude = enterprise.latitude
        End Sub
    End Class

    Public Function TransformToJson(clusters As List(Of Cluster))
        Dim param As New fastJSON.JSONParameters()
        param.UseExtensions = False

        Dim list As New List(Of JsonCluster)
        Dim count = 1
        For Each cluster As Cluster In clusters
            Dim members As New List(Of JsonEnterprise)

            For Each enterprise As Enterprise.Base In cluster.points().Cast(Of Enterprise.Base).ToList()
                members.Add(New JsonEnterprise(enterprise))
            Next

            list.Add(
                New JsonCluster(
                    count,
                    cluster.getCentroid().getX(),
                    cluster.getCentroid().getY(),
                    members
                )
            )
            count += 1
        Next

        Dim jsonClusters As New JsonClusters(list)

        Return fastJSON.JSON.ToNiceJSON(jsonClusters, param)
    End Function
End Module
