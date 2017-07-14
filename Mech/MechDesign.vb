Public Class MechDesign
    Private Name As String
    Private MechSlotsCompulsory As New List(Of String)
    Private MechSlotsFilled As New List(Of String)
    Private MechParts As New List(Of MechPart)
    Private MechDesignModifiers As New MechPart                 'hidden container that holds all the modifiers from the design
    Private HandWeaponsInventory As New List(Of MechPart)
    Private ReadOnly Property HandWeaponsSpace As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                total += mp.HandWeaponSpace
            Next
            total += MechDesignModifiers.HandWeaponSpace

            For Each hw In HandWeaponsInventory
                total -= hw.HandSpace
            Next

            Return total
        End Get
    End Property

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
            Case "Inventory" : MechDesignModifiers.HandWeaponSpace += CInt(value)
        End Select
    End Sub
    Public Function ConstructMech(ByVal mechName As String) As Mech
        If MechSlotsCompulsory.Count > 0 Then Return Nothing
        Return Mech.Construct(mechName, Name, MechParts, HandWeaponsInventory, MechDesignModifiers)
    End Function

    Public Function AddMechPart(ByVal mechpart As MechPart) As String
        Dim s As String = mechpart.Slot
        If s = "Handweapon" Then
            If HandWeaponsSpace - mechpart.HandSpace < 0 Then Return "Insufficient inventory space"
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
