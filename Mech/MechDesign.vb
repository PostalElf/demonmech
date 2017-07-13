Public Class MechDesign
    Private Name As String
    Private MechSlotsCompulsory As New List(Of String)
    Private MechSlotsFilled As New List(Of String)
    Private MechParts As New List(Of MechPart)

    Public Shared Function Load(ByVal mechName As String) As MechDesign

    End Function

    Public Function ConstructMech(ByVal mechName As String) As Mech
        If MechSlotsCompulsory.Count > 0 Then Return Nothing
        Return Mech.Construct(mechName, Name, MechParts)
    End Function

    Public Function AddMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If MechSlotsCompulsory.Contains(s) = False Then Return "Invalid mechpart slot"

        MechSlotsCompulsory.Remove(s)
        MechSlotsFilled.Add(s)
        MechParts.Add(mechpart)
        Return Nothing
    End Function
    Public Function RemoveMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If MechSlotsFilled.Contains(s) = False Then Return "Invalid mechpart slot"

        MechSlotsFilled.Remove(s)
        MechSlotsCompulsory.Add(s)
        MechParts.Remove(mechpart)
        Return Nothing
    End Function
End Class
