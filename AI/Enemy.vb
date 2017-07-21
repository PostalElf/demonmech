Public Class Enemy
    Inherits BattleCombatant
    Private _C As Char
    Public Shadows ReadOnly Property C As Char
        Get
            Return _C
        End Get
    End Property
    Private _ActionPointsMax As Integer
    Protected Overrides ReadOnly Property ActionPointsMax As Integer
        Get
            Return _ActionPointsMax
        End Get
    End Property
    Private _MovementPointsMax As Integer
    Protected Overrides ReadOnly Property MovementPointsMax As Integer
        Get
            Return _MovementPointsMax
        End Get
    End Property

    Private Weapons As New List(Of MechPart)

    Public Shared Function Load(ByVal enemyName As String) As Enemy

    End Function
    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Overrides Sub ConsoleWrite(ByVal targetListName As String)
        Dim targetList As System.Collections.Generic.IEnumerable(Of iReportable)
        Select Case targetListName
            Case "CombatLimbs" : targetList = CombatLimbs
            Case "Weapons" : targetList = Weapons
            Case Else : Exit Sub
        End Select

        Dim counter As Integer = 1
        For Each thing In targetList
            Console.WriteLine(counter & ") " & thing.Report)
            counter += 1
        Next
    End Sub
End Class
