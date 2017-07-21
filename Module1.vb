Module Module1

    Sub Main()
        Console.SetWindowSize(150, 50)

        Dim lasergunBlueprint As Blueprint = Blueprint.Load("Lasergun")
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Crystal"))
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Circuit"))
        lasergunBlueprint.AddComponent(Component.Load("Revolver Chasis"))
        Dim lasergun As MechPart = lasergunBlueprint.ConstructMechPart("Laser Revolver")

        Dim mechArmBlueprint As Blueprint = Blueprint.Load("Mechanoid Arm")
        mechArmBlueprint.AddComponent(Component.Load("EM Motor"))
        mechArmBlueprint.AddComponent(Component.Load("Nanosteel Plate"))
        Dim mechArm As MechPart = mechArmBlueprint.ConstructMechPart("Mech Arm")

        Dim chasisBlueprint As Blueprint = Blueprint.Load("Nanocarbon Mech Chasis")
        chasisBlueprint.AddComponent(Component.Load("Nanosteel Plate"))
        Dim chasis As MechPart = chasisBlueprint.ConstructMechPart("Chasis")

        Dim slothDesign As MechDesign = MechDesign.Load("Sloth")
        slothDesign.AddMechPart(mechArm)
        slothDesign.AddMechPart(chasis)
        slothDesign.AddMechPart(lasergun)
        Dim mech As Mech = slothDesign.ConstructMech("Sloth v1")

        Dim battlefield As Battlefield = battlefield.Construct(mech, 50, 50, New Camera(10, 10), BattlefieldTerrain.Wasteland, 80)
        mech.EndTurn()
        Dim enemy As New Enemy
        battlefield.PlaceObject(mech.X, mech.Y - 2, enemy)
        While True
            battlefield.ConsoleWrite()
            Console.ForegroundColor = ConsoleColor.White
            mech.ConsoleWriteReport()
            Select Case Console.ReadKey.Key
                Case ConsoleKey.NumPad8 : mech.MoveCombatant(battlefield, "N"c)
                Case ConsoleKey.NumPad4 : mech.MoveCombatant(battlefield, "W"c)
                Case ConsoleKey.NumPad6 : mech.MoveCombatant(battlefield, "E"c)
                Case ConsoleKey.NumPad2 : mech.MoveCombatant(battlefield, "S"c)
                Case ConsoleKey.A : Attack(battlefield, mech)
                Case ConsoleKey.X : mech.ConsoleWriteReportExamine()
                Case ConsoleKey.E : EquipWeapon(mech)
                Case ConsoleKey.D : TestDamageCombatLimb(mech)
                Case ConsoleKey.Enter : mech.EndTurn()
            End Select
            Console.Clear()
        End While
    End Sub
    Private Function ChooseFromList(ByVal combatant As BattleCombatant, ByVal listName As String, ByVal prompt As String) As Integer
        Dim selection As Integer = 0
        While True
            Console.WriteLine()
            combatant.ConsoleWrite(listName)
            Console.Write(prompt)
            Dim input As String = Console.ReadLine
            If IsNumeric(input) = True Then selection = Convert.ToInt32(input) : Exit While
        End While

        selection -= 1                          'indexes start at 0, not 1
        Return selection
    End Function
    Private Sub EquipWeapon(ByVal mech As Mech)
        Dim selection As Integer = ChooseFromList(mech, "HandWeaponsInventory", "Select weapon to equip: ")
        If selection = -1 Then Exit Sub
        mech.EquipHandWeapon(selection)
    End Sub
    Private Sub TestDamageCombatLimb(ByVal mech As Mech)
        Dim selection As Integer = ChooseFromList(mech, "CombatLimbs", "Select limb to damage: ")
        If selection = -1 Then Exit Sub
        mech.TargetedByAttack(selection, 200, 1, DamageType.Slashing)
    End Sub
    Private Sub Attack(ByVal battlefield As Battlefield, ByVal mech As Mech)
        Dim selection As Integer = ChooseFromList(mech, "Weapons", "Select weapon to use: ")
        If selection = -1 Then Exit Sub
        Dim weapon As MechPart = mech.Weapons(selection)

        Dim targets As List(Of BattleCombatant) = mech.WeaponTargets(battlefield, weapon)
        If targets.Count = 0 Then
            Console.WriteLine("No available targets!")
            Console.ReadKey()
            Exit Sub
        End If

        While True
            Console.WriteLine()
            Dim counter As Integer = 1
            For Each t In targets
                Console.WriteLine(counter & ") " & t.Report)
                counter += 1
            Next
            Console.Write("Select target: ")
            Dim input As String = Console.ReadLine
            If IsNumeric(input) = True Then selection = Convert.ToInt32(input) : Exit While
        End While
        selection -= 1
        If selection = -1 Then Exit Sub
        Dim target As BattleCombatant = targets(selection)

        Dim targetLimbIndex As Integer = ChooseFromList(target, "CombatLimbs", "Select target limb: ")
        If targetLimbIndex = -1 Then Exit Sub
        Console.WriteLine(target.TargetedByAttack(targetLimbIndex, weapon))
    End Sub
End Module
