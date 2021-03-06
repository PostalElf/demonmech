﻿Public Class Component
    Public Name As String
    Public Slot As String                       'category for components, slot for mechParts; also contains information on how many hands the mech needs to equip it
    Public IsVital As Boolean                   'determines if limb is vital to the survival of the mech
    Public Weight As Integer                    'impact on mech agility
    Public Agility As Integer                   'impact on mech agility
    Public Dodge As Integer                     'accuracy penalty to hits on the mechpart'
    Public Defences As New List(Of DamageType)  'damageTypes it takes half damage from
    Public Health As Integer                    'how much damage the mechpart can take
    Public ExtraHands As Integer                'how many hands it adds to the mech
    Public InventorySpace As Integer            'how much inventory space for handweapons it adds
    Public AP As Integer                        'action points the mech gains
    Public APPerSeal As Integer                 'how many AP the mech gains per seal undone
    Public MP As Integer                        'movement points (free movement) the mech gains
    Public MPPerSeal As Integer                 'how much MP the mech gains per seal undone

    Public Accuracy As Integer                  'base percentile accuracy
    Public APCost As Integer                    'base AP cost to use weapon
    Public Aim As Integer                       'how much accuracy weapon gains per additional AP spent aiming
    Public AimAP As Integer                     'how much AP can be spent aiming
    Public Range As Integer                     'how many squares away the weapon can hit
    Public CoverIgnore As BattleObstacleCover = 0 'what level of cover to ignore
    Public DamageAmount As Integer
    Public DamageType As DamageType
    Public ReadOnly Property IsNotEmpty As Boolean
        Get
            'used to check if the component has values instead of being just an empty shell
            'especially important for BlueprintModifiers

            If Weight <> 0 Then Return True
            If Agility <> 0 Then Return True
            If Accuracy <> 0 Then Return True
            If Range <> 0 Then Return True
            If DamageAmount <> 0 Then Return True
            If DamageType <> 0 Then Return True
            Return False
        End Get
    End Property

    Public Shared Function Load(ByVal targetname As String) As Component
        Const path As String = "data/components.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, targetname)

        Dim component As New Component
        With component
            .Name = q.Dequeue

            While q.Count > 0
                Dim line As String() = q.Dequeue.Split(":")
                Dim key As String = line(0).Trim
                Dim value As String = line(1).Trim
                .Construct(key, value)
            End While
        End With
        Return component
    End Function
    Public Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Category" : Slot = value
            Case "Weight" : Weight = CInt(value)
            Case "Agility" : Agility = CInt(value)
            Case "Dodge" : Dodge = CInt(value)
            Case "Defence" : Defences.Add(String2Enum(Of DamageType)(value))
            Case "Health" : Health = CInt(value)
            Case "ExtraHands" : ExtraHands = CInt(value)
            Case "Inventory" : InventorySpace += CInt(value)
            Case "AP" : AP += CInt(value)
            Case "APPerSeal" : APPerSeal += CInt(value)
            Case "Movement", "MP" : MP += CInt(value)
            Case "MPPerSeal" : MPPerSeal += CInt(value)

            Case "Accuracy" : Accuracy = CInt(value)
            Case "APCost" : APCost = CInt(value)
            Case "Aim" : Aim = CInt(value)
            Case "AimAP" : AimAP = CInt(value)
            Case "Range" : Range = CInt(value)
            Case "CoverIgnore" : CoverIgnore = String2Enum(Of BattleObstacleCover)(value)
            Case "DamageAmount" : DamageAmount = CInt(value)
            Case "DamageType"
                For Each dt In [Enum].GetValues(GetType(DamageType))
                    If dt.ToString = value Then DamageType = dt
                Next
        End Select
    End Sub
    Public Overrides Function ToString() As String
        If Name = "" Then Return "-" Else Return Name
    End Function
End Class
