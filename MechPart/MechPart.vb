Public Class MechPart
    Public Name As String
    Public Slot As String
    Public HandSpace As Integer
    Public Weight As Integer
    Public Agility As Integer
    Public Range As Integer
    Public Hands As Integer
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
                .Slot = "Hand"
                .HandSpace = value
            Else
                .Slot = mechPartSlot
            End If

            .Slot = mechPartSlot
            For Each Component In Components
                .Weight += Component.Weight
                .Agility += Component.Agility
                .Range += Component.Range
                .Hands += Component.Hands
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
End Class
