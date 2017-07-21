Public Class CombatLimb
    Implements iReportable
    Private Name As String
    Private Owner As BattleCombatant
    Private MechPart As MechPart
    Private _Damage As Integer
    Public ReadOnly Property Damage As Integer
        Get
            Return _Damage
        End Get
    End Property
    Private _Health As Integer
    Public ReadOnly Property Health As Integer
        Get
            Return _Health
        End Get
    End Property
    Private Dodge As Integer
    Private Defences As New List(Of DamageType)

    Public Shared Function Construct(ByVal mechpart As MechPart) As CombatLimb
        Dim combatLimb As New CombatLimb
        With combatLimb
            .Name = mechpart.Name
            .Owner = mechpart.Owner
            ._Health = mechpart.Health
            .Dodge = mechpart.Dodge
            .Defences = mechpart.Defences
        End With
        Return combatLimb
    End Function

    Public Function TargetedByAttack(ByVal attackAccuracy As Integer, ByVal attackDamage As Integer, ByVal damageType As DamageType) As String
        Dim accuracy As Integer = attackAccuracy - Dodge
        Dim roll As Integer = Rng.Next(1, 101)
        If roll <= accuracy Then
            'hit; check for defences
            'defences halve damage
            Dim dmg As Integer = attackDamage
            If Defences.Contains(damageType) Then dmg = Math.Floor(attackDamage / 2)
            _Damage += dmg
            TargetedByAttack = "Hit " & dmg & " " & damageType.ToString

            If _Damage > _Health Then Destroyed()
        Else
            'miss
            Return "Miss"
        End If
    End Function
    Private Sub Destroyed()
        Owner.RemoveCombatLimb(Me)
        Owner = Nothing
        If MechPart Is Nothing = False Then
            MechPart.IsDestroyed = True
            MechPart = Nothing
        End If
    End Sub
    Public Function Report() As String Implements iReportable.Report
        Dim percentage As Integer = CInt((_Health - _Damage) / _Health * 100)
        Dim total As String = Name & " [" & percentage & "%] "
        If Dodge > 0 Then total &= " - Dodge " & Dodge & "% "
        If Defences.Count > 0 Then total &= FormatCommaList(Of DamageType)(Defences)
        Return total
    End Function
End Class
