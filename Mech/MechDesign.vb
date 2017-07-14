Public Class MechDesign
    Private Name As String
    Private MechSlotsCompulsory As New List(Of String)
    Private MechSlotsFilled As New List(Of String)
    Private MechParts As New List(Of MechPart)
    Private HandWeaponsInventory As New List(Of MechPart)
    Private HandWeaponsSpace As Integer

    Public Shared Function Load(ByVal mechName As String) As MechDesign
        Const path As String = "data/mechdesigns.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, mechName)

        Dim mechDesign As New MechDesign
        With mechDesign
            .Name = q.Dequeue

            While q.Count > 0
                Dim line As String() = q.Dequeue.Split(":")
                Dim key As String = line(0).Trim
                Dim value As String = line(1).Trim
                .construct(key, value)
            End While
        End With
        Return mechDesign
    End Function
    Private Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : MechSlotsCompulsory.Add(value)
            Case "Inventory" : HandWeaponsSpace = CInt(value)
        End Select
    End Sub
    Public Function ConstructMech(ByVal mechName As String) As Mech
        If MechSlotsCompulsory.Count > 0 Then Return Nothing
        Return Mech.Construct(mechName, Name, MechParts, HandWeaponsInventory)
    End Function

    Public Function AddMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If s = "Handweapon" Then
            If HandWeaponsSpace - mechpart.Hands < 0 Then Return "Insufficient inventory space"
            HandWeaponsSpace -= mechpart.Hands
            HandWeaponsInventory.Add(mechpart)
            Return Nothing
        Else
            If MechSlotsCompulsory.Contains(s) = False Then Return "Invalid mechpart slot"
            MechSlotsCompulsory.Remove(s)
            MechSlotsFilled.Add(s)
            MechParts.Add(mechpart)
            Return Nothing
        End If
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
