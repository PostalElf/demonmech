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

            'generate map obstacles
            .GenerateObstacles()
        End With
        Return bf
    End Function
    Private Sub GenerateObstacles()
        Dim filledSquares As New List(Of xy)
        For x = 0 To XRange
            For y = 0 To YRange
                If Field(x, y) Is Nothing Then filledSquares.Add(New xy(x, y))
            Next
        Next

        Dim numberOfObstacles As Integer = 4
        For n = 1 To numberOfObstacles
            'determine random obstacle to be placed based on terrain
            Dim obstacle As BattleObstacle = GetRandomObstacle()
            Dim obstacleSize As xy = GetObstacleSize(obstacle)
            Dim obstacleXWidth As Integer = obstacleSize.X
            Dim obstacleYWidth As Integer = obstacleSize.Y

            'determine targetsquare; it will always be top-left of the rest of the obstacle structure
            'all obstacles therefore spread right and down
            Dim targetSquare As xy = GetRandom(Of xy)(filledSquares)

            'check if obstacle placement is ok
            If CheckPlacement(targetSquare, obstacleXWidth, obstacleYWidth) = True Then

            End If
        Next
    End Sub
    Private Function GetRandomObstacle() As BattleObstacle

    End Function
    Private Function GetObstacleSize(ByVal obstacle As BattleObstacle) As xy
        Select Case obstacle.Name
            Case "Tank Trap" : Return New xy(1, 1)
        End Select
    End Function
    Private Function CheckPlacement(ByVal targetSquare As xy, ByVal xWidth As Integer, ByVal yWidth As Integer) As Boolean
        'xWidth - 1 because size (1,1) is the same square
        For x = targetSquare.X To (targetSquare.X + xWidth - 1)
            For y = targetSquare.Y To (targetSquare.Y + yWidth - 1)
                If x < 0 OrElse x > XRange Then Return False
                If y < 0 OrElse y > YRange Then Return False
                If Field(x, y) Is Nothing = False Then Return False
            Next
        Next
        Return True
    End Function
    Private Sub PlaceObstacle(ByVal startSquare As xy, ByVal xWidth As Integer, ByVal yWidth As Integer, ByVal obstacle As BattleObstacle)
        For x = startSquare.X To (startSquare.X + xWidth - 1)
            For y = startSquare.Y To (startSquare.Y + yWidth - 1)
                Field(x, y) = BattleObstacle.Construct(obstacle)
            Next
        Next
    End Sub

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

    Private Structure xy
        Public X As Integer
        Public Y As Integer

        Public Sub New(ByVal _x As Integer, ByVal _y As Integer)
            X = _x
            Y = _y
        End Sub
    End Structure
End Class
