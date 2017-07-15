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
                Case ConsoleKey.NumPad8 : sloth.MoveCombatant(battlefield, "N"c)
                Case ConsoleKey.NumPad4 : sloth.MoveCombatant(battlefield, "W"c)
                Case ConsoleKey.NumPad6 : sloth.MoveCombatant(battlefield, "E"c)
                Case ConsoleKey.NumPad2 : sloth.MoveCombatant(battlefield, "S"c)
                Case ConsoleKey.Enter : sloth.EndTurn()
            End Select
            Console.Clear()
        End While
    End Sub
End Module
