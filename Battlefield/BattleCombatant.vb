Public MustInherit Class BattleCombatant
    Inherits BattleObject
    Implements iReportable
    Public ActionPoints As Integer
    Protected MustOverride ReadOnly Property ActionPointsMax As Integer
    Public MovementPoints As Integer
    Protected MustOverride ReadOnly Property MovementPointsMax As Integer
    Protected CombatLimbs As New List(Of CombatLimb)
    Protected ReadOnly Property TotalHealth As Integer
        Get
            Dim total As Integer = 0
            For Each cl In CombatLimbs
                total += cl.Health
            Next
            Return total
        End Get
    End Property
    Protected ReadOnly Property TotalDamage As Integer
        Get
            Dim total As Integer = 0
            For Each cl In CombatLimbs
                total += cl.Damage
            Next
            Return total
        End Get
    End Property
    Protected ReadOnly Property TotalHealthPercentage As Integer
        Get
            If TotalHealth = 0 OrElse TotalDamage = 0 Then Return 100
            Return CInt((TotalHealth - TotalDamage) / TotalHealth * 100)
        End Get
    End Property
    Protected _IsCrusher As Boolean = False
    Public ReadOnly Property IsCrusher As Boolean
        Get
            Return _IsCrusher
        End Get
    End Property

    Protected MechParts As New List(Of MechPart)
    Public MustOverride ReadOnly Property Weapons As List(Of MechPart)
    Public ReadOnly Property WeaponTargets(ByVal battlefield As Battlefield, ByVal weapon As MechPart) As List(Of BattleCombatant)
        Get
            Dim total As New List(Of BattleCombatant)
            Dim directions As Char() = {"N"c, "E"c, "S"c, "W"c}
            Dim range As Integer = weapon.Range

            For Each d In directions
                Dim squares As List(Of BattleObject) = battlefield.GetSquares(X, Y, range, d)
                Dim highestCover As BattleObstacleCover = BattleObstacleCover.None
                For Each square In squares
                    If square Is Nothing Then Continue For
                    If TypeOf square Is BattleObstacle Then
                        'if there's an obstacle, add it to highest cover
                        Dim obstacle As BattleObstacle = CType(square, BattleObstacle)
                        If highestCover < obstacle.Cover Then highestCover = obstacle.Cover
                    ElseIf TypeOf square Is BattleCombatant Then
                        'if it's a combatant, check if there's cover previously in the way
                        'if there's cover, and weapon ignores the cover, add target to list
                        If weapon.CoverIgnore >= highestCover Then total.Add(square)
                    End If
                Next
            Next
            Return total
        End Get
    End Property

    Public Sub MoveCombatant(ByVal bf As Battlefield, ByVal direction As Char)
        Dim newX As Integer = X
        Dim newY As Integer = Y

        Select Case Char.ToUpper(direction)
            Case "N"c : newY -= 1
            Case "S"c : newY += 1
            Case "E"c : newX += 1
            Case "W"c : newX -= 1
        End Select

        'get movecost, then check for illegal movement (-1) or insufficient AP + MP
        Dim moveCost As Integer = bf.GetMoveCost(newX, newY, Me)
        If moveCost = -1 Then Exit Sub
        If moveCost > ActionPoints + MovementPoints Then Exit Sub

        'spend MP first, then if there's leftover spend AP
        If MovementPoints > 0 Then
            MovementPoints -= moveCost
            If MovementPoints < 0 Then moveCost = Math.Abs(MovementPoints) : MovementPoints = 0 Else moveCost = 0
        End If
        If moveCost > 0 Then
            ActionPoints -= moveCost
        End If

        'now actually put the damned thing on the map
        bf.PlaceObject(newX, newY, Me)
    End Sub
    Public MustOverride Sub ConsoleWrite(ByVal targetListName As String)
    Public Sub PerformsAttack(ByVal target As BattleCombatant, ByVal targetLimbIndex As Integer, ByVal weapon As MechPart)
        If ActionPoints < weapon.APCost Then Exit Sub

        ActionPoints -= weapon.APCost
        target.TargetedByAttack(targetLimbIndex, weapon)
    End Sub
    Public Sub TargetedByAttack(ByVal LimbIndex As Integer, ByVal weapon As MechPart)
        Dim targetLimb As CombatLimb = CombatLimbs(LimbIndex)
        Dim rawReport As String = targetLimb.TargetedByAttack(weapon.Accuracy, weapon.Damage)
        If rawReport = "Miss" Then
            'miss
            Reports.Add(weapon.Name & " missed " & StringPossessive(Name) & targetLimb.ToString & "!")
        Else
            'hit X
            Dim totalDmg As Integer = CInt(rawReport)
            Reports.Add(weapon.Name & " hits " & StringPossessive(Name) & " " & targetLimb.ToString & " for " & totalDmg & " dmg.")
            If targetLimb.Damage >= targetLimb.Health Then targetLimb.Destroyed()
        End If
    End Sub
    Public Sub RemoveCombatLimb(ByVal CombatLimb As CombatLimb)
        CombatLimbs.Remove(CombatLimb)
        Reports.Add(StringPossessive(Name) & " " & CombatLimb.ToString & " was destroyed!")
        If CombatLimb.isVital = True Then
            'vital limb removed
            'check to see if any vital limbs remain
            Dim hasVitalRemaining As Boolean = False
            For Each cl In CombatLimbs
                If cl.IsVital = True Then hasVitalRemaining = True : Exit For
            Next
            If hasVitalRemaining = True Then Exit Sub

            'no vital limbs remain; destroy
            Battlefield.RemoveObject(Me)
            Reports.Add(Name & " was annihilated!")
        End If
    End Sub
    Public Sub RemoveCombatLimb(ByVal index As Integer)
        RemoveCombatLimb(CombatLimbs(index))
    End Sub
    Public Sub EndTurn()
        ActionPoints = ActionPointsMax
        MovementPoints = MovementPointsMax
    End Sub

    Public Function Report() As String Implements iReportable.Report
        Dim total As String = Name & " [" & TotalHealthPercentage & "%] "
        Return total
    End Function
End Class
