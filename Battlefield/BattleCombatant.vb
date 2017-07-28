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
    Public Function TargetedByAttack(ByVal LimbIndex As Integer, ByVal attackAccuracy As Integer, ByVal attackDamage As Dictionary(Of DamageType, Integer)) As String
        Return CombatLimbs(LimbIndex).TargetedByAttack(attackAccuracy, attackDamage)
    End Function
    Public Function TargetedByAttack(ByVal LimbIndex As Integer, ByVal weapon As MechPart) As String
        Return TargetedByAttack(LimbIndex, weapon.Accuracy, weapon.Damage)
    End Function
    Public Sub RemoveCombatLimb(ByVal CombatLimb As CombatLimb)
        CombatLimbs.Remove(CombatLimb)
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
        End If
    End Sub
    Public Sub RemoveCombatLimb(ByVal index As Integer)
        RemoveCombatLimb(CombatLimbs(index))
    End Sub

    Public Function Report() As String Implements iReportable.Report
        Dim total As String = Name & " [" & TotalHealthPercentage & "%] "
        Return total
    End Function
End Class
