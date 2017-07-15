Public Class BattleObstacle
    Inherits BattleObject
    Public Shadows ReadOnly Property C As Char
        Get
            Select Case Name
                Case "Shrub" : Return ";"
                Case "Tank Trap" : Return "^"
                Case "House" : Return "H"
                Case "Factory" : Return "F"
                Case Else : Return "."
            End Select
        End Get
    End Property
    Public IsCrushable As Boolean = False

    Public Shared Function Construct(ByVal terrain As BattlefieldTerrain) As BattleObstacle
        Dim obstacle As New BattleObstacle
        With obstacle
            Select Case terrain
                Case BattlefieldTerrain.Wasteland
                    Select Case Rng.Next(1, 101)
                        Case 1 To 20 : .Name = "Shrub" : .IsCrushable = True
                        Case 21 To 40 : .Name = "Tank Trap" : .IsCrushable = True
                        Case 41 To 60 : .Name = "House" : .IsCrushable = True
                        Case 61 To 100 : .Name = "Factory"
                    End Select


            End Select
        End With
        Return obstacle
    End Function
End Class
