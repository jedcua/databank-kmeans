Imports MySql.Data.MySqlClient
Public Module Enterprise
    Public Class Base
        Inherits Clustering.Point

        Public establishment As String
        Public yearStarted As Integer
        Public dailyIncome As Double
        Public classification As String
        Public typeOfBusiness As String
        Public industry As String
        Public contactPerson As String
        Public noOfEmployees As Integer
        Public longitude As Integer
        Public latitude As Integer

        Public Sub New(
            estblishmnt As String,
            yrStrtd As Integer,
            dlyIncome As Double,
            clssfctn As String,
            typeOfBus As String,
            indstry As String,
            cntctPrsn As String,
            noEmplyees As Integer,
            lon As Integer,
            lat As Integer
        )
            MyBase.New(lon, lat)
            Me.establishment = estblishmnt
            Me.yearStarted = yrStrtd
            Me.dailyIncome = dlyIncome
            Me.classification = clssfctn
            Me.typeOfBusiness = typeOfBus
            Me.industry = indstry
            Me.contactPerson = cntctPrsn
            Me.noOfEmployees = noEmplyees
            Me.longitude = lon
            Me.latitude = lat
        End Sub
    End Class

    Public Class DB
        Inherits Base
        Public Sub New(reader As MySqlDataReader)
            MyBase.New(
                reader.GetString(0),
                reader.GetInt16(1),
                reader.GetDouble(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7),
                reader.GetInt16(8),
                reader.GetInt16(9)
            )
        End Sub
    End Class
End Module
