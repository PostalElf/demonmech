Public Class Mech
    Private Name As String
    Private DesignName As String
    Private MechParts As New List(Of MechPart)
    Private MechDesignModifiers As MechPart

    Private HandWeaponsInventory As New List(Of MechPart)
    Private HandWeaponsEquipped As New List(Of MechPart)
    Private ReadOnly Property Hands As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                total += mp.ExtraHands
            Next
            total += MechDesignModifiers.ExtraHands
            Return total
        End Get
    End Property
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

    Public Shared Function Construct(ByVal mechName As String, ByVal mechDesignName As String, ByVal mechparts As List(Of MechPart), ByVal inventory As List(Of MechPart), ByVal mechDesignModifiers As MechPart) As Mech
        Dim mech As New Mech
        With mech
            .Name = mechName
            .DesignName = mechDesignName
            .MechParts.AddRange(mechparts)
            .HandWeaponsInventory.AddRange(inventory)
            .MechDesignModifiers = mechDesignModifiers
        End With
        Return mech
    End Function
    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Function EquipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If FreeHands < mechpart.HandSpace Then Return "Insufficient hands"
        HandWeaponsEquipped.Add(mechpart)
        Return Nothing
    End Function
    Public Function UnequipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandWeaponsEquipped.Contains(mechpart) = False Then Return "Weapon not equipped"
        HandWeaponsEquipped.Remove(mechpart)
        Return Nothing
    End Function
End Class
