Public MustInherit Class BattleCombatant
    Inherits BattleObject
    Public ActionPoints As Integer
    Protected MustOverride ReadOnly Property ActionPointsMax As Integer
    Protected _IsCrusher As Boolean = False
    Public ReadOnly Property IsCrusher As Boolean
        Get
            Return _IsCrusher
        End Get
    End Property
End Class
