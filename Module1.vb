﻿Module Module1

    Sub Main()
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
        While True
            battlefield.ConsoleWrite()
            Console.ForegroundColor = ConsoleColor.White
            mech.ConsoleWriteReport()
            Select Case Console.ReadKey.Key
                Case ConsoleKey.NumPad8 : mech.MoveCombatant(battlefield, "N"c)
                Case ConsoleKey.NumPad4 : mech.MoveCombatant(battlefield, "W"c)
                Case ConsoleKey.NumPad6 : mech.MoveCombatant(battlefield, "E"c)
                Case ConsoleKey.NumPad2 : mech.MoveCombatant(battlefield, "S"c)
                Case ConsoleKey.A : Attack(mech)
                Case ConsoleKey.X : mech.ConsoleWriteReportExamine()
                Case ConsoleKey.E : EquipWeapon(mech)
                Case ConsoleKey.D : TestDamageCombatLimb(mech)
                Case ConsoleKey.Enter : mech.EndTurn()
            End Select
            Console.Clear()
        End While
    End Sub
    Private Sub EquipWeapon(ByVal mech As Mech)
        Dim selection As Integer = 0
        While True
            Console.WriteLine()
            mech.ConsoleWriteHandWeaponsInventory()
            Console.Write("Select weapon to equip: ")
            Dim input As String = Console.ReadLine
            If IsNumeric(input) = True Then selection = Convert.ToInt32(input) : Exit While
        End While

        selection -= 1                          'indexes start at 0, not 1
        If selection = -1 Then Exit Sub
        mech.EquipHandWeapon(selection)
    End Sub
    Private Sub TestDamageCombatLimb(ByVal mech As Mech)
        Dim selection As Integer = 0
        While True
            Console.WriteLine()
            mech.ConsoleWriteCombatLimbs()
            Console.Write("Select limb to damage: ")
            Dim input As String = Console.ReadLine
            If IsNumeric(input) = True Then selection = Convert.ToInt32(input) : Exit While
        End While

        selection -= 1                          'indexes start at 0, not 1
        If selection = -1 Then Exit Sub
        mech.TargetedByAttack(selection, 200, 5, DamageType.Slashing)
    End Sub
    Private Sub Attack(ByVal mech As Mech)
        Dim selection As Integer = 0
        While True
            Console.WriteLine()
            Dim counter As Integer = 1
            For Each w In mech.Weapons
                Console.WriteLine(counter & ") " & w.Report)
                counter += 1
            Next
            Console.Write("Select weapon to use: ")
            Dim input As String = Console.ReadLine
            If IsNumeric(input) = True Then selection = Convert.ToInt32(input) : Exit While
        End While

        selection -= 1                          'indexes start at 0, not 1
        If selection = -1 Then Exit Sub
        Dim weapon As MechPart = mech.Weapons(selection)

    End Sub
End Module
