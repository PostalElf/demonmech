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
        sloth.IsCrusher = True
        sloth.EquipHandWeapon(lasergun)

        Dim battlefield As Battlefield = battlefield.Construct(sloth, 10, 10, New Camera(2, 2), BattlefieldTerrain.Wasteland)
        While True
            battlefield.ConsoleWrite()
            Select Case Console.ReadKey.Key
                Case ConsoleKey.NumPad8 : battlefield.MoveCombatant(sloth, "N"c)
                Case ConsoleKey.NumPad4 : battlefield.MoveCombatant(sloth, "W"c)
                Case ConsoleKey.NumPad6 : battlefield.MoveCombatant(sloth, "E"c)
                Case ConsoleKey.NumPad2 : battlefield.MoveCombatant(sloth, "S"c)
            End Select
            Console.Clear()
        End While
    End Sub

End Module
