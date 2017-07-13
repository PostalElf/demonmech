Module Module1

    Sub Main()
        Dim lasergunComponents As New List(Of String)
        With lasergunComponents
            .Add("Focus")
            .Add("Heat Exchanger")
            .Add("Gun Chasis")
        End With
        Dim lasergunBlueprint As Blueprint = Blueprint.Load("Lasergun")
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Crystal"))
        lasergunBlueprint.AddComponent(Component.Load("Hellstone Circuit"))
        lasergunBlueprint.AddComponent(Component.Load("Revolver Chasis"))

        Dim lasergun As MechPart = lasergunBlueprint.ConstructMechPart
    End Sub

End Module
