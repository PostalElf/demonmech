Public Class BattleObstacle
    Inherits BattleObject
    Public Shadows Property C As Char
    Public Cover As BattleObstacleCover
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
                Case "Factory"
                    .C = "F"
                    .IsCrushable = False
                    .Cover = BattleObstacleCover.Full
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
End Class
