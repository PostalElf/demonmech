Public MustInherit Class BattleObject
    Public Name As String
    Public Battlefield As Battlefield
    Public X As Integer
    Public Y As Integer
    Public ReadOnly Property C As Char
        Get
            Select Case Me.GetType
                Case GetType(Mech) : Return "@"
                Case GetType(Enemy) : Return CType(Me, Enemy).C
                Case GetType(BattleObstacle) : Return CType(Me, BattleObstacle).C
                Case Else : Return " "
            End Select
        End Get
    End Property
    Public ReadOnly Property CColour As ConsoleColor
        Get
            Select Case Me.GetType
                Case GetType(Mech) : Return ConsoleColor.White
                Case GetType(BattleObstacle) : Return CType(Me, BattleObstacle).CColour
                Case Else : Return ConsoleColor.Gray
            End Select
        End Get
    End Property
End Class
