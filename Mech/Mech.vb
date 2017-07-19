Public Class Mech
    Inherits BattleCombatant
    Private DesignName As String
    Private MechParts As New List(Of MechPart)
    Private MechDesignModifiers As Component
    Private ReadOnly Property Weight As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                total += mp.Weight
            Next
            total += MechDesignModifiers.Weight
            Return total
        End Get
    End Property
    Private ReadOnly Property Agility As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.Agility
            Next
            total += MechDesignModifiers.Agility
            Return total
        End Get
    End Property
    Private ReadOnly Property Hands As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.ExtraHands
            Next
            total += MechDesignModifiers.ExtraHands
            Return total
        End Get
    End Property
    Private ReadOnly Property HandsUsed As Integer
        Get
            Dim total As Integer = 0
            For Each hw In HandWeaponsEquipped
                If hw.IsDestroyed = False Then total += hw.HandSpace
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property HandsFree As Integer
        Get
            Return Hands - HandsUsed
        End Get
    End Property
    Private ReadOnly Property InventorySpace As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.InventorySpace
            Next
            total += MechDesignModifiers.InventorySpace
            Return total
        End Get
    End Property
    Private ReadOnly Property InventorySpaceUsed As Integer
        Get
            Dim total As Integer = 0
            For Each hw In HandWeaponsInventory
                If hw.IsDestroyed = False Then total += hw.HandSpace
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property InventorySpaceFree As Integer
        Get
            Return InventorySpace - InventorySpaceUsed
        End Get
    End Property
    Protected Overrides ReadOnly Property ActionPointsMax As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.AP
            Next
            total += MechDesignModifiers.AP
            Return total
        End Get
    End Property
    Private ReadOnly Property APPerSeal As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.APPerSeal
            Next
            total += MechDesignModifiers.APPerSeal
            Return total
        End Get
    End Property
    Protected Overrides ReadOnly Property MovementPointsMax As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.MP
            Next
            total += MechDesignModifiers.MP
            Return total
        End Get
    End Property
    Private ReadOnly Property MPPerSeal As Integer
        Get
            Dim total As Integer = 0
            For Each mp In MechParts
                If mp.IsDestroyed = False Then total += mp.MPPerSeal
            Next
            total += MechDesignModifiers.MPPerSeal
            Return total
        End Get
    End Property

    Private HandWeaponsInventory As New List(Of MechPart)
    Private HandWeaponsEquipped As New List(Of MechPart)
    Public ReadOnly Property Weapons As List(Of MechPart)
        Get
            Dim total As New List(Of MechPart)
            For Each hw In HandWeaponsEquipped
                If hw.IsWeapon = True Then total.Add(hw)
            Next
            For Each mp In MechParts
                If mp.IsDestroyed = False AndAlso mp.IsWeapon = True Then total.Add(mp)
            Next
            Return total
        End Get
    End Property
    Public ReadOnly Property WeaponTargets(ByVal battlefield As Battlefield, ByVal weapon As MechPart) As List(Of BattleCombatant)
        Get
            Dim total As New List(Of BattleCombatant)
            Dim directions As Char() = {"N"c, "E"c, "S"c, "W"c}
            Dim range As Integer = weapon.Range

            For Each d In directions
                Dim squares As List(Of BattleObject) = battlefield.GetSquares(X, Y, range, d)
                Dim highestCover As BattleObstacleCover = BattleObstacleCover.None
                For Each square In squares
                    If square Is Nothing Then Continue For
                    If TypeOf square Is BattleObstacle Then
                        'if there's an obstacle, add it to highest cover
                        Dim obstacle As BattleObstacle = CType(square, BattleObstacle)
                        If highestCover < obstacle.Cover Then highestCover = obstacle.Cover
                    ElseIf TypeOf square Is BattleCombatant Then
                        'if it's a combatant, check if there's cover previously in the way
                        'if there's cover, and weapon ignores the cover, add target to list
                        If weapon.CoverIgnore >= highestCover Then total.Add(square)
                    End If
                Next
            Next
            Return total
        End Get
    End Property

    Public Shared Function Construct(ByVal mechDesignName As String, ByRef mechDesignModifiers As Component) As Mech
        Dim mech As New Mech
        With mech
            .DesignName = mechDesignName
            .MechDesignModifiers = mechDesignModifiers
            ._IsCrusher = True
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
            CombatLimbs.Add(CombatLimb.Construct(mechpart))
            mechpart.Owner = Me
            Return Nothing
        Else
            MechParts.Add(mechpart)
            CombatLimbs.Add(CombatLimb.Construct(mechpart))
            mechpart.Owner = Me
            Return Nothing
        End If
    End Function
    Public Function RemoveMechPart(ByVal mechpart As MechPart) As String
        If mechpart.Slot = "Handweapon" Then
            If HandWeaponsInventory.Contains(mechpart) = False Then Return "Mechpart not in inventory"
            HandWeaponsInventory.Remove(mechpart)
            mechpart.Owner = Me
            Return Nothing
        Else
            If MechParts.Contains(mechpart) = False Then Return "Mechpart not on mech"
            MechParts.Remove(mechpart)
            mechpart.Owner = Me
            Return Nothing
        End If
    End Function
    Public Function EquipHandWeapon(ByVal index As Integer) As String
        If index > HandWeaponsInventory.Count - 1 OrElse index < 0 Then Return "Invalid handweaponinventory index"
        Return EquipHandWeapon(HandWeaponsInventory(0))
    End Function
    Public Function EquipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandsFree < mechpart.HandSpace Then Return "Insufficient hands"
        HandWeaponsEquipped.Add(mechpart)
        HandWeaponsInventory.Remove(mechpart)
        Return Nothing
    End Function
    Public Function UnequipHandWeapon(ByVal mechpart As MechPart) As String
        If HandWeaponsInventory.Contains(mechpart) = False Then Return "Weapon not in inventory"
        If HandWeaponsEquipped.Contains(mechpart) = False Then Return "Weapon not equipped"
        HandWeaponsEquipped.Remove(mechpart)
        HandWeaponsInventory.Add(mechpart)
        Return Nothing
    End Function
    Public Overrides Sub RemoveCombatLimb(ByVal CombatLimb As CombatLimb)
        CombatLimbs.Remove(CombatLimb)
    End Sub
    Public Overrides Sub RemoveCombatLimb(ByVal index As Integer)
        RemoveCombatLimb(CombatLimbs(index))
    End Sub
    Public Overrides Function TargetedByAttack(ByVal LimbIndex As Integer, ByVal accuracy As Integer, ByVal damage As Integer, ByVal damagetype As DamageType) As String
        Return CombatLimbs(LimbIndex).TargetedByAttack(accuracy, damage, damagetype)
    End Function
    Public Sub EndTurn()
        ActionPoints = ActionPointsMax
        MovementPoints = MovementPointsMax
    End Sub

    Public Sub ConsoleWriteReport()
        Dim total As String = "  " & Name & " - "
        If MovementPoints > 0 Then total &= MovementPoints.ToString("00") & " MP - "
        total &= ActionPoints.ToString("00") & " AP"

        Console.WriteLine(total)
    End Sub
    Public Sub ConsoleWriteReportExamine()
        Console.WriteLine()
        Console.WriteLine(Name & " [" & DesignName & "]")
        Console.WriteLine(" └ WGT  : " & Weight)
        Console.WriteLine(" └ AGI  : " & Agility)
        Console.WriteLine(" └ AP   : " & ActionPoints & "/" & ActionPointsMax)
        Console.WriteLine(" └ HAND : " & Hands)
        Console.WriteLine(" └ INV  : " & InventorySpaceUsed & "/" & InventorySpace)
        For Each hw In HandWeaponsEquipped
            Console.WriteLine("    └ " & hw.Report)
        Next
        Console.WriteLine(" └ MECH PARTS:")
        For Each cl In CombatLimbs
            Console.WriteLine("    └ " & cl.Report)
        Next
        Console.ReadKey()
    End Sub
    Public Sub ConsoleWriteHandWeaponsInventory()
        Dim counter As Integer = 1
        For Each hwp In HandWeaponsInventory
            Console.WriteLine(counter & ") " & hwp.Report)
            counter += 1
        Next
    End Sub
    Public Overrides Sub ConsoleWriteCombatLimbs()
        Dim counter As Integer = 1
        For Each cl In CombatLimbs
            Console.WriteLine(counter & ") " & cl.Report)
            counter += 1
        Next
    End Sub
End Class
