Public Class Mech
    Private Name As String
    Private DesignName As String
    Private MechParts As New Dictionary(Of String, List(Of MechPart))
    Private MechPartsSpace As New Dictionary(Of String, Integer)

    Private HandWeaponsSpace As Integer
    Private HandWeapons As New List(Of MechPart)
    Private HandWeaponsEquipped As New List(Of MechPart)
    Private Hands As Integer
    Private ReadOnly Property UsedHands As Integer
        Get
            Dim total As Integer = 0
            For Each hw In HandWeaponsEquipped
                total += hw.HandSpace
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property FreeHands As Integer
        Get
            Return Hands - UsedHands
        End Get
    End Property

    Public Shared Function Construct(ByVal mechName As String, ByVal mechDesignName As String, ByVal mechparts As List(Of MechPart)) As Mech
        Dim mech As New Mech
        With mech
            .Name = mechName
            .DesignName = mechDesignName

            For Each mp In mechparts
                .AddMechPart(mp)
            Next
        End With
        Return mech
    End Function

    Private Function AddMechPart(ByVal mechpart As MechPart) As String
        If mechpart.Slot = "Hand" Then
            'hand weapon, add to handWeapons instead of MechParts
            If HandWeaponsSpace < mechpart.HandSpace Then Return "Insufficient weapon space"
            HandWeapons.Add(mechpart)
            HandWeaponsSpace -= mechpart.HandSpace
        Else
            'mechPart
            Dim s As String = mechpart.Slot
            If MechParts.ContainsKey(s) = False Then Return "No such slot"
            If MechParts(s).Count + 1 > MechPartsSpace(s) Then Return "Insufficient slot space"
            MechParts(s).Add(mechpart)
        End If
        Return Nothing
    End Function
    Private Function RemoveMechPart(ByVal mechpart As MechPart) As String
        If mechpart.Slot = "Hand" Then
            If HandWeapons.Contains(mechpart) = False Then Return "No such part"
            HandWeapons.Remove(mechpart)
            HandWeaponsSpace += mechpart.HandSpace
        Else
            Dim s As String = mechpart.Slot
            If MechParts.ContainsKey(s) = False Then Return "No such slot"
            If MechParts(s).Contains(mechpart) = False Then Return "No such part"
            MechParts(s).Remove(mechpart)
        End If
        Return Nothing
    End Function
    Private Function EquipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeapons.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If FreeHands < mechpart.HandSpace Then Return "Insufficient hands"
        HandWeaponsEquipped.Add(mechpart)
        Return Nothing
    End Function
    Private Function UnequipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeapons.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandWeaponsEquipped.Contains(mechpart) = False Then Return "Weapon not equipped"
        HandWeaponsEquipped.Remove(mechpart)
        Return Nothing
    End Function
End Class
