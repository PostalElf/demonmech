Public MustInherit Class BattleCombatant
    Inherits BattleObject
    Public ActionPoints As Integer
    Protected MustOverride ReadOnly Property ActionPointsMax As Integer
    Protected MovementPoints As Integer
    Protected MovementPointsMax As Integer
    Protected _IsCrusher As Boolean = False
    Public ReadOnly Property IsCrusher As Boolean
        Get
            Return _IsCrusher
        End Get
    End Property

    Public Sub MoveCombatant(ByVal bf As Battlefield, ByVal direction As Char)
        If ActionPoints < 1 Then Exit Sub

        Dim newX As Integer = X
        Dim newY As Integer = Y

        Select Case Char.ToUpper(direction)
            Case "N"c : newY -= 1
            Case "S"c : newY += 1
            Case "E"c : newX += 1
            Case "W"c : newX -= 1
        End Select

        If bf.CheckMove(newX, newY, Me) = False Then Exit Sub
        If ActionPoints < 1 Then Exit Sub

        ActionPoints -= 1
        bf.PlaceObject(newX, newY, Me)
    End Sub
End Class
