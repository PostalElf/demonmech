﻿Public Class MechPart
    Inherits Component
    Implements iReportable
    Private BlueprintName As String
    Public Owner As BattleCombatant
    Public IsDestroyed As Boolean = False
    Public ReadOnly Property IsWeapon As Boolean
        Get
            For Each key In Damage.Keys
                If Damage(key) > 0 Then Return True
            Next
            Return False
        End Get
    End Property

    Public HandSpace As Integer
    Public Damage As New Dictionary(Of DamageType, Integer)

    Public Shared Shadows Function Construct(ByVal blueprintName As String, ByVal mechPartName As String, ByVal mechPartSlot As String, ByVal Components As List(Of Component)) As MechPart
        Dim MechPart As New MechPart
        With MechPart
            .BlueprintName = blueprintName
            .Name = mechPartName

            'check for handedness in mechPartSlot
            If mechPartSlot.StartsWith("Handweapon") Then
                Dim raw As String() = mechPartSlot.Split(" ")
                Dim value As Integer = CInt(raw(1))
                .Slot = "Handweapon"
                .HandSpace = value
            Else
                .Slot = mechPartSlot
            End If

            For Each Component In Components
                If Component.IsVital = True Then .IsVital = True
                .Weight += Component.Weight
                .Agility += Component.Agility
                .Dodge += Component.Dodge
                .Health += Component.Health
                .ExtraHands += Component.ExtraHands
                .InventorySpace += Component.InventorySpace
                .AP += Component.AP
                .APPerSeal += Component.APPerSeal
                .MP += Component.MP
                .MPPerSeal += Component.MPPerSeal

                .Accuracy += Component.Accuracy
                .APCost += Component.APCost
                .Aim += Component.Aim
                .AimAP += Component.AimAP
                .Range += Component.Range
                If Component.CoverIgnore > .CoverIgnore Then .CoverIgnore = Component.CoverIgnore
                If Component.DamageType <> 0 Then .Damage(Component.DamageType) += Component.DamageAmount
            Next

            If .Range < 0 Then .Range = 1
            If .Weight < 0 Then .Weight = 1
        End With
        Return MechPart
    End Function
    Public Shared Shadows Function Construct(ByVal value As String()) As MechPart
        Dim mp As New MechPart
        With mp
            Dim ac As New AutoIncrementer

            .Name = value(ac.N)
            If value(ac.N) = "Vital" Then .IsVital = True Else .IsVital = False
            .Health = CInt(value(ac.N))
            .Dodge = CInt(value(ac.N))
            Dim defences As String() = value(ac.N).Split(",")
            For Each defence In defences
                Dim modDefence As DamageType = String2Enum(Of DamageType)(defence)
                If modDefence <> 0 Then .Defences.Add(modDefence)
            Next
        End With
        Return mp
    End Function
    Public Sub New()
        For Each dt In [Enum].GetValues(GetType(DamageType))
            Damage.Add(dt, 0)
        Next
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
    Public Function Report() As String Implements iReportable.Report
        Dim total As String = Name & " [" & Slot & "]"
        If IsWeapon = True Then
            total &= ": "
            total &= APCost & "AP "
            total &= "r" & Range & " - "
            total &= Accuracy & "%"
            If AimAP > 0 Then total &= " (+" & AimAP & " x" & Aim & ")"
            total &= " - "
            For Each key In Damage.Keys
                If Damage(key) = 0 Then Continue For
                total &= Damage(key) & Shortener(key.ToString).ToLower
            Next
        End If
        Return total
    End Function

    Private Function Shortener(ByVal value As String) As String
        Select Case value
            Case "Piercing" : Return "P"
            Case "Slashing" : Return "S"
            Case "Bludgeoning" : Return "B"
            Case "Explosive" : Return "X"
            Case "Energy" : Return "N"
            Case "Infernal" : Return "I"
            Case Else : Return Nothing
        End Select
    End Function
End Class
