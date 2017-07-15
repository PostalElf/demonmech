Public Class Mech
    Inherits BattleCombatant
    Private DesignName As String
    Private MechParts As New List(Of MechPart)
    Private MechDesignModifiers As Component

    Private HandWeaponsInventory As New List(Of MechPart)
    Private HandWeaponsEquipped As New List(Of MechPart)
    Private ReadOnly Property InventorySpace As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                total += mp.InventorySpace
            Next
            total += MechDesignModifiers.InventorySpace
            Return total
        End Get
    End Property
    Private ReadOnly Property InventorySpaceUsed As Integer
        Get
            Dim total As Integer = 0
            For Each hw In HandWeaponsInventory
                total += hw.HandSpace
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property InventorySpaceFree As Integer
        Get
            Return InventorySpace - InventorySpaceUsed
        End Get
    End Property
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
    Private ReadOnly Property HandsUsed As Integer
        Get
            Dim total As Integer = 0
            For Each hw In HandWeaponsEquipped
                total += hw.HandSpace
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property HandsFree As Integer
        Get
            Return Hands - HandsUsed
        End Get
    End Property

    Public Shared Function Construct(ByVal mechDesignName As String, ByVal mechDesignModifiers As Component) As Mech
        Dim mech As New Mech
        With mech
            .DesignName = mechDesignName
            .MechDesignModifiers = mechDesignModifiers
        End With
        Return mech
    End Function
    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Function AddMechPart(ByVal mechpart As MechPart) As String
        If mechpart.Slot = "Handweapon" Then
            If InventorySpaceFree - mechpart.HandSpace < 0 Then Return "Insufficient inventory space"
            HandWeaponsInventory.Add(mechpart)
            Return Nothing
        Else
            MechParts.Add(mechpart)
            Return Nothing
        End If
    End Function
    Public Function RemoveMechPart(ByVal mechpart As MechPart) As String
        If mechpart.Slot = "Handweapon" Then
            If HandWeaponsInventory.Contains(mechpart) = False Then Return "Mechpart not in inventory"
            HandWeaponsInventory.Remove(mechpart)
            Return Nothing
        Else
            If MechParts.Contains(mechpart) = False Then Return "Mechpart not on mech"
            MechParts.Remove(mechpart)
            Return Nothing
        End If
    End Function
    Public Function EquipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandsFree < mechpart.HandSpace Then Return "Insufficient hands"
        HandWeaponsEquipped.Add(mechpart)
        Return Nothing
    End Function
    Public Function UnequipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandWeaponsEquipped.Contains(mechpart) = False Then Return "Weapon not equipped"
        HandWeaponsEquipped.Remove(mechpart)
        Return Nothing
    End Function
    Public Sub EndTurn()
        ActionPoints = ActionPointsMax
    End Sub

    Public Sub ConsoleWriteReport()
        Console.WriteLine("  " & Name & " - " & ActionPoints.ToString("00") & " AP")
    End Sub
End Class
