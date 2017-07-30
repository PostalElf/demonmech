Public Class Enemy
    Inherits BattleCombatant
    Private _C As Char
    Public Shadows ReadOnly Property C As Char
        Get
            Return _C
        End Get
    End Property
    Private _ActionPointsMax As Integer
    Protected Overrides ReadOnly Property ActionPointsMax As Integer
        Get
            Return _ActionPointsMax
        End Get
    End Property
    Private _MovementPointsMax As Integer
    Protected Overrides ReadOnly Property MovementPointsMax As Integer
        Get
            Return _MovementPointsMax
        End Get
    End Property

    Public Overrides ReadOnly Property Weapons As List(Of MechPart)
        Get
            Dim total As New List(Of MechPart)
            For Each mp In MechParts
                If mp.IsDestroyed = False AndAlso mp.IsWeapon = True Then total.Add(mp)
            Next
            Return total
        End Get
    End Property

    Public Shared Function Load(ByVal enemyName As String)
        Const path = "data/enemies.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, enemyName)

        Dim enemy As New Enemy
        With enemy
            .Name = q.Dequeue()
            While q.Count > 0
                Dim line As String() = q.Dequeue.Split(":")
                Dim key As String = line(0).Trim
                Dim value As String = line(1).Trim
                .Construct(key, value)
            End While
        End With
        Return enemy
    End Function
    Private Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Char" : _C = CChar(value)
            Case "AP" : _ActionPointsMax = CInt(value)
            Case "MP" : _MovementPointsMax = CInt(value)
            Case "Limb"
                Dim mechPart As MechPart = mechPart.Construct(value.Split("|"))
                If mechPart Is Nothing = True Then Exit Sub
                mechPart.Owner = Me
                MechParts.Add(mechPart)
                CombatLimbs.Add(CombatLimb.Construct(mechPart))
        End Select
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
    Public Overrides Sub ConsoleWrite(ByVal targetListName As String)
        Dim targetList As System.Collections.Generic.IEnumerable(Of iReportable)
        Select Case targetListName
            Case "CombatLimbs" : targetList = CombatLimbs
            Case "Weapons" : targetList = Weapons
            Case Else : Exit Sub
        End Select

        Dim counter As Integer = 1
        For Each thing In targetList
            Console.WriteLine(counter & ") " & thing.Report)
            counter += 1
        Next
    End Sub

    Public Sub TakeTurn(ByVal mech As Mech)

        'end turn
        EndTurn()
    End Sub
End Class
