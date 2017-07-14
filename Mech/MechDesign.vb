Public Class MechDesign
    Private Name As String
    Private Mech As Mech
    Private MechSlotsCompulsory As New List(Of String)
    Private MechSlotsFilled As New List(Of String)
    Private MechDesignModifiers As New Component                 'hidden container that holds all the modifiers from the design

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

            .Mech = Mech.Construct(.Name, .MechDesignModifiers)
        End With
        Return mechDesign
    End Function
    Private Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : MechSlotsCompulsory.Add(value)
            Case "Inventory" : MechDesignModifiers.InventorySpace += CInt(value)
        End Select
    End Sub
    Public Function ConstructMech(ByVal mechName As String) As Mech
        If MechSlotsCompulsory.Count > 0 Then Return Nothing
        Mech.Name = mechName
        Return Mech
    End Function

    Public Function AddMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If s <> "Handweapon" Then
            If MechSlotsCompulsory.Contains(s) = False Then Return "Invalid mechpart slot"
            MechSlotsCompulsory.Remove(s)
            MechSlotsFilled.Add(s)
        End If
        Return Mech.AddMechPart(mechpart)
    End Function
    Public Function RemoveMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If s <> "Handweapon" Then
            If MechSlotsFilled.Contains(s) = False Then Return "Invalid mechpart slot"
            MechSlotsFilled.Remove(s)
            MechSlotsCompulsory.Add(s)
        End If
        Return Mech.RemoveMechPart(mechpart)
    End Function
End Class
