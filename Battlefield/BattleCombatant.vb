Public MustInherit Class BattleCombatant
    Inherits BattleObject
    Public ActionPoints As Integer
    Protected MustOverride ReadOnly Property ActionPointsMax As Integer
    Public MovementPoints As Integer
    Protected MustOverride ReadOnly Property MovementPointsMax As Integer
    Protected CombatLimbs As New List(Of CombatLimb)
    Protected _IsCrusher As Boolean = False
    Public ReadOnly Property IsCrusher As Boolean
        Get
            Return _IsCrusher
        End Get
    End Property

    Public Sub MoveCombatant(ByVal bf As Battlefield, ByVal direction As Char)
        If MovementPoints < 1 AndAlso ActionPoints < 1 Then Exit Sub

        Dim newX As Integer = X
        Dim newY As Integer = Y

        Select Case Char.ToUpper(direction)
            Case "N"c : newY -= 1
            Case "S"c : newY += 1
            Case "E"c : newX += 1
            Case "W"c : newX -= 1
        End Select

        If bf.CheckMove(newX, newY, Me) = False Then Exit Sub

        If MovementPoints > 0 Then
            MovementPoints -= 1
        Else
            ActionPoints -= 1
        End If

        bf.PlaceObject(newX, newY, Me)
    End Sub
    Public MustOverride Sub ConsoleWrite(ByVal targetListName As String)
    Public MustOverride Function TargetedByAttack(ByVal LimbIndex As Integer, ByVal weapon As MechPart) As String
    Public MustOverride Function TargetedByAttack(ByVal LimbIndex As Integer, ByVal accuracy As Integer, ByVal damage As Integer, ByVal damagetype As DamageType) As String
    Public MustOverride Sub RemoveCombatLimb(ByVal CombatLimb As CombatLimb)
    Public MustOverride Sub RemoveCombatLimb(ByVal index As Integer)
End Class
