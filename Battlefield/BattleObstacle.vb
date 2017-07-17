Public Class BattleObstacle
    Inherits BattleObject
    Public Shadows Property C As Char
    Public Cover As BattleObstacleCover
    Public IsCrushable As Boolean = False

    Public Shared Function Construct(ByVal obstacleName As String) As BattleObstacle
        Dim obstacle As New BattleObstacle
        With obstacle
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
End Class
