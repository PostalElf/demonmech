Public Class MechPart
    Public Name As String
    Public Slot As String
    Public HandSpace As Integer
    Public Weight As Integer
    Public Agility As Integer
    Public Range As Integer
    Public ExtraHands As Integer
    Public InventorySpace As Integer
    Public AP As Integer
    Public APPerSeal As Integer
    Public Accuracy As Integer
    Public Damage As New Dictionary(Of DamageType, Integer)

    Public Shared Function Construct(ByVal blueprintName As String, ByVal mechPartSlot As String, ByVal Components As List(Of Component)) As MechPart
        Dim MechPart As New MechPart
        With MechPart
            .Name = blueprintName

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
                .Weight += Component.Weight
                .Agility += Component.Agility
                .Range += Component.Range
                .ExtraHands += Component.ExtraHands
                .InventorySpace += Component.InventorySpace
                .Accuracy += Component.Accuracy
                If Component.DamageType <> 0 Then .Damage(Component.DamageType) += Component.DamageAmount
            Next

            If .Range < 0 Then .Range = 1
            If .Weight < 0 Then .Weight = 1
        End With
        Return MechPart
    End Function
    Public Sub New()
        For Each dt In [Enum].GetValues(GetType(DamageType))
            Damage.Add(dt, 0)
        Next
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
    Public Function Report() As String
        Return Name & " [" & Slot & "]: "
    End Function
End Class
