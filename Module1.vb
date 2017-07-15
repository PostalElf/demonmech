Module Module1

    Sub Main()
        Dim lasergunBlueprint As Blueprint = Blueprint.Load("Lasergun")
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Crystal"))
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Circuit"))
        lasergunBlueprint.AddComponent(Component.Load("Revolver Chasis"))
        Dim lasergun As MechPart = lasergunBlueprint.ConstructMechPart

        Dim mechArmBlueprint As Blueprint = Blueprint.Load("Mechanoid Arm")
        mechArmBlueprint.AddComponent(Component.Load("EM Motor"))
        mechArmBlueprint.AddComponent(Component.Load("Nanosteel Plate"))
        Dim mechArm As MechPart = mechArmBlueprint.ConstructMechPart

        Dim chasisBlueprint As Blueprint = Blueprint.Load("Nanocarbon Mech Chasis")
        chasisBlueprint.AddComponent(Component.Load("Nanosteel Plate"))
        Dim chasis As MechPart = chasisBlueprint.ConstructMechPart

        Dim slothDesign As MechDesign = MechDesign.Load("Sloth")
        slothDesign.AddMechPart(mechArm)
        slothDesign.AddMechPart(chasis)
        slothDesign.AddMechPart(lasergun)
        Dim sloth As Mech = slothDesign.ConstructMech("Sloth v1")
        sloth.EquipHandWeapon(lasergun)

        Dim battlefield As Battlefield = battlefield.Construct(sloth, 15, 15, New Camera(2, 2), BattlefieldTerrain.Wasteland)
        sloth.EndTurn()
        While True
            battlefield.ConsoleWrite()
            sloth.ConsoleWriteReport()
            Select Case Console.ReadKey.Key
                Case ConsoleKey.NumPad8 : MoveCombatant(battlefield, sloth, "N"c)
                Case ConsoleKey.NumPad4 : MoveCombatant(battlefield, sloth, "W"c)
                Case ConsoleKey.NumPad6 : MoveCombatant(battlefield, sloth, "E"c)
                Case ConsoleKey.NumPad2 : MoveCombatant(battlefield, sloth, "S"c)
                Case ConsoleKey.Enter : sloth.EndTurn()
            End Select
            Console.Clear()
        End While
    End Sub
    Private Sub MoveCombatant(ByVal battlefield As Battlefield, ByVal mech As Mech, ByVal direction As Char)
        If mech.ActionPoints < 1 Then Exit Sub
        mech.ActionPoints -= 1
        battlefield.MoveCombatant(mech, direction)
    End Sub

End Module
