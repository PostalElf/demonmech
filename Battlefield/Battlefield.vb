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

        If Field(battleObject.X, battleObject.Y) Is Nothing = False Then
            'existing object on field
            'remove from original square first
            Field(battleObject.X, battleObject.Y) = Nothing
        End If

        battleObject.X = x
        battleObject.Y = y
        Field(x, y) = battleObject
    End Sub
    Public Function CheckMove(ByVal x As Integer, ByVal y As Integer, ByVal battlecombatant As BattleCombatant) As Boolean
        'check bounds
        If x < 0 Then Return False
        If x > XRange Then Return False
        If y < 0 Then Return False
        If y > YRange Then Return False

        'check if square is filled
        If Field(x, y) Is Nothing = False Then
            If TypeOf (Field(x, y)) Is BattleObstacle Then
                'square filled with obstacle
                'return true if obstacle is crushable and combatant is crusher; else return false
                Dim obstacle As BattleObstacle = CType(Field(x, y), BattleObstacle)
                If obstacle.IsCrushable = True AndAlso battlecombatant.IsCrusher = True Then Return True Else Return False
            Else
                'square filled with something that cannot be crushed; return false
                Return False
            End If
        Else
            'square is empty
            Return True
        End If
    End Function
    Public Sub EndPlayerTurn()
        Mech.EndTurn()
    End Sub
End Class
