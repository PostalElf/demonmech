Public Class Battlefield
    Private Field As BattleObject(,)
    Private Terrain As BattlefieldTerrain
    Private XRange As Integer
    Private YRange As Integer

    Private Camera As Camera
    Private Mech As Mech

    Public Shared Function Construct(ByVal mech As Mech, ByVal xRange As Integer, ByVal yRange As Integer, ByVal camera As Camera, ByVal terrain As BattlefieldTerrain, Optional ByVal obstacleDensity As Integer = 50) As Battlefield
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
            Dim squareArea As Integer = (xRange - 1) * (yRange - 1)
            Dim density As Integer = squareArea * obstacleDensity / 100
            .GenerateObstacles(density)
        End With
        Return bf
    End Function
    Private Sub GenerateObstacles(ByVal density As Integer)
        Dim emptySquares As New List(Of xy)
        For x = 0 To XRange
            For y = 0 To YRange
                If Field(x, y) Is Nothing Then emptySquares.Add(New xy(x, y))
            Next
        Next

        Dim numberOfObstacles As Integer = density
        For n = 1 To numberOfObstacles
            'determine random obstacle to be placed based on terrain
            Dim obstacle As BattleObstacle = BattleObstacle.GetRandomObstacle(Terrain)

            'determine targetsquare; it will always be top-left of the rest of the obstacle structure
            'all obstacles therefore spread right and down
            Dim targetSquare As xy = GetRandom(Of xy)(emptySquares)

            'check if obstacle placement is ok
            Dim failureCount As Integer = 5                     'number of times the system should try to place the obstacle before giving up
            While CheckPlacement(targetSquare, obstacle, emptySquares) = False
                targetSquare = GetRandom(Of xy)(emptySquares)
                failureCount -= 1
                If failureCount <= 0 Then Exit Sub
            End While

            'add obstacle
            PlaceObstacle(targetSquare, obstacle)

            'remove obstacle and margin from emptySquares
            For x = targetSquare.X - 1 To targetSquare.X + obstacle.XWidth + 1
                If x < 0 OrElse x > XRange Then Continue For
                For y = targetSquare.Y - 1 To targetSquare.Y + obstacle.YWidth + 1
                    If y < 0 OrElse y > YRange Then Continue For
                    Dim newEmptySquare As xy = GetEmptySquare(emptySquares, x, y)
                    If emptySquares.Contains(newEmptySquare) Then emptySquares.Remove(newEmptySquare)
                Next
            Next
        Next
    End Sub
    Private Function CheckPlacement(ByVal targetSquare As xy, ByVal obstacle As BattleObstacle, ByVal emptySquares As List(Of xy)) As Boolean
        'set reference point for xy nothing
        Dim xyNothing As New xy(-1, -1)

        'xWidth - 1 because size (1,1) is a single square
        For x = targetSquare.X To (targetSquare.X + obstacle.XWidth - 1)
            For y = targetSquare.Y To (targetSquare.Y + obstacle.YWidth - 1)
                If x < 0 OrElse x > XRange Then Return False
                If y < 0 OrElse y > YRange Then Return False
                If GetEmptySquare(emptySquares, x, y) = xyNothing Then Return False
            Next
        Next
            Return True
    End Function
    Private Sub PlaceObstacle(ByVal startSquare As xy, ByVal obstacle As BattleObstacle)
        For x = startSquare.X To (startSquare.X + obstacle.XWidth - 1)
            For y = startSquare.Y To (startSquare.Y + obstacle.YWidth - 1)
                Field(x, y) = BattleObstacle.Construct(obstacle)
            Next
        Next
    End Sub
    Private Function GetEmptySquare(ByRef list As List(Of xy), ByVal x As Integer, ByVal y As Integer) As xy
        For Each sq In list
            If sq.X = x AndAlso sq.Y = y Then Return sq
        Next
        Return New xy(-1, -1)
    End Function

    Public Sub ConsoleWrite()
        For y = Mech.Y - Camera.YRange To Mech.Y + Camera.YRange
            For x = Mech.X - Camera.XRange To Mech.X + Camera.XRange
                If (x < 0 OrElse x > XRange) OrElse (y < 0 OrElse y > YRange) Then
                    'out of bounds, display asterix
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.Write("*")
                Else
                    If Field(x, y) Is Nothing Then
                        'empty field, display dot
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.Write(".")
                    Else
                        'filled field, display character
                        Console.ForegroundColor = Field(x, y).CColour
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
    Public Function GetSquares(ByVal originX As Integer, ByVal originY As Integer, ByVal range As Integer, ByVal direction As Char) As List(Of BattleObject)
        If range = 0 Then Return Nothing

        Dim total As New List(Of BattleObject)
        Select Case direction
            Case "N"c
                For y = (originY - 1) To (originY - range) Step -1
                    If y < 0 Then Exit For
                    total.Add(Field(originX, y))
                Next
            Case "S"c
                For y = (originY + 1) To (originY + range)
                    If y > YRange Then Exit For
                    total.Add(Field(originX, y))
                Next
            Case "E"c
                For x = (originX + 1) To (originX + range)
                    If x > XRange Then Exit For
                    total.Add(Field(x, originY))
                Next
            Case "W"c
                For x = (originX - 1) To (originX - range) Step -1
                    If x < 0 Then Exit For
                    total.Add(Field(x, originY))
                Next
            Case Else : Throw New Exception("Unexpected direction character") : Return Nothing
        End Select
        Return total
    End Function
    Public Sub EndPlayerTurn()
        Mech.EndTurn()
    End Sub

    Public Structure xy
        Public X As Integer
        Public Y As Integer

        Public Sub New(ByVal _x As Integer, ByVal _y As Integer)
            X = _x
            Y = _y
        End Sub
        Public Overrides Function ToString() As String
            Return X & "," & Y
        End Function
        Public Shared Operator =(ByVal a1 As xy, ByVal a2 As xy)
            If a1.X = a2.X AndAlso a1.Y = a2.Y Then Return True Else Return False
        End Operator
        Public Shared Operator <>(ByVal a1 As xy, ByVal a2 As xy)
            If a1 = a2 Then Return False Else Return True
        End Operator
    End Structure
End Class
