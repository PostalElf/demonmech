<DebuggerStepThrough()>
Public Module Common
    Public Rng As New Random(5)
    Public Function FormatCommaList(Of T)(ByVal list As List(Of T)) As String
        Dim total As String = ""
        For n = 0 To list.Count - 1
            total &= list(n).ToString
            If n <> list.Count - 1 Then total &= ", "
        Next
        Return total
    End Function
    Public Function GetRandom(Of T)(ByRef list As List(Of T)) As T
        Dim roll As Integer = Rng.Next(list.Count)
        GetRandom = list(roll)
    End Function
    Public Function GrabRandom(Of T)(ByRef list As List(Of T)) As T
        Dim roll As Integer = Rng.Next(list.Count)
        GrabRandom = list(roll)
        list.RemoveAt(roll)
    End Function
    Public Function String2Enum(Of T)(ByVal value As String) As T
        For Each dt In [Enum].GetValues(GetType(T))
            If dt.ToString = value Then Return dt
        Next
        Return Nothing
    End Function
End Module

Public Class AutoIncrementer
    Private Counter As Integer = 0
    Public Function N() As Integer
        N = Counter
        Counter += 1
    End Function
End Class