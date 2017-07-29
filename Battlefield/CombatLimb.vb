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
    Private _IsVital As Boolean
    Public ReadOnly Property IsVital As Boolean
        Get
            Return _IsVital
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
    Public Shared Function Construct(ByRef owner As BattleCombatant, ByVal value As String()) As CombatLimb
        Dim cl As New CombatLimb
        With cl
            Dim ac As New AutoIncrementer
            .Owner = owner

            .Name = value(ac.N)
            If value(ac.N) = "Vital" Then ._IsVital = True Else ._IsVital = False
            ._Health = CInt(value(ac.N))
            .Dodge = CInt(value(ac.N))
            Dim defences As String() = value(ac.N).Split(",")
            For Each defence In defences
                Dim modDefence As DamageType = String2Enum(Of DamageType)(defence)
                If modDefence <> 0 Then .Defences.Add(modDefence)
            Next
        End With
        Return cl
    End Function

    Public Function TargetedByAttack(ByVal attackAccuracy As Integer, ByVal damage As Dictionary(Of DamageType, Integer)) As String
        Dim accuracy As Integer = attackAccuracy - Dodge
        Dim totalDmg As Integer = 0
        Dim roll As Integer = Rng.Next(1, 101)
        If roll <= accuracy Then
            'hit; check for defences through all damage types
            'defences halve damage
            For Each dt In damage.Keys
                Dim dmg As Integer = damage(dt)
                If dmg <= 0 Then Continue For
                If Defences.Contains(dt) Then dmg = Math.Floor(dmg / 2)
                _Damage += dmg
                totalDmg += dmg
            Next
            Return totalDmg
        Else
            'miss
            Return "Miss"
        End If
    End Function
    Public Sub Destroyed()
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
        If Defences.Count > 0 Then total &= "vs " & FormatCommaList(Of DamageType)(Defences)
        Return total
    End Function
    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
