Public Class MechPart
    Private Name As String
    Private Weight As Integer
    Private Agility As Integer
    Private Range As Integer
    Private Accuracy As Integer
    Private Damage As New Dictionary(Of DamageType, Integer)

    Public Shared Function Construct(ByVal blueprintName As String, ByVal Components As List(Of Component)) As MechPart
        Dim MechPart As New MechPart
        With MechPart
            .Name = blueprintName
            For Each Component In Components
                .Weight += Component.Weight
                .Agility += Component.Agility
                .Accuracy += Component.Accuracy
                .Range += Component.Range
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
