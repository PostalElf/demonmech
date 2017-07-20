Public Class Enemy
    Inherits BattleCombatant

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

    Public Overrides Sub ConsoleWrite(ByVal targetListName As String)

    End Sub
End Class
