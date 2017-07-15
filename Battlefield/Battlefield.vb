Public Class Battlefield
    Private Field As BattleObject(,)
    Private Terrain As BattlefieldTerrain
    Private XRange As Integer
    Private YRange As Integer

    Private Camera As Camera
    Private Mech As Mech

    Public Shared Function Construct(ByVal mech As Mech, ByVal xRange As Integer, ByVal yRange As Integer, ByVal camera As Camera, ByVal terrain As BattlefieldTerrain) As Battlefield
        Dim bf As New Battlefield
        With bf
            'initialise camera
            .Camera = camera

            'create 2D array to represent field
            .XRange = xRange
            .YRange = yRange
            ReDim .Field(xRange, yRange)

            'place player mech
            Dim mechY As Integer = yRange
            Dim xMidpoint As Integer = Math.Floor(xRange / 2)
            Dim mechX As Integer = xMidpoint + Rng.Next(-2, 2)
            If mechX > xRange Then mechX = xRange
            .Mech = mech
            .PlaceObject(mechX, mechY, .Mech)

            'place obstacle 2 in front of mech
            Dim obstacle As BattleObstacle = BattleObstacle.Construct(terrain)
            .PlaceObject(mechX, mechY - 2, obstacle)
        End With
        Return bf
    End Function
    Public Sub ConsoleWrite()
        For y = Mech.Y - Camera.YRange To Mech.Y + Camera.YRange
            For x = Mech.X - Camera.XRange To Mech.X + Camera.XRange
                If (x < 0 OrElse x > XRange) OrElse (y < 0 OrElse y > YRange) Then
                    'out of bounds, display asterix
                    Console.Write("*")
                Else
                    If Field(x, y) Is Nothing Then
                        'empty field, display dot
                        Console.Write(".")
                    Else
                        'filled field, display character
                        Console.Write(Field(x, y).C)
                    End If
                End If
            Next
            Console.WriteLine()         'newline
        Next
    End Sub

    Public Sub PlaceObject(ByVal x As Integer, ByVal y As Integer, ByRef battleObject As BattleObject)
        If x < 0 OrElse x > XRange OrElse y < 0 OrElse y > YRange Then Exit Sub

        battleObject.X = x
        battleObject.Y = y
        Field(x, y) = battleObject
    End Sub
End Class
