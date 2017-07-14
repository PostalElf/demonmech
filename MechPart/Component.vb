Public Class Component
    Public Name As String
    Public Category As String
    Public Weight As Integer
    Public Agility As Integer
    Public Accuracy As Integer
    Public Range As Integer
    Public Hands As Integer
    Public DamageAmount As Integer
    Public DamageType As DamageType
    Public ReadOnly Property IsNotEmpty As Boolean
        Get
            'used to check if the component has values instead of being just an empty shell
            'especially important for BlueprintModifiers

            If Weight <> 0 Then Return True
            If Agility <> 0 Then Return True
            If Accuracy <> 0 Then Return True
            If Range <> 0 Then Return True
            If DamageAmount <> 0 Then Return True
            If DamageType <> 0 Then Return True
            Return False
        End Get
    End Property

    Public Shared Function Load(ByVal targetname As String) As Component
        Const path As String = "data/components.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, targetname)

        Dim component As New Component
        With component
            .Name = q.Dequeue

            While q.Count > 0
                Dim line As String() = q.Dequeue.Split(":")
                Dim key As String = line(0).Trim
                Dim value As String = line(1).Trim
                .Construct(key, value)
            End While
        End With
        Return component
    End Function
    Public Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Category" : Category = value
            Case "Weight" : Weight = CInt(value)
            Case "Agility" : Agility = CInt(value)
            Case "Accuracy" : Accuracy = CInt(value)
            Case "Range" : Range = CInt(value)
            Case "Hands" : Hands = CInt(value)
            Case "DamageAmount" : DamageAmount = CInt(value)
            Case "DamageType"
                For Each dt In [Enum].GetValues(GetType(DamageType))
                    If dt.ToString = value Then DamageType = dt
                Next
        End Select
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
