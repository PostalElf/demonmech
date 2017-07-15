Public MustInherit Class BattleObject
    Public Name As String
    Public X As Integer
    Public Y As Integer
    Public ReadOnly Property C As Char
        Get
            Select Case Me.GetType
                Case GetType(Mech) : Return "@"
                Case GetType(BattleObstacle) : Return CType(Me, BattleObstacle).C
                Case Else : Return " "
            End Select
        End Get
    End Property
End Class
