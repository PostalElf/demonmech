Public Class BattleObstacle
    Inherits BattleObject
    Public Shadows Property C As Char
    Public Cover As BattleObstacleCover
    Public XWidth As Integer
    Public YWidth As Integer
    Public IsCrushable As Boolean = False

    Public Shared Function Construct(ByVal obstacleName As String) As BattleObstacle
        Dim obstacle As New BattleObstacle
        With obstacle
            .Name = obstacleName
            Select Case obstacleName
                Case "Tank Trap"
                    .C = "^"
                    .IsCrushable = True
                    .Cover = BattleObstacleCover.Low
                    .XWidth = 1
                    .YWidth = 1
                Case "Factory"
                    .C = "F"
                    .IsCrushable = False
                    .Cover = BattleObstacleCover.Full
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
            .IsCrushable = obstacle.IsCrushable
            .Cover = obstacle.Cover
        End With
        Return total
    End Function
    Public Shared Function GetRandomObstacle(ByVal terrain As BattlefieldTerrain) As BattleObstacle

    End Function
End Class
