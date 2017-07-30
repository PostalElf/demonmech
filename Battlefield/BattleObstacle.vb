Public Class BattleObstacle
    Inherits BattleObject
    Public Shadows Property C As Char
    Public Shadows Property CColour As ConsoleColor
    Public Cover As BattleObstacleCover
    Public XWidth As Integer
    Public YWidth As Integer
    Public CrushCost As Integer = 0

    Public Shared Function Construct(ByVal obstacleName As String) As BattleObstacle
        Dim obstacle As New BattleObstacle
        With obstacle
            .Name = obstacleName
            Select Case obstacleName
                Case "Tank Trap"
                    .C = "^"
                    .CColour = ConsoleColor.Gray
                    .CrushCost = 1
                    .Cover = BattleObstacleCover.Low
                    .XWidth = 1
                    .YWidth = 1
                Case "Prefab House"
                    .C = "H"
                    .CColour = ConsoleColor.Gray
                    .CrushCost = 2
                    .Cover = BattleObstacleCover.High
                    .XWidth = 2
                    .YWidth = 2
                Case "Factory"
                    .C = "F"
                    .CColour = ConsoleColor.Gray
                    .CrushCost = -1
                    .Cover = BattleObstacleCover.Total
                    .XWidth = 4
                    .YWidth = 2
                Case Else : Return Nothing
            End Select
        End With
        Return obstacle
    End Function
    Public Shared Function Construct(ByVal obstacle As BattleObstacle) As BattleObstacle
        Dim total As New BattleObstacle
        With total
            .Name = obstacle.Name
            .C = obstacle.C
            .CColour = obstacle.CColour
            .CrushCost = obstacle.CrushCost
            .Cover = obstacle.Cover
        End With
        Return total
    End Function
    Public Shared Function GetRandomObstacle(ByVal terrain As BattlefieldTerrain) As BattleObstacle
        Return Construct("Factory")
    End Function
End Class
